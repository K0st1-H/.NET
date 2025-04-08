using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

public class Game
{
    private bool gameRunning;
    private Tank player1;
    private Tank player2;
    private List<Bullet> bullets;

    public static void Main()
    {
        Game game = new Game();
        game.Run();
    }

    public void Run()
    {
        Raylib.InitWindow(800, 600, "Tank Game");
        Raylib.SetTargetFPS(60);
        ResetGame();
        GameLoop();
    }

    private void ResetGame()
    {
        player1 = new Tank(100, 250, Colors.BLUE, 1);
        player2 = new Tank(600, 250, Colors.RED, 2);
        bullets = new List<Bullet>();
        gameRunning = true;
    }

    private void GameLoop()
    {
        while (!Raylib.WindowShouldClose())
        {
            if (gameRunning)
            {
                UpdateGame();
                DrawGame();
            }
            else
            {
                DrawGameOver();
                if (Raylib.IsKeyPressed(KeyboardKey.R))
                {
                    ResetGame();
                }
            }
        }

        Raylib.CloseWindow();
    }

    private void UpdateGame()
    {
        player1.HandleInput(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.Space, bullets);
        player2.HandleInput(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.Enter, bullets);

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i].Update();

            if (bullets[i].Active && bullets[i].ShooterId != player1.Id && Raylib.CheckCollisionRecs(bullets[i].Shape, player1.GetRectangle()))
            {
                player1.IsAlive = false;
                bullets[i].Active = false;
            }

            if (bullets[i].Active && bullets[i].ShooterId != player2.Id && Raylib.CheckCollisionRecs(bullets[i].Shape, player2.GetRectangle()))
            {
                player2.IsAlive = false;
                bullets[i].Active = false;
            }

            if (!bullets[i].Active || bullets[i].IsOffScreen())
            {
                bullets.RemoveAt(i);
            }
        }

        if (!player1.IsAlive || !player2.IsAlive)
        {
            gameRunning = false;
        }
    }

    private void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Colors.LIGHTGREEN);

        player1.Draw();
        player2.Draw();

        foreach (var bullet in bullets)
        {
            bullet.Draw();
        }

        Raylib.EndDrawing();
    }

    private void DrawGameOver()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Colors.LIGHTGREEN);
        Raylib.DrawText("Game Over! Press R to Restart", 220, 250, 20, Colors.BLACK);
        Raylib.EndDrawing();
    }
}

// ==============================
// 🎨 Värit
// ==============================
public static class Colors
{
    public static Color BLACK = new Color(0, 0, 0, 255);
    public static Color BLUE = new Color(0, 0, 255, 255);
    public static Color RED = new Color(255, 0, 0, 255);
    public static Color LIGHTGREEN = new Color(144, 238, 144, 255);
}

// ==============================
// 🚀 TANKKI
// ==============================
public class Tank
{
    public Vector2 Position;
    public Vector2 Direction;
    public Color TankColor;
    public bool IsAlive = true;
    public int Id;

    public Tank(float x, float y, Color color, int id)
    {
        Position = new Vector2(x, y);
        TankColor = color;
        Id = id;
        Direction = new Vector2(1, 0); // Oletussuunnan oikealle
    }

    public void HandleInput(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right, KeyboardKey fire, List<Bullet> bullets)
    {
        if (!IsAlive) return;

        if (Raylib.IsKeyDown(up)) { Move(0, -2); Direction = new Vector2(0, -1); }
        if (Raylib.IsKeyDown(down)) { Move(0, 2); Direction = new Vector2(0, 1); }
        if (Raylib.IsKeyDown(left)) { Move(-2, 0); Direction = new Vector2(-1, 0); }
        if (Raylib.IsKeyDown(right)) { Move(2, 0); Direction = new Vector2(1, 0); }

        if (Raylib.IsKeyPressed(fire))
        {
            Vector2 bulletStart = Position + new Vector2(10, 10) + Direction * 10;
            bullets.Add(new Bullet(bulletStart, Direction * 5, Id));
        }
    }

    public void Move(float dx, float dy)
    {
        Position.X += dx;
        Position.Y += dy;
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle(Position.X, Position.Y, 20, 20);
    }

    public void Draw()
    {
        if (!IsAlive) return;

        Raylib.DrawRectangleRec(GetRectangle(), TankColor);

        // Piirrä tykki
        Vector2 gunStart = Position + new Vector2(10, 10);
        Vector2 gunEnd = gunStart + Direction * 15;
        Raylib.DrawLineEx(gunStart, gunEnd, 3, Colors.BLACK);
    }
}

// ==============================
// 🚀 AMMUS
// ==============================
public class Bullet
{
    public Rectangle Shape;
    public Vector2 Speed;
    public bool Active;
    public int ShooterId;

    public Bullet(Vector2 position, Vector2 speed, int shooterId)
    {
        Shape = new Rectangle(position.X, position.Y, 5, 5);
        Speed = speed;
        Active = true;
        ShooterId = shooterId;
    }

    public void Update()
    {
        if (Active)
        {
            Shape.X += Speed.X;
            Shape.Y += Speed.Y;
        }
    }

    public void Draw()
    {
        if (Active)
        {
            Raylib.DrawRectangleRec(Shape, Colors.RED);
        }
    }

    public bool IsOffScreen()
    {
        return Shape.X < 0 || Shape.X > 800 || Shape.Y < 0 || Shape.Y > 600;
    }
}
