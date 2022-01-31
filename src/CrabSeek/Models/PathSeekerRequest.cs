namespace CrabSeek.Models
{
    /// <summary>
    /// Request details for searching the start and end XY positions, including multiple settings that
    /// can be changed to impact the behaviour of the adapated A* pathfinding algorithm.
    /// </summary>
    public class PathSeekerRequest
    {
        private int _heuristicEstimate = 2;

        /// <summary>
        /// The target start and end XY positions on the grid.
        /// </summary>
        public PathSeekerRequest(XY start, XY end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// The XY of the start node on the grid.
        /// </summary>
        public XY Start { get; }

        /// <summary>
        /// The XY of the end node on the grid.
        /// </summary>
        public XY End { get; }

        /// <summary>
        /// The equation used to calculate the heuristic. Defaults to Manhattan (optimised for 4-directional grids).
        /// </summary>
        public DistanceFormula Formula { get; set; } = DistanceFormula.Manhattan;

        /// <summary>
        /// Consider diagonal nodes when finding a path. Defaults to <see langword="false"/> (only UDLR nodes).
        /// </summary>
        public bool AllowDiagonals { get; set; }

        /// <summary>
        /// Puts lower priority on potential diagonal routes by altering the node G value. Defaults to <see langword="false"/>.
        /// </summary>
        public bool LessenDiagonals { get; set; }

        /// <summary>
        /// Allows reopening of closed nodes where the F cost is considered higher priority. Provides a better path at a performance cost. Defaults to <see langword="false"/>.
        /// </summary>
        public bool ReopenClosedNodes { get; set; }

        /// <summary>
        /// Puts lower priority on potential directional changes by altering the node G value. Defaults to <see langword="false"/>.
        /// </summary>
        public bool LessenDirectionalChange { get; set; }

        /// <summary>
        /// When multiple paths with the same priority F cost for reaching the end XY are identified, setting this property 
        /// to <see langword="true"/> means further options are researched by incrementing node H and multiplying by a constant factor
        /// until a priority route is identified. Defaults to <see langword="false"/>.
        /// </summary>
        public bool UseTieBreaker { get; set; }

        /// <summary>
        /// The estimated distance from the current position to the end node; this is calculated per-node as part of the algorithm, 
        /// however an accurate initial estimate results reduces pathfinding overhead. Defaults to 2; must be greater than 0.
        /// </summary>
        public int HeuristicEstimate
        {
            get
            {
                return _heuristicEstimate;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Estimate cannot be less than 0.");

                _heuristicEstimate = value;
            }
        }
    }
}
