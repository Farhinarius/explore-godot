using Godot;

namespace ExploreGodot.Code.Control;

public partial class CubeController : CharacterBody3D
{
	private const float SPEED = 5.0f;
	private const float JUMP_VELOCITY = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JUMP_VELOCITY;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		Vector2 inputVector = Input.GetVector(
			InputMapping.HorizontalNegative, 
			InputMapping.HorizontalPositive, 
			InputMapping.VerticalNegative,
			InputMapping.VerticalPositive);

		Vector3 movementDirection = (Transform.Basis * new Vector3(inputVector.X, 0, -inputVector.Y)).Normalized();
		if (movementDirection != Vector3.Zero)
		{
			velocity.X = movementDirection.X * SPEED;
			velocity.Z = movementDirection.Z * SPEED;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, SPEED);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, SPEED);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
