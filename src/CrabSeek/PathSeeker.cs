using CrabSeek.Models;

namespace CrabSeek
{
    /// <summary>
    /// Manages pathseeking requests for the supplied grid.
    /// </summary>
    public class PathSeeker
    {
        private readonly PathSeekerGrid _grid;

        /// <summary>
        /// Creates a new pathseeking instance, configured for the supplied grid.
        /// </summary>
        /// <param name="grid"></param>
        public PathSeeker(PathSeekerGrid grid)
        {
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
        }

        /// <summary>
        /// Finds a path between the requested start and end XY positions, customising the route calculation based on additional settings in the request.
        /// </summary>
        /// <param name="request">Request containing the start and end XY positions as well as custom route-calculation settings.</param>
        /// <returns>The result of the pathseeking request, including the node route, if found, and the overall result.</returns>
        /// <exception cref="ArgumentException"></exception>
        public PathSeekerResponse FindPath(PathSeekerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!_grid.IsValidPoint(request.Start) || !_grid.IsValidPoint(request.End))
                throw new ArgumentOutOfRangeException($"Requested points are not within the bounds of the grid; must be between '0,0' and '{_grid.Width}, {_grid.Height}'.");

            var pathSeeker = new PathSeekerService(_grid, request);

            return pathSeeker.FindPath();
        }
    }
}
