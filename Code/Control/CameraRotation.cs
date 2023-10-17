using Godot;
using System;
using System.Diagnostics;

namespace ExploreGodot.Code.Control;

public partial class CameraRotation : Node3D
{
    [Export]
    private float _mouseSensivity = 0.01f;

    private Vector3 _rotationDegrees = Vector3.Zero;
    private Vector3 _cameraTargetRotation;
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
            var horizontalCameraOffsetRotation = mouseMotionEvent.Relative.X * _mouseSensivity;
            var verticalCameraOffsetRotation = mouseMotionEvent.Relative.Y * _mouseSensivity;

            _cameraTargetRotation = Rotation;
            
            _cameraTargetRotation.X += verticalCameraOffsetRotation;
            _cameraTargetRotation.Y += verticalCameraOffsetRotation;

            Rotation = _cameraTargetRotation;
        }
    }
}


