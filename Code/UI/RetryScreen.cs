using Godot;

public partial class RetryScreen : Control
{
	public override void _Ready() => Hide();

    private void OnPlayableHero_Hit() => Show();

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_accept") && Visible)
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
