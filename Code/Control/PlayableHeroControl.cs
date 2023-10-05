using ExploreGodot.Code;
using Godot;
using System.Diagnostics;

namespace ExporeGodot.Code.Control;

public partial class PlayableHeroControl : CharacterBody3D
{
	[Export]
	private float _speed = 5.0f;
	[Export]
	private float _jumpStrength = 4.5f;
	[Export]
	private float _gravityForce = 75f;

	private Vector2 _inputLeftCross = Vector2.Zero;
	private Vector3 _moveDirection = Vector3.Zero;
	private Vector2 _lookDirection = Vector2.Down;          // initial setup -> look up
	private Vector3 _snap = Vector3.Down;
	private Vector3 _velocity = Vector3.Zero;
	private bool _jumpPressed;

	public float LookDirectionAngle => _lookDirection.Angle() - (Mathf.Pi / 2);

	public override void _PhysicsProcess(double delta)
	{
		TranslateInput();
		ApplyMovement((float)delta);
		ApplyJump();
		SetLookDirection();
	}

	private void TranslateInput()
	{
		_inputLeftCross.X = Input.GetAxis(InputMapping.LeftCrossHorizontalNegative, 
								 InputMapping.LeftCrossHorizontalPositive);
		_inputLeftCross.Y = Input.GetAxis(InputMapping.LeftCrossVerticalNegative,
								 InputMapping.LeftCrossVerticalPositive);
		
		_moveDirection.X = _inputLeftCross.X;
		_moveDirection.Z = -_inputLeftCross.Y;
		_moveDirection = _moveDirection.Normalized();

		_jumpPressed = Input.IsActionJustPressed(InputMapping.Jump);
	}

	private void ApplyMovement(float delta)
	{
		_velocity.X = _moveDirection.X * _speed;
		_velocity.Z = _moveDirection.Z * _speed;
		if (!IsOnFloor())
			_velocity.Y -= _gravityForce * delta;

		Velocity = _velocity;
		MoveAndSlide();
	}

	private void ApplyJump()
	{
		var justLanded = IsOnFloor() && _snap == Vector3.Zero;
		var jumping = IsOnFloor() && _jumpPressed;

		if (jumping)
		{
			_velocity.Y = _jumpStrength;
			_snap = Vector3.Zero;
		}
		else if (justLanded)
		{
			_snap = Vector3.Down;
		}
	}

	private void SetLookDirection()
	{
		if (!_moveDirection.IsEqualApprox(Vector3.Zero))
        {
			_lookDirection.X = _inputLeftCross.X;
			_lookDirection.Y = _inputLeftCross.Y;
		}
	}

}

