using Code.Input;
using Godot;

namespace Code.UI;

public partial class RetryScreen : Godot.Control
{
	public override void _Ready() => Hide();

    private void OnPlayableHero_Hit() => Show();

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(InputMapping.Retry) && Visible)
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
