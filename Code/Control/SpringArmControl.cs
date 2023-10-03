using Godot;

public partial class SpringArmControl : SpringArm3D
{
    private Vector3 _rotationDegrees = Vector3.Zero;
    private float _mouseSensivity = 0.05f;

    private (float NegativeX, float PositiveX,
             float NegativeY, float PositiveY) _cameraBoundaries = new(-90f, 30f, 0f, 360f);

	public override void _Ready()
	{
        TopLevel = true;
        Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseInputEvent)
        {
            _rotationDegrees.X -= mouseInputEvent.Position.Y * _mouseSensivity;
            _rotationDegrees.X = Mathf.Clamp(
                RotationDegrees.X,
                _cameraBoundaries.NegativeX,
                _cameraBoundaries.PositiveX);
            _rotationDegrees.Y = Mathf.Wrap(
                RotationDegrees.Y,
                _cameraBoundaries.NegativeY,
                _cameraBoundaries.PositiveY);
        }
    }
}
