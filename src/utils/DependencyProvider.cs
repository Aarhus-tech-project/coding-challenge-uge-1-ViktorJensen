namespace BoxingGame
{
    public class DependencyProvider
    {
        public char[,] Buffer { get; }
        public int Width { get; }
        public int Height { get; }

        public Action<int,int,string> DrawString { get; }   //https://www.geeksforgeeks.org/c-sharp/action-delegate-in-c-sharp/
        public Action<int,int,char> DrawChar { get; }

        public DependencyProvider(
            char[,] buffer,
            int width,
            int height,
            Action<int,int,string> drawString,
            Action<int,int,char> drawChar)
        {
            Buffer = buffer;
            Width = width;
            Height = height;
            DrawString = drawString;
            DrawChar = drawChar;
        }
    }
}