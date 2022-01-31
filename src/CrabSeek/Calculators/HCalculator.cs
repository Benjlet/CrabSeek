using CrabSeek.Models;

namespace CrabSeek.Calculators
{
    internal class HCalculator
    {
        private readonly PathSeekerRequest _request;
        public HCalculator(PathSeekerRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Applies the supplied distance formula to calculate the initial node H value.
        /// </summary>
        public int CalculateHeuristic(PathSeekerNode node)
        {
            switch (_request.Formula)
            {
                case DistanceFormula.Diagonal:
                    int diagonal = Math.Min(Math.Abs(node.X - _request.End.X), Math.Abs(node.Y - _request.End.Y));
                    int straight = Math.Abs(node.X - _request.End.X) + Math.Abs(node.Y - _request.End.Y);
                    return _request.HeuristicEstimate * 2 * diagonal + _request.HeuristicEstimate * (straight - 2 * diagonal);

                case DistanceFormula.Euclidean:
                    return (int)(_request.HeuristicEstimate * Math.Sqrt(Math.Pow(node.X - _request.End.X, 2) + Math.Pow(node.Y - _request.End.Y, 2)));

                default:
                case DistanceFormula.Manhattan:
                    return _request.HeuristicEstimate * (Math.Abs(node.X - _request.End.X) + Math.Abs(node.Y - _request.End.Y));
            }
        }

        /// <summary>
        /// Prefer straight paths during tiebreak checks, applying a constant value to adjust the node priority.
        /// </summary>
        public int CalculateTieBreaker(PathSeekerNode node)
        {
            int dx1 = node.PX - _request.End.X;
            int dy1 = node.PY - _request.End.Y;
            int dx2 = _request.Start.X - _request.End.X;
            int dy2 = _request.Start.Y - _request.End.Y;

            return (int)(Math.Abs((dx1 * dy2) - (dx2 * dy1)) * Constants.H_TIEBREAKER_INCREMENT);
        }
    }
}
