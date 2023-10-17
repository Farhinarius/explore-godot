namespace ExploreGodot.Code.Input;
using Godot;

public static class InputHandler
{
    private static Vector3 _leftCross;
    public static Vector3 LeftCross
    {
        get
        {
            _leftCross.X = Input.GetAxis(InputMapping.LeftCrossHorizontalNegative,
                                     InputMapping.LeftCrossHorizontalPositive);
            _leftCross.Y = Input.GetAxis(InputMapping.LeftCrossVerticalNegative,
                                     InputMapping.LeftCrossVerticalPositive);
            return _leftCross;
        }
    }


    public static bool ConfrimPressed => Input.IsActionJustPressed(InputMapping.Confirm);
}
