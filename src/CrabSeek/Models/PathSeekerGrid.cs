namespace CrabSeek.Models
{
    /// <summary>
    /// The pathfinding grid with a base-2 size (e.g. 1024 x 1024)
    /// </summary>
    public class PathSeekerGrid
    {
        private readonly byte[,] _grid;

        /// <summary>
        /// The byte value at X,Y.
        /// </summary>
        public byte this[int x, int y] => _grid[x, y];

        /// <summary>
        /// Sets a base-2 grid, based on the supplied two-dimensional byte array; the input must be valid base-2 grid (e.g. 1024x1024).
        /// Byte values of 0 are considered inaccessible; byte values of 1 are considered accessible.
        /// </summary>
        /// <param name="grid"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public PathSeekerGrid(byte[,] grid)
        {
            if (grid == null)
                throw new ArgumentNullException(paramName: nameof(grid));

            int width = grid.GetUpperBound(0) + 1;
            int height = grid.GetUpperBound(1) + 1;

            if (width != height || (width & (width - 1)) + (height & (height - 1)) != 0)
                throw new ArgumentOutOfRangeException("Invalid grid size: must have equal height/width and be base-2.");

            Width = width;
            Height = height;
            _grid = grid;
        }

        /// <summary>
        /// Sets a base-2 grid of equal width and height, based on the supplied width; the input must be a base-2 value.
        /// The grid is created as inaccessible (0) by default.
        /// </summary>
        /// <param name="gridWidth"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public PathSeekerGrid(int gridWidth)
        {
            if ((gridWidth & (gridWidth - 1)) != 0)
                throw new ArgumentOutOfRangeException("Invalid grid size: must be base-2.");

            _grid = new byte[gridWidth, gridWidth];

            Width = gridWidth;
            Height = gridWidth;
        }

        public int Width { get; }
        public int Height { get; }

        public bool IsValidPoint(XY point) => IsValidPoint(point.X, point.Y);
        public bool IsValidPoint(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        public void CloseAllNodes()
        {
            CloseNodeArea(new XY(0, 0), new XY(Width - 1, Height - 1));
        }

        public void OpenAllNodes()
        {
            OpenNodeArea(new XY(0, 0), new XY(Width - 1, Height - 1));
        }

        public void EditAllNodes(byte newValue)
        {
            EditNodeArea(new XY(0, 0), new XY(Width - 1, Height - 1), newValue);
        }

        public void OpenNode(int x, int y) => _grid[x, y] = Constants.OPEN_NODE_VALUE;
        public void CloseNode(int x, int y) => _grid[x, y] = Constants.CLOSE_NODE_VALUE;
        public void EditNode(int x, int y, byte newValue) => _grid[x, y] = newValue;

        /// <summary>
        /// Opens nodes between the start and end points, accepting straight lines or exact 45-degree diagonal lines.
        /// These nodes will be considered accessible during pathfinding.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void OpenNodeLine(XY start, XY end)
        {
            if (!EditNodeValues(start, end, Constants.OPEN_NODE_VALUE))
                throw new ArgumentOutOfRangeException("Nodes must be in a straight line or directly diagonal.");
        }

        /// <summary>
        /// Closes nodes between the start and end points, accepting straight lines or exact 45-degree diagonal lines.
        /// These nodes will be considered accessible during pathfinding.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void CloseNodeLine(XY start, XY end)
        {
            if (!EditNodeValues(start, end, Constants.CLOSE_NODE_VALUE))
                throw new ArgumentOutOfRangeException("Nodes must be in a straight line or directly diagonal.");
        }

        /// <summary>
        /// Opens nodes between the start and end points, accepting straight lines or exact 45-degree diagonal lines.
        /// These nodes will be considered accessible during pathfinding.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void EditNodeLine(XY start, XY end, byte newValue)
        {
            if (!EditNodeValues(start, end, newValue))
                throw new ArgumentOutOfRangeException("Nodes must be in a straight line or directly diagonal.");
        }


        public void OpenNodeArea(XY start, XY end)
        {
            EditNodeArea(start, end, Constants.OPEN_NODE_VALUE);
        }

        public void CloseNodeArea(XY start, XY end)
        {
            EditNodeArea(start, end, Constants.CLOSE_NODE_VALUE);
        }

        public void EditNodeArea(XY start, XY end, byte newValue)
        {
            if (!IsValidPoint(start) || !IsValidPoint(end))
                throw new ArgumentOutOfRangeException($"Start and end nodes must be on the grid (0,0 - {Width},{Height}).");

            if (start.X > end.X || start.Y > end.Y)
            {
                var switchStart = start;
                start = end;
                end = switchStart;
            }

            if (start.X == end.X)
            {
                EditNodeLine(start, end, newValue);
            }
            else if (start.Y == end.Y)
            {
                EditNodeLine(start, end, newValue);
            }
            else
            {
                for (int i = start.X; i <= end.X; i++)
                    EditNodeLine(new XY(i, start.Y), new XY(i, end.Y), newValue);
            }

            if (!EditNodeValues(start, end, newValue))
                throw new ArgumentOutOfRangeException("Nodes must be in a straight line or directly diagonal.");
        }

        private bool EditNodeValues(XY start, XY end, byte newValue)
        {
            if (!IsValidPoint(start) || !IsValidPoint(end))
                return false;

            if (start.X > end.X || start.Y > end.Y)
            {
                var switchStart = start;
                start = end;
                end = switchStart;
            }

            if (start.X == end.X)
            {
                for (int i = start.Y; i <= end.Y; i++)
                    _grid[start.X, i] = newValue;
            }
            else if (start.Y == end.Y)
            {
                for (int i = start.X; i <= end.X; i++)
                    _grid[i, start.Y] = newValue;
            }
            else if ((start.X - end.X) == (start.Y - end.Y))
            {
                for (int i = start.X; i <= end.X; i++)
                    _grid[i, i] = newValue;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
