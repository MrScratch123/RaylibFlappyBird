using Raylib_cs;
namespace RaylibFlappyBird
{
    class Renderer
    {
        public static void Init()
        {
            Raylib.InitWindow(1280, 720, "Floppy Bird!");
            Raylib.SetTargetFPS(60);

        }
        public static void Render()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            if (GameLogic.gameState == GameState.menu)
            {
                DrawMenu();
            }
            else if (GameLogic.gameState == GameState.game)
            {
                DrawGame();
            }
            else
            {
                DrawEndScreen();
            }
            Raylib.EndDrawing();

        }

        private static void DrawMenu()
        {
            // draw the main menu
            Raylib.DrawText("Floppy Rocket!", FindCentreXPos("Floppy Rocket!", 100), 60, 100, Color.WHITE);
            Raylib.DrawText("Press [Enter] to continue", FindCentreXPos("Press [Enter] to continue", 40), Raylib.GetScreenHeight() / 2, 40, Color.WHITE);
        }

        private static void DrawGame()
        {
            // draw the bird
            Raylib.DrawEllipse((int)GameLogic.Bird.position.X, (int)GameLogic.Bird.position.Y, 45, 30, Color.YELLOW);
            DrawGameTunnels();
            DrawGameUI();
        }
        private static void DrawEndScreen()
        {
            Raylib.DrawText("You lost!", FindCentreXPos("You lost!", 100), 60, 100, Color.WHITE);
            Raylib.DrawText($"Score: {GameLogic.score}", FindCentreXPos($"Score: {GameLogic.score}", 40), Raylib.GetScreenHeight() / 2, 40, Color.WHITE);
            Raylib.DrawText($"High Score: {GameLogic.highScore}", FindCentreXPos($"High Score: {GameLogic.highScore}", 40), Raylib.GetScreenHeight() / 2 + 50, 40, Color.WHITE);
            Raylib.DrawText("Press [Enter] to try again", FindCentreXPos("Press [Enter] to try again", 40), Raylib.GetScreenHeight() / 2 + 100, 40, Color.WHITE);
        }

        private static void DrawGameUI()
        {
            if (GameLogic.paused)
            {
                Raylib.DrawText("GAME PAUSED", FindCentreXPos("GAME PAUSED", 100), Raylib.GetScreenHeight() / 2 - 50, 100, Color.WHITE);
                Raylib.DrawText($"Score: {GameLogic.score}", FindCentreXPos($"Score: {GameLogic.score}", 50), Raylib.GetScreenHeight() / 2 + 50, 50, Color.WHITE);
            }
            else
            {
                Raylib.DrawText($"Score: {GameLogic.score}", FindCentreXPos($"Score: {GameLogic.score}", 50), 60, 50, Color.WHITE);
            }
        }

        private static void DrawGameTunnels()
        {
            foreach (var _tunnel in GameLogic.tunnels)
            {
                Rectangle _down;
                Rectangle _up;
                if (_tunnel.tunnelType == TunnelType.up)
                {
                    _down = new Rectangle(_tunnel.position.X, _tunnel.position.Y + 025, 100, 400);
                    _up = new Rectangle(_tunnel.position.X, _tunnel.position.Y - 625, 100, 400);
                }
                else if (_tunnel.tunnelType == TunnelType.middle)
                {
                    _down = new Rectangle(_tunnel.position.X, _tunnel.position.Y + 125, 100, 400);
                    _up = new Rectangle(_tunnel.position.X, _tunnel.position.Y - 525, 100, 400);
                }
                else
                {
                    _down = new Rectangle(_tunnel.position.X, _tunnel.position.Y + 225, 100, 400);
                    _up = new Rectangle(_tunnel.position.X, _tunnel.position.Y - 425, 100, 400);
                }
                Raylib.DrawRectangleRec(_down, Color.GREEN);
                Raylib.DrawRectangleRec(_up, Color.GREEN);
            }
        }


        // function to find the center x position to render the text on
        static int FindCentreXPos(string _text, int _size)
        {
            return Raylib.GetScreenWidth() / 2 - Raylib.MeasureText(_text, _size) / 2;
        }
    }
}
