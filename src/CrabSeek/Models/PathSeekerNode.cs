namespace CrabSeek.Models
{
    /// <summary>
    /// A grid node; its values are set as part of the pathfinding algorithm.
    /// </summary>
    internal struct PathSeekerNode
    {
        /// <summary>
        /// The exact cost of the path from the starting node to any node.
        /// </summary>
        public int G;

        /// <summary>
        /// The heuristic estimated cost of moving from this node to the final cell.
        /// </summary>
        public int H;

        /// <summary>
        /// X position of this grid node.
        /// </summary>
        public int X;

        /// <summary>
        /// Y coordinate of this grid node.
        /// </summary>
        public int Y;

        /// <summary>
        /// X position of the parent node.
        /// </summary>
        public int PX;

        /// <summary>
        /// Y position of the parent node.
        /// </summary>
        public int PY;

        public bool Equals(int x, int y) => X == x && Y == y;
    }
}
