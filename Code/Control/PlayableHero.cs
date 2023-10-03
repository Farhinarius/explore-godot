using ExploreGodot.Code;
using Godot;

namespace ExporeGodot.Code.Control;

public partial class PlayableHero : CharacterBody3D
{
    [Export]
    private float _speed = 5.0f;
    [Export]
    private float _jumpStrength = 4.5f;
    [Export]
    private float _gravityForce = 75f;
    [Export]
    private Node3D _model;

    private Vector3 _snapVector = Vector3.Down;
    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        _moveDirection = Vector3.Zero;
        _moveDirection.X = Input.GetAxis(InputMapping.HorizontalNegative, InputMapping.HorizontalPositive);
        _moveDirection.Z = Input.GetAxis(InputMapping.VerticalPositive, InputMapping.VerticalNegative);       
        _moveDirection = _moveDirection.Normalized();                // related to camera

        _velocity.X = _moveDirection.X * _speed;
        _velocity.Z = _moveDirection.Z * _speed;
        //_velocity.Y -= _gravityForce * (float)delta;

        var justLanded = IsOnFloor() && _snapVector == Vector3.Zero;
        var jumping = IsOnFloor() && Input.IsActionJustPressed(InputMapping.Jump);

        //if (jumping)
        //{
        //    _velocity.Y = _jumpStrength;
        //    _snapVector = Vector3.Zero;
        //}
        //else if (justLanded)
        //{
        //    _snapVector = Vector3.Down;
        //}

        //if (_velocity.Length() > 0.2)
        //{
        //    var lookDirection = new Vector2(_velocity.Z, _velocity.X);
        //    _rotation.X = lookDirection.Angle();
        //}

        
        Velocity = _velocity;
        MoveAndSlide();
    }
}

