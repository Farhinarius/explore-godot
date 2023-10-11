using Godot;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

public partial class RetryScreen : Control
{
	public override void _Ready() => HideScreen();

	private void ShowScreen() => Show();

	private void HideScreen() => Hide();

    private void OnPlayableHero_DeadInUI()
	{
		Show();
	}

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_accept") && Visible)
        {
            GetTree().ReloadCurrentScene();
        }
    }
}
