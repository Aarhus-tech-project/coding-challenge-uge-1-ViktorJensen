using System;
using BoxingGame;
class Program
{
    private static void PrintBlue(string s ) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(s);
            Console.ResetColor();
        }
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var game = new Game();
        Console.Clear();
        PrintBlue(@"
       ___  ____  _  _______  _______  __  ______   _  _________ 
      / _ )/ __ \| |/_/  _/ |/ / ___/ /  |/  / _ | / |/ /  _/ _ |
     / _  / /_/ />  <_/ //    / (_ / / /|_/ / __ |/    // // __ |
    /____/\____/_/|_/___/_/|_/\___/ /_/  /_/_/ |_/_/|_/___/_/ |_|
        ");
        Console.WriteLine("\n               Press any key to start and Esc to quit.");
        Console.WriteLine("P1: A/D move, W punch, S block -- P2: Left/Right move, Up punch, Down block");
        Console.ReadKey(true);
        Console.Clear();
        game.Run();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }
}

