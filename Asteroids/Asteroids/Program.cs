using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;

class Program
{
    static int screenWidth = 800;
    static int screenHeight = 600;

    static Color BACKGROUND = new Color(0, 0, 0, 255);         // Musta
    static Color SHIP_COLOR = new Color(255, 255, 255, 255);   // Valkoinen
    static Color BULLET_COLOR = new Color(255, 255, 0, 255);   // Keltainen

    static void Main()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Asteroids - Raylib");
        Raylib.SetTargetFPS(60);

        Player player = new Player();
        List<Bullet> bullets = new();

        while (!Raylib.WindowShouldClose())
        {
            // --- UPDATE ---
            player.Update();

            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                bullets.Add(new Bullet(player.Position, player.Rotation));

            foreach (var bullet in bullets)
                bullet.Update();

            bullets.RemoveAll(b => !b.IsOnScreen());

            // --- DRAW ---
            Raylib.BeginDrawing();
            Raylib.ClearBackground(BACKGROUND);

            player.Draw(SHIP_COLOR);

            foreach (var bullet in bullets)
                bullet.Draw(BULLET_COLOR);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

class Player
{
    public Vector2 Position;
    public float Rotation;
    float speed = 2.5f;
    Vector2 velocity;

    public Player()
    {
        Position = new Vector2(400, 300);
        Rotation = 0;
    }

    public void Update()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Left)) Rotation -= 3f;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) Rotation += 3f;

        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
            float rad = MathF.PI / 180 * Rotation;
            velocity += new Vector2(MathF.Cos(rad), MathF.Sin(rad)) * 0.1f;
        }

        // Rajoita nopeus
        if (velocity.Length() > speed)
            velocity = Vector2.Normalize(velocity) * speed;

        Position += velocity;
        WrapAround();
    }

    void WrapAround()
    {
        if (Position.X < 0) Position.X += 800;
        if (Position.X > 800) Position.X -= 800;
        if (Position.Y < 0) Position.Y += 600;
        if (Position.Y > 600) Position.Y -= 600;
    }

    public void Draw(Color color)
    {
        Vector2 dir = new Vector2(MathF.Cos(Rotation * MathF.PI / 180), MathF.Sin(Rotation * MathF.PI / 180));
        Vector2 p1 = Position + dir * 20;
        Vector2 p2 = Position + new Vector2(-dir.Y, dir.X) * 10 - dir * 10;
        Vector2 p3 = Position + new Vector2(dir.Y, -dir.X) * 10 - dir * 10;

        Raylib.DrawTriangle(p1, p2, p3, color);
    }
}

class Bullet
{
    public Vector2 Position;
    Vector2 velocity;

    public Bullet(Vector2 startPosition, float angle)
    {
        Position = startPosition;
        float rad = MathF.PI / 180 * angle;
        velocity = new Vector2(MathF.Cos(rad), MathF.Sin(rad)) * 5f;
    }

    public void Update()
    {
        Position += velocity;
        WrapAround();
    }

    public void Draw(Color color)
    {
        Raylib.DrawCircleV(Position, 3, color);
    }

    public bool IsOnScreen()
    {
        return Position.X >= 0 && Position.X <= 800 && Position.Y >= 0 && Position.Y <= 600;
    }

    void WrapAround()
    {
        if (Position.X < 0) Position.X += 800;
        if (Position.X > 800) Position.X -= 800;
        if (Position.Y < 0) Position.Y += 600;
        if (Position.Y > 600) Position.Y -= 600;
    }
}
