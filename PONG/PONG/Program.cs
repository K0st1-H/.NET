using System;
using System.Numerics;
using Raylib_cs;

struct Player
{
    public Vector2 position;
    public int score;
}

struct Ball
{
    public Vector2 position;
    public Vector2 direction;
}

class PongGame
{
    static void Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "Pong Game");

        const int paddleWidth = 10;
        const int paddleHeight = 100;
        const float paddleSpeed = 6.0f;
        const float ballSpeed = 5.0f;
        const int ballSize = 10;

        Player player1 = new Player { position = new Vector2(50, screenHeight / 2 - paddleHeight / 2), score = 0 };
        Player player2 = new Player { position = new Vector2(screenWidth - 50 - paddleWidth, screenHeight / 2 - paddleHeight / 2), score = 0 };

        Ball ball = new Ball { position = new Vector2(screenWidth / 2, screenHeight / 2), direction = new Vector2(-1, -1) };

        Color black = new Color(0, 0, 0, 255);
        Color white = new Color(255, 255, 255, 255);

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsKeyDown(KeyboardKey.W) && player1.position.Y > 0)
                player1.position.Y -= paddleSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.S) && player1.position.Y < screenHeight - paddleHeight)
                player1.position.Y += paddleSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.Up) && player2.position.Y > 0)
                player2.position.Y -= paddleSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.Down) && player2.position.Y < screenHeight - paddleHeight)
                player2.position.Y += paddleSpeed;

            ball.position += ball.direction * ballSpeed;

            if (ball.position.Y <= 0 || ball.position.Y >= screenHeight - ballSize)
                ball.direction.Y *= -1;

            Rectangle paddle1Rect = new Rectangle(player1.position.X, player1.position.Y, paddleWidth, paddleHeight);
            Rectangle paddle2Rect = new Rectangle(player2.position.X, player2.position.Y, paddleWidth, paddleHeight);
            Rectangle ballRect = new Rectangle(ball.position.X, ball.position.Y, ballSize, ballSize);

            if (Raylib.CheckCollisionRecs(paddle1Rect, ballRect))
                ball.direction.X = 1;
            if (Raylib.CheckCollisionRecs(paddle2Rect, ballRect))
                ball.direction.X = -1;

            if (ball.position.X <= 0)
            {
                player2.score++;
                ball.position = new Vector2(screenWidth / 2, screenHeight / 2);
                ball.direction = new Vector2(-1, -1);
            }
            if (ball.position.X >= screenWidth)
            {
                player1.score++;
                ball.position = new Vector2(screenWidth / 2, screenHeight / 2);
                ball.direction = new Vector2(1, 1);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(black);
            Raylib.DrawRectangle((int)player1.position.X, (int)player1.position.Y, paddleWidth, paddleHeight, white);
            Raylib.DrawRectangle((int)player2.position.X, (int)player2.position.Y, paddleWidth, paddleHeight, white);
            Raylib.DrawRectangle((int)ball.position.X, (int)ball.position.Y, ballSize, ballSize, white);
            Raylib.DrawText(player1.score.ToString(), screenWidth / 4, 20, 40, white);
            Raylib.DrawText(player2.score.ToString(), 3 * screenWidth / 4, 20, 40, white);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
