namespace ExporeGodot.Code.Control;

using ExploreGodot.Code.Input;
using ExporeGodot.Code.Enemies;
using Godot;

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
    [Export]
    private AnimationPlayer _animationPlayer;
    [Export]
    private Node3D _pivot;
    [Export]
    private float _rotationSpeed = 5f;
    [Signal]
    public delegate void HitEventHandler();

    private Vector2 _inputLeftCross = Vector2.Zero;
    private Vector3 _moveDirection = Vector3.Zero;
    private Vector2 _lookDirection = Vector2.Down;          // initial setup -> look up
    private Vector3 _targetVelocity = Vector3.Zero;
    private Vector3 _rotation;
    private float _lookAngle;
    private bool _jumpPressed;

    public override void _PhysicsProcess(double delta)
    {
        TranslateInput();
        ApplyMovement();
        ApplyGravity((float)delta);
        ApplyJump();
        SetLooking((float)delta);
        BounceFromEnemy();
        ApplyAnimationSpeed();
        ApplyArcJump();
    }

    private void TranslateInput()
    {
        _inputLeftCross.X = Input.GetAxis(InputMapping.LeftCrossHorizontalNegative,
                                 InputMapping.LeftCrossHorizontalPositive);
        _inputLeftCross.Y = Input.GetAxis(InputMapping.LeftCrossVerticalNegative,
                                 InputMapping.LeftCrossVerticalPositive);

        _moveDirection.X = _inputLeftCross.X;
        _moveDirection.Z = -_inputLeftCross.Y;
        _moveDirection = _moveDirection.Normalized();

        _jumpPressed = Input.IsActionJustPressed(InputMapping.Confirm);
    }

    private void ApplyMovement()
    {
        _targetVelocity.X = _moveDirection.X * _speed;
        _targetVelocity.Z = _moveDirection.Z * _speed;

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private void ApplyGravity(float delta)
    {
        if (!IsOnFloor())
            _targetVelocity.Y -= _gravityForce * delta;
    }

    private void ApplyJump()
    {
        if (IsOnFloor() && _jumpPressed)
        {
            _targetVelocity.Y = _jumpStrength;
        }
        Velocity = _targetVelocity;
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
                    mob.Squash();
                    _targetVelocity.Y = _bounceImpulse;
                    Velocity = _targetVelocity;
                    break;
                }
            }

        }
    }

    private void SetLooking(float delta)
    {
        if (!_moveDirection.IsEqualApprox(Vector3.Zero))
        {
            _lookAngle = _inputLeftCross.Angle() - (Mathf.Pi / 2);
            _rotation.Y = Mathf.LerpAngle(Rotation.Y, _lookAngle, delta * _rotationSpeed);
            Rotation = _rotation;
        }

    }

    private void ApplyAnimationSpeed()
    {
        if (!_moveDirection.IsEqualApprox(Vector3.Zero))
        {
            _animationPlayer.SpeedScale = 3;
        }
        else
        {
            _animationPlayer.SpeedScale = 1;
        }
    }

    private void ApplyArcJump()
    {
        // TODO: check what a hell is that?
        _pivot.Rotation = new Vector3(Mathf.Pi / 6.0f * Velocity.Y / _jumpStrength, _pivot.Rotation.Y, _pivot.Rotation.Z);

        if (IsOnFloor())
        {
            _pivot.Rotation = Vector3.Zero;
        }
    }

    private void Die()
    {
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    private void OnMobDetector_BodyEntered(Node3D Body)
    {
        Die();
    }

}

