using Raylib_cs;

namespace RaylibFlappyBird
{
    class Program
    {

        static void Main(string[] args)
        {

            // init
            Raylib.SetTargetFPS(60);
            Renderer.Init();
            GameLogic.Reset();
            while (!Raylib.WindowShouldClose())
            {
                // tick
                GameLogic.Tick();
                Renderer.Render();
            }

            Raylib.CloseWindow();

        }



    }
}
