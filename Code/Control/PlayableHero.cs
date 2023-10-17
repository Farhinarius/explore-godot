using ExploreGodot.Code.InputTranslation;
using ExporeGodot.Code.Enemies;
using Godot;

namespace ExporeGodot.Code.Control;

public partial class PlayableHero : CharacterBody3D
{
    [Export]
    private float _speed = 7.5f;
    [Export]
    private float _jumpStrength = 16f;
    [Export]
    private float _fallAcceleration = 75f;
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
    [Export]
    private InputHandler _input;

    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _targetVelocity = Vector3.Zero;
    private Vector3 _rotation;
    private float _lookAngle;

    public override void _PhysicsProcess(double delta)
    {
        GetMovementDirection();
        GetMovementVelocity();
        GetGravityVelocity((float)delta);
        GetJumpVelocity();
        RotateTowardsMoveDirection((float)delta);
        BounceFromEnemy();
        SetAnimationSpeed();

        ApplyTargetVelocity();
        ApplyArcJump();
    }

    private void GetMovementDirection()
    {
        _moveDirection.X = _input.LeftCross.X;
        _moveDirection.Z = -_input.LeftCross.Y;
        _moveDirection = _moveDirection.Normalized();
    }

    private void GetMovementVelocity()
    {
        _targetVelocity.X = _moveDirection.X * _speed;
        _targetVelocity.Z = _moveDirection.Z * _speed;
    }

    private void GetGravityVelocity(float delta)
    {
        if (!IsOnFloor())
            _targetVelocity.Y -= _fallAcceleration * delta;
    }

    private void GetJumpVelocity()
    {
        if (IsOnFloor() && _input.ConfirmPressed)
        {
            _targetVelocity.Y = _jumpStrength;
        }
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
                    break;
                }
            }

        }
    }

    private void RotateTowardsMoveDirection(float delta)
    {
        if (!_moveDirection.IsEqualApprox(Vector3.Zero))
        {
            _lookAngle = _input.LeftCross.Angle() - (Mathf.Pi / 2);
            _rotation.Y = Mathf.LerpAngle(Rotation.Y, _lookAngle, delta * _rotationSpeed);
            Rotation = _rotation;
        }
    }

    private void SetAnimationSpeed()
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
    private void ApplyTargetVelocity()
    {
        Velocity = _targetVelocity;
        MoveAndSlide();
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

