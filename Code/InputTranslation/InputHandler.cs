using Godot;

namespace ExploreGodot.Code.InputTranslation;

public partial class InputHandler : Node
{
    public Vector2 LeftCross { get; private set; }
    public bool ConfirmPressed { get; private set; }

    public override void _Process(double delta)
    {
        LeftCross = Input.GetVector(InputMapping.LeftCrossHorizontalNegative,
            InputMapping.LeftCrossHorizontalPositive,
            InputMapping.LeftCrossVerticalNegative,
            InputMapping.LeftCrossVerticalPositive);

        ConfirmPressed = Input.IsActionJustPressed(InputMapping.Confirm);
    }
}
