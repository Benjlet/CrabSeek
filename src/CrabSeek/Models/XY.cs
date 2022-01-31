namespace CrabSeek.Models
{
    public struct XY
    {
        public XY(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
        public static XY Zero => new(0, 0);
    }
}
