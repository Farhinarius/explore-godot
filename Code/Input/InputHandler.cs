using Godot;
using EngineInput = Godot.Input;

namespace Code.Input;

public partial class InputHandler : Node
{
    public Vector2 LeftCross { get; private set; }
    public bool ConfirmPressed { get; private set; }
    public bool RetryPressed { get; private set; }

    public override void _Process(double delta)
    {
        LeftCross = EngineInput.GetVector(InputMapping.LeftCrossHorizontalNegative,
            InputMapping.LeftCrossHorizontalPositive,
            InputMapping.LeftCrossVerticalNegative,
            InputMapping.LeftCrossVerticalPositive);

        ConfirmPressed = EngineInput.IsActionJustPressed(InputMapping.Confirm);
        RetryPressed = EngineInput.IsActionJustPressed(InputMapping.Retry);
    }
}
