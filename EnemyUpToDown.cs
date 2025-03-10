using Godot;
using System;
using Utiles;

public partial class EnemyUpToDown : Area2D
{
	Vector2 destiny;
	AnimatedSprite2D animation;
	const int speed = 400;
	public const float colliderLarge = 40, colliderWidth = 47;
	public Vector2 collider;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Utils.pickAnimationEnemy(ref animation);
		int randomX = Utils.randomNumberFrom0to(1153);
		Position = new Vector2(randomX, -65);
		collider = new Vector2(7.5f + randomX, -1f - 65);
		randomX = Utils.randomNumberFrom0to(1153);
		destiny = new Vector2(randomX, 720);
	}

	
	public override void _Process(double delta)
	{
		Position = Utils.Mover(destiny, Position, (float)delta, speed);
		collider = new Vector2(Position.X + 7.5f, Position.Y + -1f);
		if(DestinoAlcanzado())
			QueueFree();
	}

	private bool DestinoAlcanzado()
	{
		return (Position.Y >= destiny.Y) ? true : false;
	}

}
