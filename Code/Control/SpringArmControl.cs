using Godot;
using System;
using System.Diagnostics;

namespace ExploreGodot.Code.Control;

public partial class SpringArmControl : SpringArm3D
{
    [Export]
    private float _mouseSensivity = 0.05f;

    private Vector3 _rotationDegrees = Vector3.Zero;
    private Vector3 _position = Vector3.Zero;
    private (float NegativeX, float PositiveX,
             float NegativeY, float PositiveY) _cameraBoundaries = new(-90f, 30f, 0f, 360f);

    public override void _Ready()
    {
        TopLevel = true;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent inputEvent)
    {
        RotateCameraByMouseInput(inputEvent);
    }

    public void RotateCameraByMouseInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseMotion mouseMotionEvent)
        {
            var horizontalCameraOffsetByMouse = mouseMotionEvent.Relative.X * _mouseSensivity;
            var verticalCameraOffsetByMouse = mouseMotionEvent.Relative.Y * _mouseSensivity;

            _position.X = Mathf.Clamp(
                horizontalCameraOffsetByMouse,
                _cameraBoundaries.NegativeX,
                _cameraBoundaries.PositiveX);
            _position.X = Mathf.Clamp(
                verticalCameraOffsetByMouse,
                _cameraBoundaries.NegativeY,
                _cameraBoundaries.PositiveY);

            Position = _position;
        }
    }
}


