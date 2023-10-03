using ExploreGodot.Code;
using Godot;

namespace ExporeGodot.Code.CharacterControl;

public partial class PlayableHero : CharacterBody3D
{
    [Export]
    private float _speed = 5.0f;
    [Export]
    private float _jumpStrength = 4.5f;
    [Export]
    private float _gravityForce = 75f;
    [Export]
    private SpringArm3D _springArm;

    private Vector3 _snapVector = Vector3.Down;
    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        _moveDirection.X = Input.GetAxis(InputMapping.HorizontalNegative, InputMapping.HorizontalPositive);
        _moveDirection.Z = Input.GetAxis(InputMapping.VerticalPositive, InputMapping.VerticalNegative);       
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

        
        Velocity = _velocity;
        MoveAndSlide();
        ApplyFloorSnap();
    }

    public override void _Process(double delta)
    {
        _springArm.Position = Position;
    }
}

