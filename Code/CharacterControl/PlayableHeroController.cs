using ExploreGodot.Code;
using Godot;

public partial class PlayableHeroController : CharacterBody3D
{
	[Export]
	private float _speed = 5.0f;
	[Export] 
	private float _jumpVelocity = 4.5f;
	[Export]
	private float _gravity = 75f;

	private InputMapping _inputMapping = new InputMapping();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= _gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed(_inputMapping.Accept) && IsOnFloor())
			velocity.Y = _jumpVelocity;

        Vector2 input = Input.GetVector(
            _inputMapping.HorizontalNegative,
            _inputMapping.HorizontalPositive,
            _inputMapping.VerticalNegative,
            _inputMapping.VerticalPositive);

		Vector3 movementDirection = (Transform.Basis * new Vector3(input.X, 0, -input.Y)).Normalized();

        if (movementDirection != Vector3.Zero)
        {
            velocity.X = movementDirection.X * _speed;
			velocity.Z = movementDirection.Z * _speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
