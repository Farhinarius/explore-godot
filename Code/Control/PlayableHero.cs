using ExploreGodot.Code;
using ExporeGodot.Code.Enemies;
using Godot;
using System.Diagnostics;

namespace ExporeGodot.Code.Control;

public partial class PlayableHero : CharacterBody3D
{
    [Export]
    private float _speed = 7.5f;
    [Export]
    private float _jumpStrength = 16f;
    [Export]
    private float _gravityForce = 75f;
    [Export]
    private float _bounceImpulse { get; set; } = 16;        // Vertical impulse applied to the character upon bouncing over a mob in meters per second

    private Vector2 _inputLeftCross = Vector2.Zero;
    private Vector3 _moveDirection = Vector3.Zero;
    private Vector2 _lookDirection = Vector2.Down;          // initial setup -> look up
    private Vector3 _velocity = Vector3.Zero;
    private bool _jumpPressed;
    private bool _collisionConditionDetected;

    public float LookDirectionAngle => _lookDirection.Angle() - (Mathf.Pi / 2);

    [Signal]
    public delegate void DeadEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        TranslateInputToMoveDirection();
        ApplyMovement((float)delta);
        ApplyJump();
        SetLookDirection();
        BounceFromEnemy();
    }

    private void TranslateInputToMoveDirection()
    {
        _inputLeftCross.X = Input.GetAxis(InputMapping.LeftCrossHorizontalNegative,
                                 InputMapping.LeftCrossHorizontalPositive);
        _inputLeftCross.Y = Input.GetAxis(InputMapping.LeftCrossVerticalNegative,
                                 InputMapping.LeftCrossVerticalPositive);

        _moveDirection.X = _inputLeftCross.X;
        _moveDirection.Z = -_inputLeftCross.Y;
        _moveDirection = _moveDirection.Normalized();

        _jumpPressed = Input.IsActionJustPressed(InputMapping.Jump);
    }

    private void ApplyMovement(float delta)
    {
        _velocity.X = _moveDirection.X * _speed;
        _velocity.Z = _moveDirection.Z * _speed;
        if (!IsOnFloor())
            _velocity.Y -= _gravityForce * delta;

        Velocity = _velocity;
        MoveAndSlide();
    }

    private void ApplyJump()
    {
        if (IsOnFloor() && _jumpPressed)
        {
            _velocity.Y = _jumpStrength;
        }
        Velocity = _velocity;
    }

    private void BounceFromEnemy()
    {
        // Iterate through all collisions that occured this frame
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            // We get one of the collisions with the player
            KinematicCollision3D collision = GetSlideCollision(i);

            // If the collision is with a mob.
            if (collision.GetCollider() is Mob mob)
            {
                // We check that we are hitting it from above.
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
                {
                    Debug.WriteLine($"Squashed {mob.GetInstanceId()}");
                    // If so, we squash it and bounce;
                    mob.Squash();
                    _velocity.Y = _bounceImpulse;
                    Velocity = _velocity;
                    break;
                }
            }

        }
    }

    private void SetLookDirection()
    {
        if (!_moveDirection.IsEqualApprox(Vector3.Zero))
        {
            _lookDirection = _inputLeftCross;
        }
    }

    private void Die()
    {
        EmitSignal(SignalName.Dead);
        QueueFree();
    }

    private void OnMobDetector_BodyEntered(Node3D Body)
    {
        Die();
    }

}

