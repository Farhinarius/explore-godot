using InputHandler = ExploreGodot.Code.InputTranslation.InputHandler;
using Godot;

namespace ExploreGodot.Code.Control;

public partial class CubeController : CharacterBody3D
{
    [Export]
    private float _speed = 7.0f;
    [Export]
    private float _jumpStrength = 20f;
    [Export]
    private float _fallAcceleration = 75f;
    [Export]
    private Node3D _cameraPivot;
    [Export]
    private Node3D _model;
    [Export]
    private InputHandler _input;

    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _targetVelocity = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        // get move direction
        _moveDirection.X = _input.LeftCross.X;
        _moveDirection.Z = -_input.LeftCross.Y;
        _moveDirection = _moveDirection.Rotated(Vector3.Up, _cameraPivot.Rotation.Y).Normalized();

        // get movement velocity
        _targetVelocity.X = _moveDirection.X * _speed;
        _targetVelocity.Z = _moveDirection.Z * _speed;

        // get gravity velocity
        if (!IsOnFloor())
        {
            _targetVelocity.Y -= _fallAcceleration * (float)delta;
        }
        
        // get jump velocity
        if (IsOnFloor() && _input.ConfirmPressed)
        {
            _targetVelocity.Y = _jumpStrength;
        }

        if (_targetVelocity.Length() > 0.2f)
        {
            var lookDirection = new Vector2(_targetVelocity.Z, _targetVelocity.X);
            var _modelRotation = _model.Rotation;
            _modelRotation.Y = lookDirection.Angle();
            _model.Rotation = _modelRotation;
        }

        // apply target velocity
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
