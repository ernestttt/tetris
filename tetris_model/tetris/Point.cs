namespace Tetris
{
    internal struct Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        internal Point(int x, int y)
        {
            X = x; Y = y;
        }
    }
}
