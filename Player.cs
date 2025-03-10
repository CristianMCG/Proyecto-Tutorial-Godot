using Godot;
using System;
using Utiles;

public partial class Player : Area2D
{
	const int speed = 400;
	public const float radium = 54;
	public Vector2 collider;
	AnimatedSprite2D animation;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collider = new Vector2(Position.X, Position.Y - 14f);
	}

	public override void _Process(double delta)
	{
		
		Vector2 variation = Utils.MovimientoPlayer();
		variation = Utils.Normalizar(variation);
		Position = variation * speed * (float)delta + Position;
		Position = Utils.EvitarQueSeVayaDePantalla(Position, 54, 1100, 63, 575);
		collider = new Vector2(Position.X, Position.Y - 14f);

		if(variation == Vector2.Zero)  {
			animation.Play("walk");
			animation.Stop();
		}
		if(variation.Y != 0 && variation.X == 0)  {
			animation.Play("up");
			if(variation.Y > 0)
				animation.FlipV = true;
			else
				animation.FlipV = false;
		}
		if(variation.X != 0)  {
			animation.Play("walk");
			if(variation.X > 0)
				animation.FlipH = false;
			else
				animation.FlipH = true;
		}
	}

}
