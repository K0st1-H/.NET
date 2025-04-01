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
        Init();
        GameLoop();
    }

    private void Init()
    {
        Raylib.InitWindow(800, 600, "Tank Game");
        Raylib.SetTargetFPS(60);

        ResetGame();
    }

    private void ResetGame()
    {
        player1 = new Tank(100, 250, new Color(0, 0, 255, 255), 1);  // Sininen tankki
        player2 = new Tank(600, 250, new Color(255, 0, 0, 255), 2);  // Punainen tankki
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
        if (player1.IsAlive)
        {
            if (Raylib.IsKeyDown(KeyboardKey.W)) player1.Move(0, -2);
            if (Raylib.IsKeyDown(KeyboardKey.S)) player1.Move(0, 2);
            if (Raylib.IsKeyDown(KeyboardKey.A)) player1.Move(-2, 0);
            if (Raylib.IsKeyDown(KeyboardKey.D)) player1.Move(2, 0);
        }

        if (player2.IsAlive)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Up)) player2.Move(0, -2);
            if (Raylib.IsKeyDown(KeyboardKey.Down)) player2.Move(0, 2);
            if (Raylib.IsKeyDown(KeyboardKey.Left)) player2.Move(-2, 0);
            if (Raylib.IsKeyDown(KeyboardKey.Right)) player2.Move(2, 0);
        }

        if (player1.IsAlive && Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            bullets.Add(new Bullet(player1.Position, new Vector2(5, 0), player1.Id));
        }

        if (player2.IsAlive && Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            bullets.Add(new Bullet(player2.Position, new Vector2(-5, 0), player2.Id));
        }

        // Päivitetään ammukset ja tarkistetaan osumat
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

            // Poistetaan epäaktiiviset ammukset
            if (!bullets[i].Active || bullets[i].IsOffScreen())
            {
                bullets.RemoveAt(i);
            }
        }

        // Tarkistetaan, onko peli päättynyt
        if (!player1.IsAlive || !player2.IsAlive)
        {
            gameRunning = false;
        }
    }

    private void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(new Color(144, 238, 144, 255));  // Vihreä tausta

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
        Raylib.ClearBackground(new Color(144, 238, 144, 255));  // Vihreä tausta

        Color black = new Color(0, 0, 0, 255);
        Raylib.DrawText("Game Over!", 300, 250, 40, black);
        Raylib.DrawText("Press R to Restart", 280, 300, 30, black);

        Raylib.EndDrawing();
    }
}

// ==============================
// 🚀 TANKKI-LUOKKA
// ==============================
public class Tank
{
    public Vector2 Position;
    public Color TankColor;
    public bool IsAlive = true;
    public int Id;

    public Tank(float x, float y, Color color, int id)
    {
        Position = new Vector2(x, y);
        TankColor = color;
        Id = id;
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
        if (IsAlive)
        {
            Raylib.DrawRectangleRec(GetRectangle(), TankColor);
        }
    }
}

// ==============================
// 🚀 AMMUS-LUOKKA
// ==============================
public class Bullet
{
    public Rectangle Shape;
    public Vector2 Speed;
    public bool Active;
    public int ShooterId;

    public Bullet(Vector2 position, Vector2 speed, int shooterId)
    {
        Shape = new Rectangle(position.X + 10, position.Y + 10, 5, 5);
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
            Raylib.DrawRectangleRec(Shape, new Color(255, 0, 0, 255));  // Punainen ammus
        }
    }

    public bool IsOffScreen()
    {
        return Shape.X < 0 || Shape.X > 800 || Shape.Y < 0 || Shape.Y > 600;
    }
}

