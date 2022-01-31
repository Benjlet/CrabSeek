using CrabSeek.Calculators;
using CrabSeek.Models;

namespace CrabSeek
{
    /// <summary>
    /// Service for managing a PriorityQueue and custom calculation for a path between requested start and end nodes.
    /// </summary>
    internal class PathSeekerService
    {
        private readonly PathSeekerGrid _grid;
        private readonly PriorityQueue<PathSeekerNode, int> _priorityQueue;
        private readonly PathSeekerRequest _request;
        private readonly List<PathSeekerNode> _path;
        private readonly HCalculator _hCalculator;
        private readonly GCalculator _gCalculator;

        internal PathSeekerService(PathSeekerGrid grid, PathSeekerRequest request)
        {
            _grid = grid;
            _request = request;
            _path = new List<PathSeekerNode>();
            _hCalculator = new HCalculator(_request);
            _gCalculator = new GCalculator(_grid);
            _priorityQueue = new PriorityQueue<PathSeekerNode, int>();
        }

        /// <summary>
        /// Builds and returns a route containing the nodes between the requested start and end XY positions.
        /// The start node is added to a queue, processing priority nodes until the end point where the result is backtracked to get the path.
        /// </summary>
        internal PathSeekerResponse FindPath()
        {
            PathSeekerNode startNode = new()
            {
                G = Constants.G_INITIAL_VALUE,
                H = _request.HeuristicEstimate,
                X = _request.Start.X,
                Y = _request.Start.Y
            };

            EnqueueNode(startNode);

            while (_priorityQueue.Count > 0)
            {
                if (PriorityNodeIsEnd())
                {
                    BacktrackRoute();

                    return new PathSeekerResponse()
                    {
                        Path = _path.Select(p => new XY(p.X, p.Y)),
                        Result = PathSeekerResult.PathFound
                    };
                }
            }

            return new PathSeekerResponse()
            {
                Result = PathSeekerResult.PathNotFound
            };
        }

        private void EnqueueNode(PathSeekerNode node) => _priorityQueue.Enqueue(node, node.G + node.H);

        /// <summary>
        /// Gets the latest priority node and checks if the end XY has been reached. If not, adjacent node properties are calculated then added to the PQ.
        /// </summary>
        /// <returns><see langword="true"/> if the priority node in the QA matches the end XY; otherwise, <see langword="false"/>.</returns>
        private bool PriorityNodeIsEnd()
        {
            var parentNode = _priorityQueue.Dequeue();
            _path.Add(parentNode);

            if (parentNode.Equals(_request.End.X, _request.End.Y))
                return true;

            ProcessAdjacentNodes(parentNode);
            return false;
        }

        /// <summary>
        /// Traverses back through parent XY values from the end of the route to the start, removing oustanding nodes and leaving only the calculated route.
        /// </summary>
        private void BacktrackRoute()
        {
            PathSeekerNode lastNode = _path[^1];

            for (int i = _path.Count - 1; i >= 0; i--)
            {
                if (_path[i].Equals(lastNode.PX, lastNode.PY) || i == _path.Count - 1)
                {
                    lastNode = _path[i];
                }
                else
                {
                    _path.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Processes all nodes that are considered adjacent to the supplied node and on-grid.
        /// Any that are considered unopened and valid have their GH values calculated for F, adding that node to the PQ.
        /// </summary>
        private void ProcessAdjacentNodes(PathSeekerNode parentNode)
        {
            sbyte[,] positionOffset = _request.AllowDiagonals
                ? new sbyte[Constants.ADJACENCY_COUNT_DIAGIONAL, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } }
                : new sbyte[Constants.ADJACENCY_COUNT_GRID, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

            int horizontalOffset = _request.LessenDirectionalChange ? parentNode.X - parentNode.PX : 0;

            for (int i = 0; i < (_request.AllowDiagonals ? Constants.ADJACENCY_COUNT_DIAGIONAL : Constants.ADJACENCY_COUNT_GRID); i++)
            {
                PathSeekerNode adjacentNode = new()
                {
                    X = parentNode.X + positionOffset[i, 0],
                    Y = parentNode.Y + positionOffset[i, 1],
                    PX = parentNode.X,
                    PY = parentNode.Y
                };

                if (!_grid.IsValidPoint(adjacentNode.X, adjacentNode.Y) || _priorityQueue.UnorderedItems.Any(i => i.Element.Equals(adjacentNode.X, adjacentNode.Y)))
                    continue;

                bool lessenDiagonals = _request.LessenDiagonals && i >= Constants.ADJACENCY_COUNT_GRID;
                int gValue = _gCalculator.Calculate(adjacentNode, parentNode, lessenDiagonals);

                if (gValue == parentNode.G || _path.Any(n => n.Equals(adjacentNode.X, adjacentNode.Y) && (_request.ReopenClosedNodes || n.G <= gValue)))
                    continue;

                int gDirection = _gCalculator.CalculateDirectionModifier(adjacentNode, _request.LessenDirectionalChange, horizontalOffset);
                int hTieBreaker = _request.UseTieBreaker ? _hCalculator.CalculateTieBreaker(adjacentNode) : 0;
                int hDistance = _hCalculator.CalculateHeuristic(adjacentNode);

                adjacentNode.H = hDistance + hTieBreaker;
                adjacentNode.G = gValue + gDirection;

                EnqueueNode(adjacentNode);
            }
        }
    }
}
