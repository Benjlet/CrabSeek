namespace CrabSeek.UnitTests
{
    internal class TestGrids
    {
        /// <summary>
        /// 8x8 open grid with equal node costs.
        /// </summary>
        public readonly static byte[,] OpenGrid8x8 = new byte[8, 8]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        /// <summary>
        /// 8x8 open grid with increasing costs towards the centre, other than one low-cost path.
        /// Consider [3,3] as the peak with a path down from [0,3] -> [3,3] being a low-cost path to the centre, which the pathfinder should prioritise over the clostly slope.
        /// </summary>
        public readonly static byte[,] OpenPyraGrid8x8 = new byte[8, 8]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 4, 4, 1, 4, 4, 4, 1 },
            { 1, 4, 8, 1, 8, 8, 4, 1 },
            { 1, 4, 8, 1, 8, 8, 4, 1 },
            { 1, 4, 8, 8, 8, 8, 4, 1 },
            { 1, 4, 8, 8, 8, 8, 4, 1 },
            { 1, 4, 4, 4, 4, 4, 4, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        /// <summary>
        /// 4x4 closed grid with no open nodes.
        /// </summary>
        public readonly static byte[,] ClosedGrid4x4 = new byte[4, 4]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        };
    }
}
