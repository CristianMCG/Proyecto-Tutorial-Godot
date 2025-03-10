using Godot;
using System;
using Utiles;

public partial class EnemyLeftToRight : Area2D
{
	Vector2 destiny;
	AnimatedSprite2D animation;
	const int speed = 400;
	public const float colliderLarge = 38, colliderWidth = 35;
	public Vector2 collider;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Utils.pickAnimationEnemy(ref animation);
		int randomY = Utils.randomNumberFrom0to(649);
		Position = new Vector2(-51, randomY);
		collider = new Vector2(-51 + 5.5f, 2f + randomY);
		randomY = Utils.randomNumberFrom0to(649);
		destiny = new Vector2(1228, randomY);
	}

	
	public override void _Process(double delta)
	{
		Position = Utils.Mover(destiny, Position, (float)delta, speed);
		collider = new Vector2(Position.X + 5.5f, Position.Y + 2f);
		if(DestinoAlcanzado())
			QueueFree();
	}

	private bool DestinoAlcanzado()
	{
		return (Position.X >= destiny.X) ? true : false;
	}

}
