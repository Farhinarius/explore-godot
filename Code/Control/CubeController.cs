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
}
