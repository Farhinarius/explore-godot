using InputHandler = ExploreGodot.Code.Input.InputHandler;
using Godot;

namespace ExploreGodot.Code.Control;

public partial class CubeController : CharacterBody3D
{
    [Export]
    private float _speed = 7.0f;
    [Export]
    private float _jumpStrength = 20f;
    [Export]
    private float _gravityForce = 50f;
    [Export]
    private SpringArm3D _springArm;

    private Vector3 _moveDirection = Vector3.Zero;
    private Vector3 _targetVelocity = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        // get move direction
        _moveDirection.X = InputHandler.LeftCross.X;
        _moveDirection.Z = -InputHandler.LeftCross.Y;
        _moveDirection = _moveDirection.Rotated(Vector3.Up, _springArm.Rotation.Y).Normalized();

        // get movement velocity
        _targetVelocity = _moveDirection * _speed;
        
        // get gravity velocity
        _targetVelocity.Y -= _gravityForce * _speed;
        
        // get jump velocity
        if (IsOnFloor() && InputHandler.ConfrimPressed)
        {
            _targetVelocity.Y = _jumpStrength;
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    //public override void _Process(double delta)
    //{
    //    _springArm.Transform = Transform;
    //}
}
