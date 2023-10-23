using Godot;

namespace Code.Enemies;

public partial class Mob : CharacterBody3D
{
    [Signal]
    public delegate void SquashedEventHandler();
    [Export]
    private float _minSpeed { get; set; } = 10;
    [Export]
    private float _maxSpeed { get; set; } = 14;
    [Export]
    private AnimationPlayer _animationPlayer;

    public void Squash()
    {
        EmitSignal(SignalName.Squashed);
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
    
    // This function will be called from the Main scene
    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        // We position the mob by placing it at startPosition
        // and rotate it towards playerPosition, so it looks at the Player
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
        // Rotate this mob randomly within range of - 90 and + 90 degrees,
        // so that it doesn't move directly towards the player
        RotateY((float)GD.RandRange(-Mathf.Pi / 4f, Mathf.Pi / 4f));

        // We calculate a random speed (integer).
        var randomSpeed = (float) GD.RandRange(_minSpeed, _maxSpeed);
        // We calculate a forward velocity that respresents the speed
        Velocity = Vector3.Forward * randomSpeed;
        // We then rotate the velocity vector based on the mob's Y rotation
        // in order to move in the direction the mob is looking
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
        // Set up animation player playback speed
        _animationPlayer.SpeedScale = randomSpeed / _minSpeed;
    }

    // We also specified this fucntion name in PascalCase ine the editor's connection window
    public void OnVisibleOnScreenNotifier3d_ScreenExited()
    {
        QueueFree();
    }
}
