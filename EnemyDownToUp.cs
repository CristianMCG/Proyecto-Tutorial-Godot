using Godot;
using System;
using Utiles;

public partial class EnemyDownToUp : Area2D
{
	Vector2 destiny;
	AnimatedSprite2D animation;
	const int speed = 400;
	public const float colliderLarge = 42, colliderWidth = 46;
	public Vector2 collider;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Utils.pickAnimationEnemy(ref animation);
		int randomX = Utils.randomNumberFrom0to(1153);
		Position = new Vector2(randomX, 720);
		collider = new Vector2(6.5f + randomX, -0.5f + 720);
		randomX = Utils.randomNumberFrom0to(1153);
		destiny = new Vector2(randomX, -70);
	}

	public override void _Process(double delta)
	{
		Position = Utils.Mover(destiny, Position, (float)delta, speed);
		collider = new Vector2(Position.X + 6.5f, Position.Y + -0.5f);
		if(DestinoAlcanzado())
			QueueFree();
	}

	private bool DestinoAlcanzado()
	{
		return (Position.Y <= destiny.Y) ? true : false;
	}

}
