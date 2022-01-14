using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

enum GameState
{
    menu,
    game,
    death
}
enum TunnelType
{
    up,
    middle,
    down
}
namespace RaylibFlappyBird
{
    class Bird
    {
        public Vector2 position;
        public float yVel = 0;
    }

    class Tunnel
    {
        public Vector2 position;
        public TunnelType tunnelType;
        public bool scoreAdded = false;
        public Tunnel(TunnelType _tunnelType, Vector2 _position)
        {
            position = _position;
            tunnelType = _tunnelType;
        }
    }

    class GameLogic
    {
        public static GameState gameState = GameState.menu;
        public static Bird Bird;

        public static float gravity = 30f;
        public static float movementMultiplier = 1f;

        public static int score;
        public static int highScore;
        public static List<Tunnel> tunnels;
        public static Random random;

        public static bool paused = false;

        public static void Reset()
        {
            tunnels = new List<Tunnel>();
            random = new Random();
            SpawnBird();
            SpawnTunnels();
            score = 0;
        }

        private static void SpawnTunnels()
        {
            for (int i = 2; i < 7; i++)
            {
                SpawnTunnel(i * 300);
            }
        }

        private static void SpawnTunnel(int xPos)
        {
            var type = random.Next(1, 4);
            switch (type)
            {
                case 1:
                    tunnels.Add(new Tunnel(TunnelType.up, new Vector2(xPos, Raylib.GetScreenHeight() / 2)));
                    break;
                case 2:
                    tunnels.Add(new Tunnel(TunnelType.middle, new Vector2(xPos, Raylib.GetScreenHeight() / 2)));
                    break;
                case 3:
                    tunnels.Add(new Tunnel(TunnelType.down, new Vector2(xPos, Raylib.GetScreenHeight() / 2)));
                    break;
            }
        }

        private static void SpawnBird()
        {
            Bird = new Bird();
            // spawn the bird in the mid vertically and on x = 200 
            Bird.position = new Vector2(200, Raylib.GetScreenHeight() / 2);
        }

        public static void Tick()
        {
            if (gameState == GameState.menu)
            {
                // wait for player to press the enter key
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    gameState = GameState.game;
                }
            }
            else if (gameState == GameState.game)
            {
                // gravity on the bird
                Bird.yVel += gravity * 1 / 60 * movementMultiplier;
                Bird.position.Y += Bird.yVel * movementMultiplier;
                // if space key pressed, make the bird *jump*
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    Bird.yVel = -10f;
                }
                // die if player goes out of bounds
                if (Bird.position.Y < 50 || Bird.position.Y > 650)
                {
                    gameState = GameState.death;
                }
                // loop through the tunnels
                for (int i = 0; i < tunnels.Count; i++)
                {
                    Tunnel _tunnel = tunnels[i];
                    // move the tunnels
                    _tunnel.position.X -= 200 * 1 / 60 * movementMultiplier;
                    if (_tunnel.position.X <= -100)
                    {
                        tunnels.Remove(_tunnel);
                        SpawnTunnel(1500);
                    }
                    if (_tunnel.position.X < 400)
                    {
                        if (CheckForCollision(_tunnel))
                        {
                            GameOver();
                        }
                    }
                    if (!_tunnel.scoreAdded && _tunnel.position.X <= 200)
                    {
                        _tunnel.scoreAdded = true;
                        score += 1;
                    }

                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                {
                    TogglePause();
                }
            }
            else
            {
                // wait for player to press the enter key
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    gameState = GameState.game;
                    SpawnBird();
                    Reset();

                }
            }
        }

        private static void TogglePause()
        {
            paused = !paused;
            movementMultiplier = paused ? 0 : 1;
        }

        private static void GameOver()
        {
            gameState = GameState.death;
            if (highScore < score)
            {
                highScore = score;
            }
        }

        private static bool CheckForCollision(Tunnel _tunnel)
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
            return Raylib.CheckCollisionCircleRec(Bird.position, 45, _down) || Raylib.CheckCollisionCircleRec(Bird.position, 45, _up);
        }
    }
}
