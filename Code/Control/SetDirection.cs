using ExporeGodot.Code.Control;
using Godot;

namespace ExploreGodot.Code.Control;

public partial class SetDirection : Node3D
{
	[Export]
	private PlayableHeroControl _playableHeroControl;

    [Export]
    private float _rotationSpeed = 0.5f;

    private Vector3 _rotation;

	public override void _Process(double delta)
	{
        _rotation.Y = Mathf.LerpAngle(Rotation.Y, _playableHeroControl.LookDirection, (float)delta * _rotationSpeed);
        Rotation = _rotation;
    }
}
