namespace CrabSeek
{
    /// <summary>
    /// Internal constant values for pathfinding calculations.
    /// </summary>
    internal class Constants
    {
        public const int ADJACENCY_COUNT_GRID = 4;
        public const int ADJACENCY_COUNT_DIAGIONAL = 8;

        public const byte OPEN_NODE_VALUE = 1;
        public const byte CLOSE_NODE_VALUE = 0;

        public const int G_INITIAL_VALUE = 0;
        public const int G_HORIZONTAL_INCREMENT = 20;
        public const double G_DIAGONAL_INCREMENT = 1.41;
        public const double H_TIEBREAKER_INCREMENT = 0.001;
    }
}
