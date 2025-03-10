namespace Utiles;

using Godot;
using System;
using System.Linq;

static class Utils 
{
    public static Vector2 MovimientoPlayer()
    {
        int x = 0, y = 0;
        if(Input.IsActionPressed("up"))
            y--;
        if(Input.IsActionPressed("down"))
            y++;
        if(Input.IsActionPressed("left"))
            x--;
        if(Input.IsActionPressed("right"))
            x++;
        Vector2 v = new Vector2(x,y);
        return v;
    }

    public static Vector2 Normalizar(Vector2 v)  
    {
        if(v.X == 0 && v.Y == 0)
            return Vector2.Zero;
        double suma = v.X * v.X + v.Y * v.Y;
        double raiz2 = Math.Sqrt(suma);
        Vector2 ret = new Vector2(v.X / (float)raiz2, v.Y / (float)raiz2);
        return ret;
    }

    public static Vector2 EvitarQueSeVayaDePantalla(Vector2 v, int limXizq, int limXder, int limYup, int limYdown)  
    {
        if(v.X <= limXizq)  {
			v = new Vector2(limXizq + 1, v.Y);
		}
        if(v.X >= limXder)  {
            v = new Vector2(limXder - 1, v.Y);
        }
        if(v.Y <= limYup)   {
            v = new Vector2(v.X, limYup + 1);
        }
        if(v.Y >= limYdown)  {
            v = new Vector2(v.X, limYdown - 1);
        }
        return v;
    }

    public static int randomNumberFrom0to(int x)
    {
        Random random = new Random();
        int ret = random.Next() % x;
        return ret;
    } 

    public static void pickAnimationEnemy(ref AnimatedSprite2D animation)
    {
        int AnimationType = randomNumberFrom0to(3);
		if(AnimationType == 0)
			animation.Play("fly");
		if(AnimationType == 1)
			animation.Play("swim");
		if(AnimationType == 2)
			animation.Play("walk");
    } 

    public static Vector2 Mover(Vector2 destiny, Vector2 Position, float delta, int speed)
    {
        Vector2 variation = destiny - Position;
		variation = Utils.Normalizar(variation);
		Position = variation * speed * delta + Position;
        return Position;
    }  

    public static bool colliderCircle(float centerX, float centerY, float p1, float p2, float radium)
    {
        return (distance(centerX, centerY, p1, p2) <= radium) ? true : false;
    }

    public static bool colliderRectangle(float centerX, float centerY, float p1, float p2, float large, float width)
    {
        return (centerX - width < p1 && p1 < centerX + width && centerY - large < p2 && p2 < centerY + large);
    }

    public static float distance(float x1, float y1, float x2, float y2)
    {
        return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }

    public static void SeleccionarEnemigo(int type, int itLR, int itRL, int itDU, int itUD, EnemyLeftToRight[] ELR, EnemyRightToLeft[] ERL, EnemyDownToUp[] EDU, EnemyUpToDown[] EUD, ref PackedScene LR, ref PackedScene RL, ref PackedScene UD, ref PackedScene DU)
    {
        if(type == 0)  
            ELR[itLR] = (EnemyLeftToRight)LR.Instantiate();
        if(type == 1)
            ERL[itRL] = (EnemyRightToLeft)RL.Instantiate();
        if(type == 2)
            EDU[itDU] = (EnemyDownToUp)DU.Instantiate();
        if(type == 3)
            EUD[itUD] = (EnemyUpToDown)UD.Instantiate();
    }

    public static void RestartIterador(ref int LR, ref int RL, ref int UD, ref int DU, int res)
    {
        if(LR == res) LR = 0;
        if(RL == res) RL = 0;
        if(DU == res) DU = 0;
        if(UD == res) UD = 0;
    }

    public static void UpdateIterador(int type, ref int LR, ref int RL, ref int UD, ref int DU)
    {
        if(type == 0) LR++;
        if(type == 1) RL++;
        if(type == 2) DU++;
        if(type == 3) UD++;
    }

    public static bool VereficarColision(float rx, float ry, float large, float width, float cx, float cy, float radio)
    {
        
        for(int j = (int)ry - (int)large; j < ry + large; j++)  
            for(int i = (int)rx - (int)width; i < rx + width; i++)  
                if(colliderCircle(cx, cy, i, j, radio))
                    return true;
        return false;
    }

    public static string NumberToString(long number)
    {
        if(number == 0)
            return "0";
        string ret = "";
        while(number > 0)  {
            int last = (int)(number % 10);
            char c = (char)(last + '0');
            ret += c;
            number /= 10;
        }
        ret = ReverseString(ret);
        return ret;
    }

    public static string ReverseString(string s)
    {
        string ret = "";
        for(int i = s.Length - 1; i >= 0; i--)  
            ret += s[i];
        return ret;
    }

}