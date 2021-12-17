namespace WormsWorld.model
{
    public class Direction
    {
        public static readonly Direction Right = new(1, 0);
        public static readonly Direction Left = new(-1, 0);
        public static readonly Direction Up = new(0, 1);
        public static readonly Direction Down = new(0, -1);

        private readonly int _x;
        private readonly int _y;

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        private Direction(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}