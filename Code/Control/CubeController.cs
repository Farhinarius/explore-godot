using Godot;

namespace ExploreGodot.Code.Control;

public partial class CubeController : CharacterBody3D
{
    [Export]
    private float _speed = 5.0f;
    [Export]
    private float _jumpStrength = 4.5f;
    [Export]
    private float _gravityForce = 75f;
    [Export]
    private SpringArm3D _springArm;
    [Export]
    private Node3D _model;

    private Vector3 _snapVector = Vector3.Down;
    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        _moveDirection.X = Input.GetAxis(InputMapping.LeftCrossHorizontalNegative, InputMapping.LeftCrossHorizontalPositive);

        _moveDirection.Z = Input.GetAxis(InputMapping.LeftCrossVerticalPositive,
            InputMapping.LeftCrossVerticalNegative);

        _moveDirection = _moveDirection.Rotated(Vector3.Up, _springArm.Rotation.Y).Normalized();                // related to camera

        _velocity.X = _moveDirection.X * _speed;
        _velocity.Z = _moveDirection.Z * _speed;
        _velocity.Y -= _gravityForce * (float)delta;

        var justLanded = IsOnFloor() && _snapVector == Vector3.Zero;
        var jumping = IsOnFloor() && Input.IsActionJustPressed(InputMapping.Jump);

        if (jumping)
        {
            _velocity.Y = _jumpStrength;
            _snapVector = Vector3.Zero;
        }
        else if (justLanded)
        {
            _snapVector = Vector3.Down;
        }

        //if (_velocity.Length() > 0.2)
        //{
        //    var lookDirection = new Vector2(_velocity.Z, _velocity.X);
        //    _rotation.X = lookDirection.Angle();
        //}


        Velocity = _velocity;
        MoveAndSlide();
        ApplyFloorSnap();
    }

    public override void _Process(double delta)
    {
        _springArm.Position = Position;
    }
}
