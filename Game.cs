using System;
using System.Collections.Generic;
using System.Threading;
namespace BoxingGame
{
    public class Game
    {
        public int Width { get; }
        public int Height { get; }
        public Boxer Player1 { get; private set; }
        public Boxer Player2 { get; private set; }
        public Renderer Renderer { get; }
        public InputManager InputManager { get; }
        public bool IsRunning { get; private set; } = true;
        public bool IsPaused { get; private set; } = false;
        public bool IsAiMode { get; private set; } = false;
        public Game(int width = 85, int height = 25)
        {
            Width = width;
            Height = height;
            Player1 = new Boxer("P1", 20, height - 8, false);
            Player2 = new Boxer("P2", width - 20, height - 8, true);
            Renderer = new Renderer(width, height, this);
            InputManager = new InputManager();
            RoundTimeMs = 60 * 1000;
        }
        public int RoundTimeMs { get; private set; }
        public void Run()
        {
            Console.CursorVisible = false;
            var sw = new System.Diagnostics.Stopwatch();
            const int targetFps = 30;
            const int frameTime = 1000 / targetFps;
            while (IsRunning)
            {
                sw.Restart();
                InputManager.PollInputs();
                HandleInput();
                    if (!IsPaused)
                    {
                        Update(frameTime);
                        RoundTimeMs = Math.Max(0, RoundTimeMs - frameTime);
                    }
                Renderer.Render();
                        if (Player1.Health <= 0 || Player2.Health <= 0 || RoundTimeMs <= 0)
                    {
                        IsRunning = false;
                        Renderer.RenderGameOver();
                        break;
                    }
                int remaining = frameTime - (int)sw.ElapsedMilliseconds;
                if (remaining > 0) Thread.Sleep(remaining);
            }
            Console.CursorVisible = true;
            Renderer.RenderGameOver();
            Console.WriteLine("Press R to restart or any other key to quit...");
            var k = Console.ReadKey(true);
            if (k.Key == ConsoleKey.R)
            {
                Player1 = new Boxer("P1", 20, Height - 8, false);
                Player2 = new Boxer("P2", Width - 20, Height - 8, true);
                IsRunning = true;
                Run();
            }
            Console.SetCursorPosition(0, Height);
        }
        private void HandleInput()
        {
            // Player 1                             //TODO await Task.Delay(xxxx);
            int leftBound = 4;
            int rightBound = this.Width - 5;
            // Player 1
            if (InputManager.IsKeyDown(ConsoleKey.A)) Player1.MoveLeft(leftBound);
            else if (InputManager.IsKeyDown(ConsoleKey.D)) Player1.MoveRight(rightBound);
            else if (InputManager.IsKeyDown(ConsoleKey.W)) Player1.Punch();
            else if (InputManager.IsKeyDown(ConsoleKey.S)) Player1.Block();

            // Player 2
            if (InputManager.IsKeyDown(ConsoleKey.LeftArrow)) Player2.MoveLeft(leftBound);
            else if (InputManager.IsKeyDown(ConsoleKey.RightArrow)) Player2.MoveRight(rightBound);
            else if (InputManager.IsKeyDown(ConsoleKey.UpArrow)) Player2.Punch();
            else if (InputManager.IsKeyDown(ConsoleKey.DownArrow)) Player2.Block();
            // Toggle AI
            if (InputManager.IsKeyDown(ConsoleKey.M)) IsAiMode = !IsAiMode;
            // Quit
            if (InputManager.IsKeyDown(ConsoleKey.Escape)) IsRunning = false;
                if (InputManager.IsKeyDown(ConsoleKey.P)) IsPaused = !IsPaused;
        }
        private void Update(int deltaMs)
        {
            Player1.Tick(deltaMs);
            Player2.Tick(deltaMs);
            int leftBound = 4;
            int rightBound = this.Width - 5;
            if (IsAiMode)
            {
                RunAiForPlayer(Player2, Player1, leftBound, rightBound);
            }
            CheckPunchHits(Player1, Player2, leftBound, rightBound);
            CheckPunchHits(Player2, Player1, leftBound, rightBound);
            StaminaDrain(Player1, Player2);
            StaminaDrain(Player2, Player1);
        }
        private void StaminaDrain(Boxer attacker, Boxer defender)
        {
            if (attacker.IsPunching)
            {
                attacker.Stamina = Math.Max(0, attacker.Stamina - 2);
            }
            if (defender.IsBlocking)
            {
                defender.Stamina = Math.Max(0, defender.Stamina - 2);
            }
        }
        private void CheckPunchHits(Boxer attacker, Boxer defender, int leftBound, int rightBound)
        {
            if (!attacker.IsPunching || attacker.PunchHit) return;
            int range = 4;
            int distance = Math.Abs(attacker.X - defender.X);
                if (distance <= range && Math.Abs(attacker.Y - defender.Y) <= 2)
            {
                if (defender.IsBlocking)
                {
                    defender.Health -= 5;
                }
                else
                {
                    defender.Health -= 12;
                }
                attacker.PunchHit = true;
                int dir = attacker.X < defender.X ? 1 : -1;
                defender.X = Math.Max(leftBound, Math.Min(rightBound, defender.X + dir * 2));
            }
        }
        private void RunAiForPlayer(Boxer ai, Boxer opponent, int leftBound, int rightBound)
        {
            if (ai.X < opponent.X - 2) ai.MoveRight(rightBound);
            else if (ai.X > opponent.X + 2) ai.MoveLeft(leftBound);
            int distance = Math.Abs(ai.X - opponent.X);
            if (distance <= 4)
            {
                ai.Punch();
                
            }
        }
    }
}