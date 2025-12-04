using System;
using System.Text;
namespace BoxingGame
{
    public class Renderer
    {
        private int width;
        private int height;
        private char[,] buffer;
        private Game game;
        public Renderer(int width, int height, Game game)
        {
            this.width = width;
            this.height = height;
            buffer = new char[height, width];
            this.game = game;
            try
            {
                Console.SetWindowSize(Math.Max(100, width), Math.Max(25, height + 1));
            }
            catch { }
        }
        public void Render()
        {
            ClearBuffer();
            DrawRing();
            DrawBoxer(game.Player1);
            DrawBoxer(game.Player2);
            DrawUI();
            Flush();
        }
        private void ClearBuffer()
        {
<<<<<<< HEAD
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    buffer[row, col] = ' ';
                }
            }
=======
            for (int r = 0; r < height; r++) 
            for (int c = 0; c < width; c++) buffer[r, c] = ' ';
>>>>>>> 59ab4a7 (added crit/miss, refactored stamina and health, renaming)
        }
        private void DrawRing()
        {
            int left = 2;
            int right = width - 3;
            int top = 3;
            int bottom = height-4;

            for (int x = left; x <= right; x++)
            {
                buffer[top, x] = '=';
                buffer[bottom, x] = '=';
            }
            for (int y = top; y <= bottom; y++) 
            {
                buffer[y, left] = '|';
                buffer[y, right] = '|';
            }
            buffer[top, left] = '+'; buffer[top, right] = '+'; 
            buffer[bottom, left] = '+'; buffer[bottom, right] = '+'; 
        }
        private void DrawBoxer(Boxer b)
        {
            int baseY = b.Y;
            int baseX = b.X;
            if (b.IsFacingLeft)
            {
                DrawString(baseY - 2, baseX - 1, " 0 ");
                DrawString(baseY - 1, baseX - 2, "<|\\");
                DrawString(baseY, baseX - 2, " / \\");
            }
            else
            {
                DrawString(baseY - 2, baseX - 1, " 0 ");
                DrawString(baseY - 1, baseX - 1, "/|>");
                DrawString(baseY, baseX - 1, " / \\");
            }
            if (b.IsPunching)
            {
                if (b.IsFacingLeft)
                {
                    DrawString(baseY - 1, baseX - 5, "<--");
                }
                else
                {
                    DrawString(baseY - 1, baseX + 3, "-->");
                }
            }
            if (b.healthChecker <= 0)
            {
                DrawChar(baseY - 2, baseX, 'X');
            }
        }
        private void DrawUI()
        {
            // Player names
            DrawString(0, 20, $"{game.Player1.Name}");
            DrawString(0, width - 20, $"{game.Player2.Name}");

            // Health bars (row 1)
            DrawBar(1, 0, "Health", game.Player1.healthChecker);
            DrawBar(1, width - 40, "Health", game.Player2.healthChecker);

            // Stamina bars (row 2)
            DrawBar(2, 0, "Stamina", game.Player1.staminaChecker);
            DrawBar(2, width - 40, "Stamina", game.Player2.staminaChecker);

            // Timer at row 3
            var times = TimeSpan.FromMilliseconds(game.RoundTimeMs);
            var timerStr = $"Time: {times.Minutes:D2}:{times.Seconds:D2}";
            DrawString(3, (width - timerStr.Length) / 2, timerStr);

            // AI status also at row 3
            if (game.IsAiMode)
            {
                DrawString(4, (width - 8) / 2 + 1, "AI: ON");
            }
            else
            {
                DrawString(4, (width - 8) / 2 + 1, "AI: OFF");
            }

            DrawString(height - 1, 1, "P1: A/D move, W punch, S block -- P2: Left/Right move, Up punch, Down Block -- esc");
        }
        private void DrawBar(int y, int x, string label, int value)
        {
            int w = 26;
            int fill = Math.Max(0, Math.Min(w, (int)Math.Ceiling(w * value / 100.0)));
            string bar = label + ": " + new string('#', fill) + new string(' ', w - fill);
            DrawString(y, x, bar + $" {value,3}");
        }
        private void DrawString(int row, int col, string s)
        {
            if (row < 0 || row >= height) return;
            for (int i = 0; i < s.Length; i++)
            {
                int c = col + i;
                if (c < 0 || c >= width) continue;
                buffer[row, c] = s[i];
            }
        }
        private void DrawChar(int row, int col, char ch)
        {
            if (row < 0 || row >= height || col < 0 || col >= width) return;
            buffer[row, col] = ch;
        }
        private void Flush()
        {
            var sb = new StringBuilder();
            sb.Append("\u001b"); sb.Append("[0;0H]");
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++) sb.Append(buffer[r, c]);
                sb.Append("\n");
            }
            Console.Write(sb.ToString());
        }
        public void RenderGameOver()
        {
            bool p1Dead = game.Player1.healthChecker <= 0;
            bool p2Dead = game.Player2.healthChecker <= 0;
            bool timeUp = game.RoundTimeMs <= 0;
            string msg = (p1Dead, p2Dead, timeUp) 
            switch
            {
                (true, true, _) => "Draw! Both players knocked out!",
                (true, false, _) => "Player 2 wins!",
                (false, true, _) => "Player 1 wins!",
                (false, false, true) =>
                    game.Player1.healthChecker > game.Player2.healthChecker
                        ? "Time! Player 1 wins by higher HP!"
                        : game.Player2.healthChecker > game.Player1.healthChecker
                            ? "Time! Player 2 wins by higher HP!"
                            : "Time! Draw!",
                _ => "Game ended."
            };
            DrawString(height / 2, (width - msg.Length) / 2, msg);
            Flush();
        }
    }
}
