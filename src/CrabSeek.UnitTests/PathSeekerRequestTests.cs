using CrabSeek.Models;
using NUnit.Framework;
using System;

namespace CrabSeek.UnitTests
{
    internal class PathSeekerRequestTests
    {
        [Test]
        public void InvalidHeuristic_ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PathSeekerRequest(XY.Zero, XY.Zero)
            {
                HeuristicEstimate = -1
            });
        }
    }
}
