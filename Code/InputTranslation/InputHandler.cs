using Godot;

namespace ExploreGodot.Code.InputTranslation;

public partial class InputHandler : Node
{
    private Vector2 _leftCross;
    public Vector2 LeftCross => _leftCross;
    public bool ConfirmPressed { get; private set; }

    public override void _Process(double delta)
    {
        _leftCross.X = Input.GetAxis(InputMapping.LeftCrossHorizontalNegative, 
            InputMapping.LeftCrossHorizontalPositive);
        _leftCross.Y = Input.GetAxis(InputMapping.LeftCrossVerticalNegative, 
            InputMapping.LeftCrossVerticalPositive);

        ConfirmPressed = Input.IsActionJustPressed(InputMapping.Confirm);
    }
}
