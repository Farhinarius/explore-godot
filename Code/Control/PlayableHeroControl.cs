using ExploreGodot.Code;
using Godot;
using static Godot.TextServer;

namespace ExporeGodot.Code.Control;

public partial class PlayableHeroControl : CharacterBody3D
{
    [Export]
    private float _speed = 5.0f;
    [Export]
    private float _jumpStrength = 4.5f;
    [Export]
    private float _gravityForce = 75f;

    private Vector3 _input = Vector3.Zero;
    private Vector3 _snap = Vector3.Down;
    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;
    private Vector2 _lookDirection = Vector2.Zero;
    private bool _jumpPressed;

    public float LookDirection => _lookDirection.Angle();

    public override void _PhysicsProcess(double delta)
    {
        TranslateInput();
        ApplyMovement((float)delta);
        ApplyJump();
        SetLookDirection();

        //if (_velocity.Length() > 0.2)
        //{
        //    var lookDirection = new Vector2(_velocity.Z, _velocity.X);
        //    _rotation.X = lookDirection.Angle();
        //}
    }

    private void TranslateInput()
    {
        _moveDirection.X = Input.GetAxis(InputMapping.HorizontalNegative, InputMapping.HorizontalPositive);
        _moveDirection.Z = Input.GetAxis(InputMapping.VerticalPositive, InputMapping.VerticalNegative);
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
        var justLanded = IsOnFloor() && _snap == Vector3.Zero;
        var jumping = IsOnFloor() && _jumpPressed;

        if (_jumpPressed)
        {
            _velocity.Y = _jumpStrength;
            _snap = Vector3.Zero;
        }
        else if (justLanded)
        {
            _snap = Vector3.Down;
        }
    }

    private void SetLookDirection()
    {
        if (_moveDirection != Vector3.Zero)
        {
            _lookDirection.X = -_moveDirection.Z;
            _lookDirection.Y = -_moveDirection.X;
        }
    }

}

