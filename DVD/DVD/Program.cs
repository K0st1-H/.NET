namespace DVD
{
    using Raylib_cs;  // Add this to access Raylib functions
    using System.Numerics;

    namespace DVD // Your project namespace
    {
        class Program
        {
            static void Main()
            {
                // Alustetaan ikkuna
                Raylib.InitWindow(800, 600, "DVD Screensaver");

                // Tekstin alkuperäinen sijainti (keskelle ikkunaa)
                int windowWidth = Raylib.GetScreenWidth();
                int windowHeight = Raylib.GetScreenHeight();
                Font font = Raylib.GetFontDefault();
                Vector2 textSize = Raylib.MeasureTextEx(font, "DVD", 20, 2);
                Vector2 position = new Vector2(windowWidth / 2 - textSize.X / 2, windowHeight / 2 - textSize.Y / 2);

                // Tekstin suunta
                Vector2 direction = new Vector2(1, 1);

                // Tekstin nopeus (nopeutettu)
                float speed = 30.0f;  // Increased speed to 30.0f

                // Pelin aikaruutu
                float frameTime;

                // Loputon pelisilmukka
                while (!Raylib.WindowShouldClose())
                {
                    // Päivitä
                    frameTime = Raylib.GetFrameTime();
                    position += direction * speed * frameTime;

                    // Tarkista reunoista kimpoaminen
                    if (position.X + textSize.X > windowWidth || position.X < 0)
                    {
                        direction = new Vector2(-direction.X, direction.Y);
                    }

                    if (position.Y + textSize.Y > windowHeight || position.Y < 0)
                    {
                        direction = new Vector2(direction.X, -direction.Y);
                    }

                    // Piirrä
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(new Color(0, 0, 0, 255));  // Musta väri
                    Raylib.DrawTextEx(font, "DVD", position, 20, 2, new Color(255, 255, 0, 255)); // Keltainen väri
                    Raylib.EndDrawing();
                }

                // Sulje ikkuna
                Raylib.CloseWindow();
            }
        }
    }

}
