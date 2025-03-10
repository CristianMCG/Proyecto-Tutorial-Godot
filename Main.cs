using Godot;
using System;
using System.Diagnostics;
using Utiles;
using System.IO;

public partial class Main : Node2D
{
	AudioStreamPlayer audio, audioGameOver;
	Label points, best;
	Stopwatch cronometro = new Stopwatch();
	PackedScene InstanciadorEnemyUD = ResourceLoader.Load<PackedScene>("res://enemy_up_to_down.tscn");
	PackedScene InstanciadorEnemyDU = ResourceLoader.Load<PackedScene>("res://enemy_down_to_up.tscn");
	PackedScene InstanciadorEnemyRL = ResourceLoader.Load<PackedScene>("res://enemy_right_to_left.tscn");
	PackedScene InstanciadorEnemyLR = ResourceLoader.Load<PackedScene>("res://enemy.tscn");
	PackedScene InstanciadorPlayer = ResourceLoader.Load<PackedScene>("res://player.tscn");

	bool gameOver;

	long time;

	Player player;
	EnemyLeftToRight[] arrayLR = new EnemyLeftToRight[10];
	EnemyRightToLeft[] arrayRL = new EnemyRightToLeft[10];
	EnemyUpToDown[] arrayUD = new EnemyUpToDown[10];
	EnemyDownToUp[] arrayDU = new EnemyDownToUp[10];

	int iteradorLR = 0, iteradorRL = 0, iteradorUD = 0, iteradorDU = 0;

	public override void _Ready()
	{
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		audioGameOver = GetNode<AudioStreamPlayer>("AudioStreamPlayer2");
		player = (Player)InstanciadorPlayer.Instantiate();
		points = GetNode<Label>("Label");
		best = GetNode<Label>("Label2");
		best.AddThemeColorOverride("font_color", new Color(0,0,0));
		best.Text = "Best: " + File.ReadAllText("best.txt");
		points.Position = new Vector2(554, 24);
		points.AddThemeColorOverride("font_color", new Color(0,0,0));
		AddChild(player);
		cronometro.Restart();
		cronometro.Start();
		gameOver = false;
		time = 0;
	}

	
	public override void _Process(double delta)
	{
		if(!audio.Playing && !gameOver)
			audio.Play();
		if(cronometro.ElapsedMilliseconds >= time && !gameOver)  {
			int typeEnemy = Utils.randomNumberFrom0to(4);		
			Utils.SeleccionarEnemigo(typeEnemy, iteradorLR, iteradorRL, iteradorDU, iteradorUD, arrayLR, arrayRL, arrayDU, arrayUD, ref InstanciadorEnemyLR, ref InstanciadorEnemyRL, ref InstanciadorEnemyUD, ref InstanciadorEnemyDU);
			if(typeEnemy == 0) AddChild(arrayLR[iteradorLR]);
			if(typeEnemy == 1) AddChild(arrayRL[iteradorRL]);
			if(typeEnemy == 2) AddChild(arrayDU[iteradorDU]);
			if(typeEnemy == 3) AddChild(arrayUD[iteradorUD]);
			Utils.UpdateIterador(typeEnemy, ref iteradorLR, ref iteradorRL, ref iteradorUD, ref iteradorDU);
			Utils.RestartIterador(ref iteradorLR, ref iteradorRL, ref iteradorUD, ref iteradorDU, 10);
			time += UpdateTime(cronometro.ElapsedMilliseconds / 1000);
		}
		if(CheckCollisions() && !gameOver) {
			player.QueueFree();
			gameOver = true;
			cronometro.Stop();
			audio.Stop(); audioGameOver.Play();
			points.Position = new Vector2(185,260);
			points.Text = "Game Over. You Achieved " + cronometro.ElapsedMilliseconds / 1000 + " Points.\n           Press Enter To Retry";
			int currBest = int.Parse(File.ReadAllText("Best.txt"));
			long curr = cronometro.ElapsedMilliseconds / 1000;
			if(curr > currBest)
				File.WriteAllText("Best.txt", curr.ToString());

		}
		if(!gameOver) points.Text = Utils.NumberToString(cronometro.ElapsedMilliseconds / 1000);
		if(gameOver) ShowGameOver();
	}

	private bool CheckCollisions()
	{
		for(int i = 0; i < 10; i++)  {
			if(arrayDU[i] != null && Utils.VereficarColision(arrayDU[i].collider.X, arrayDU[i].collider.Y, EnemyDownToUp.colliderLarge, EnemyDownToUp.colliderWidth, player.collider.X, player.collider.Y, Player.radium))  
				return true;
			if(arrayUD[i] != null && Utils.VereficarColision(arrayUD[i].collider.X, arrayUD[i].collider.Y, EnemyUpToDown.colliderLarge, EnemyUpToDown.colliderWidth, player.collider.X, player.collider.Y, Player.radium))  
				return true;
			if(arrayLR[i] != null && Utils.VereficarColision(arrayLR[i].collider.X, arrayLR[i].collider.Y, EnemyLeftToRight.colliderLarge, EnemyLeftToRight.colliderWidth, player.collider.X, player.collider.Y, Player.radium))  
				return true;
			if(arrayRL[i] != null && Utils.VereficarColision(arrayRL[i].collider.X, arrayRL[i].collider.Y, EnemyRightToLeft.colliderLarge, EnemyRightToLeft.colliderWidth, player.collider.X, player.collider.Y, Player.radium))  
				return true;
		}
		return false;
	}

	int UpdateTime(long seconds)  
	{
		if(seconds < 15)
			return 4000;
		if(seconds < 30)
			return 3000;
		if(seconds < 45)
			return 2000;
		if(seconds < 60)
			return 1000;
		if(seconds < 90)
			return 700;
		return 500;
	}

	void ShowGameOver()
	{
		if(Input.IsActionPressed("retry"))
			_Ready();
	}

}
