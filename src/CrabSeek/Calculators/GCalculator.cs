using CrabSeek.Models;

namespace CrabSeek.Calculators
{
    internal class GCalculator
    {
        private readonly PathSeekerGrid _grid;

        public GCalculator(PathSeekerGrid grid)
        {
            _grid = grid;
        }

        /// <summary>
        /// Calculate G, applying a punishment factor if enabled for diagonal checks.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parentNode"></param>
        /// <param name="lessenDiagonals"></param>
        /// <returns></returns>
        public int Calculate(PathSeekerNode node, PathSeekerNode parentNode, bool lessenDiagonals)
        {
            return lessenDiagonals
                ? parentNode.G + (int)(_grid[node.X, node.Y] * Constants.G_DIAGONAL_INCREMENT)
                : parentNode.G + _grid[node.X, node.Y];
        }

        /// <summary>
        /// Calculates updates to G if lessened directional changes have been requested, based on the alignment of the parent node's X and PX.
        /// </summary>
        public int CalculateDirectionModifier(PathSeekerNode node, bool lessenDirectionals, int horizontalOffset)
        {
            int directionModifier = 0;

            if (!lessenDirectionals)
                return directionModifier;

            if ((node.X - node.PX) != 0 && horizontalOffset == 0)
                directionModifier += Constants.G_HORIZONTAL_INCREMENT;

            if ((node.Y - node.PY) != 0 && horizontalOffset != 0)
                directionModifier += Constants.G_HORIZONTAL_INCREMENT;

            return directionModifier;
        }
    }
}
