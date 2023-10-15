using ExporeGodot.Code.Control;
using ExporeGodot.Code.Enemies;
using Godot;

public partial class SpawningEnemies : Node3D
{
    [Export]
    public PackedScene MobScene { get; set; }
    [Export]
    private PlayableHero _playableHero;
    [Export]
    private PathFollow3D _spawnPath;
    [Export]
    private Timer _timer;
    [Export]
    private ScoreLabel _scoreLabel;

    private void OnMobSpawn_TimerTimeout()
    {
        // Create a new instance of the Mob Scene 
        // TODO: Debug instantiation process
        var mob = MobScene.Instantiate<Mob>();

        // Choose a random locatin on the Spawn Path
        // We store the reference to the SpawnLocation node
        var mobSpawnLocation = _spawnPath;
        // And give it random offset
        mobSpawnLocation.ProgressRatio = GD.Randf();

        Vector3 playerPosition = _playableHero.Position;
        // initilize mob relative to player
        mob.Initialize(mobSpawnLocation.Position, playerPosition);

        // Spawn the mob by adding it to the Main scene.
        // TODO: debug AddChild method
        AddChild(mob);

        // set callback after enemy is squashed
        mob.Squashed += _scoreLabel.Increase;
    }

    private void OnPlayableHero_Hit()
    {
        _timer.Stop();
    }
}
