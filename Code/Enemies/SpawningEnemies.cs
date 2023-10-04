using ExporeGodot.Code.Control;
using ExporeGodot.Code.Enemies;
using Godot;
using System;

public partial class SpawningEnemies : Node3D
{
    [Export]
    public PackedScene MobScene { get; set; }

    public void OnMobSpawn_TimerTimeout()
    {
        var mob = MobScene.Instantiate<Mob>();

        var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");

        mobSpawnLocation.ProgressRatio = GD.Randf();

        Vector3 playerPosition = GetNode<PlayableHeroControl>("PlayableHero").Position;
        mob.Initialize(mobSpawnLocation.Position, playerPosition);

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }
}
