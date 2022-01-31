using CrabSeek.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CrabSeek.UnitTests
{
    public class PathSeekerPerformance
    {
        [Test]
        [TestCase(64)]
        [TestCase(128)]
        [TestCase(256)]
        [TestCase(512)]
        [TestCase(1024)]
        [TestCase(2048)]
        public void Performance(int gridWidth)
        {
            var grid = new byte[gridWidth, gridWidth];

            for (int i = 0; i < gridWidth; i++)
                for (int j = 0; j < gridWidth; j++)
                    grid[i, j] = 1;

            var pf = new PathSeeker(new PathSeekerGrid(grid));
            var sw = Stopwatch.StartNew();
            var random = new Random();

            int timesToRun = 2;

            var result = new List<XY>();

            for (int i = 0; i < timesToRun; i++)
            {
                result = pf.FindPath(new PathSeekerRequest(new XY(0, 0), new XY(random.Next(0, gridWidth), random.Next(0, gridWidth)))).Path?.ToList();
            }

            Assert.IsNotNull(result);

            double meanRuntime = sw.Elapsed.TotalSeconds / timesToRun;

            if (result != null)
            {
                Console.WriteLine($"Found {result.Count} in {meanRuntime} seconds.");
                Assert.IsTrue(result.Count > 0);
            }
            else
            {
                Console.WriteLine($"Found 0 in {meanRuntime} seconds.");
            }    
        }
    }
}