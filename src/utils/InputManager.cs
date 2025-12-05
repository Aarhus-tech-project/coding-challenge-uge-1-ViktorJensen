
using System;
using System.Collections.Generic;
namespace BoxingGame
{
    public class InputManager
    {
        private HashSet<ConsoleKey> keysDown = new HashSet<ConsoleKey>();
        /// <summary>
        /// </summary>
        public void PollInputs()
        {
            keysDown.Clear();
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                keysDown.Add(key);
            }
        }
        public bool IsKeyDown(ConsoleKey key)
        {
            return keysDown.Contains(key);
        }
    }
}