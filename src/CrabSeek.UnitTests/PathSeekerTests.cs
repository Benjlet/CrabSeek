using CrabSeek.Models;
using NUnit.Framework;
using System;
using System.Linq;

namespace CrabSeek.UnitTests
{
    internal class PathSeekerTests
    {
        [Test]
        public void NullGrid_ShouldThrowException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PathSeeker(null));
            Assert.That(ex?.ParamName == "grid");
        }

        [Test]
        public void NullRequest_ShouldThrowException()
        {
            var grid = new PathSeekerGrid(8);
            Assert.Throws<ArgumentNullException>(() => new PathSeeker(grid).FindPath(null));
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(-1, -1)]
        [TestCase(0, -1)]
        [TestCase(999, 0)]
        [TestCase(0, 999)]
        [TestCase(999, 999)]
        public void InvalidStartPoint_ShouldThrowException(int startX, int startY)
        {
            var grid = new PathSeekerGrid(8);
            var request = new PathSeekerRequest(new XY(startX, startY), new XY(grid.Height - 1, grid.Width - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new PathSeeker(grid).FindPath(request));
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(-1, -1)]
        [TestCase(0, -1)]
        [TestCase(999, 0)]
        [TestCase(0, 999)]
        [TestCase(999, 999)]
        public void InvalidEndPoint_ShouldThrowException(int endX, int endY)
        {
            var grid = new PathSeekerGrid(8);
            var request = new PathSeekerRequest(new XY(grid.Height - 1, grid.Width - 1), new XY(endX, endY));
            Assert.Throws<ArgumentOutOfRangeException>(() => new PathSeeker(grid).FindPath(request));
        }

        [Test]
        public void ZeroDistancePath_ShouldReturnSingleNode()
        {
            var grid = new PathSeekerGrid(8);
            var request = new PathSeekerRequest(new XY(0, 0), new XY(0, 0));
            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);

            if (result?.Path != null)
            {
                Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
                Assert.That(result.Path.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public void HorizontalRoute_ShouldReturnShortest()
        {
            var grid = new PathSeekerGrid(8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, 0);
            var request = new PathSeekerRequest(startPoint, endPoint);

            grid.OpenNodeLine(startPoint, endPoint);

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);

            if (result?.Path != null)
            {
                var path = result.Path.ToArray();
                Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
                Assert.That(path.Length, Is.EqualTo(endPoint.X - startPoint.X + 1));

                for (int i = startPoint.X; i <= endPoint.X; i++)
                    Assert.That(path[i].X, Is.EqualTo(i));
            }
        }

        [Test]
        public void VerticalRoute_ShouldReturnShortest()
        {
            var grid = new PathSeekerGrid(8);
            var startPoint = XY.Zero;
            var endPoint = new XY(0, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint);

            grid.OpenNodeLine(startPoint, endPoint);

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);

            if (result.Path != null)
            {
                var path = result.Path.ToArray();
                Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
                Assert.That(path.Length, Is.EqualTo(endPoint.Y - startPoint.Y + 1));

                for (int i = startPoint.Y; i <= endPoint.Y; i++)
                    Assert.That(path[i].Y, Is.EqualTo(i));
            }
        }

        [Test]
        public void DiagonalRoute_WhenNotEnabled_ShouldNotFindPath()
        {
            var grid = new PathSeekerGrid(8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint);

            grid.OpenNodeLine(startPoint, endPoint);

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathNotFound));
        }

        [Test]
        public void DiagonalRoute_WhenEnabled_ShouldFindPath()
        {
            var grid = new PathSeekerGrid(8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                AllowDiagonals = true
            };

            grid.OpenNodeLine(startPoint, endPoint);

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);

            if (result.Path != null)
            {
                var path = result.Path.ToArray();
                Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
                Assert.That(path.Length, Is.EqualTo(endPoint.Y - startPoint.Y + 1));

                for (int i = startPoint.Y; i <= endPoint.Y; i++)
                {
                    Assert.That(path[i].Y, Is.EqualTo(i));
                    Assert.That(path[i].X, Is.EqualTo(i));
                }
            }
        }

        [Test]
        public void DiagonalRoute_WhenChangeDirectionLessened_ShouldPrioritiseOtherPath()
        {
            var grid = new PathSeekerGrid(8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                AllowDiagonals = true,
                LessenDirectionalChange = true
            };

            grid.OpenNodeLine(startPoint, endPoint);
            grid.OpenNodeLine(startPoint, new XY(0, 7));
            grid.OpenNodeLine(new XY(1, 7), new XY(6, 7));

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);

            if (result.Path != null)
            {
                var path = result.Path.ToArray();

                Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
                Assert.That(path.Length, Is.EqualTo(14));

                for (int i = 0; i <= 6; i++)
                {
                    Assert.That(path[i].Y, Is.EqualTo(i));
                    Assert.That(path[i].X, Is.EqualTo(0));
                }

                for (int i = 1; i <= 7; i++)
                {
                    Assert.That(path[6 + i].Y, Is.EqualTo(7));
                    Assert.That(path[6 + i].X, Is.EqualTo(i));
                }
            }
        }

        [Test]
        public void HighCostGrid_ShouldFollowLowCostPath()
        {
            var grid = new PathSeekerGrid(TestGrids.OpenPyraGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(3, 3);

            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                AllowDiagonals = true
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);

            if (result.Path != null)
            {
                var path = result.Path.ToArray();

                Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
                Assert.That(path.Length, Is.EqualTo(6));

                for (int i = 0; i < 3; i++)
                {
                    Assert.That(path[i].Y, Is.EqualTo(i));
                    Assert.That(path[i].X, Is.EqualTo(0));
                }

                for (int i = 1; i < 3; i++)
                {
                    Assert.That(path[2 + i].Y, Is.EqualTo(3));
                    Assert.That(path[2 + i].X, Is.EqualTo(i));
                }
            }
        }

        [Test]
        public void AllClosedNodes_ShouldNotFindPath()
        {
            var grid = new PathSeekerGrid(TestGrids.ClosedGrid4x4);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint);

            grid.OpenNodeLine(startPoint, endPoint);

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathNotFound));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AllOpenNodes_WithTieBreaker_ShouldFindPath(bool allowDiagonals)
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                UseTieBreaker = true,
                AllowDiagonals = allowDiagonals
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AllOpenNodes_WithLessenDiagonals_ShouldFindPath(bool allowDiagonals)
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                LessenDiagonals = true,
                AllowDiagonals = allowDiagonals
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AllOpenNodes_WithLessenDirectionalChange_ShouldFindPath(bool allowDiagonals)
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                LessenDirectionalChange = true,
                AllowDiagonals = allowDiagonals
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AllOpenNodes_WithReopenClosedNodes_ShouldFindPath(bool allowDiagonals)
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                ReopenClosedNodes = true,
                AllowDiagonals = allowDiagonals
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
        }

        [Test]
        public void AllOpenNodes_WithHeuristicEstimate_ShouldFindPath()
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                HeuristicEstimate = 4
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
        }

        [Test]
        [TestCase(DistanceFormula.Euclidean)]
        [TestCase(DistanceFormula.Manhattan)]
        [TestCase(DistanceFormula.Diagonal)]
        public void AllOpenNodes_WithDistanceFormula_ShouldFindPath(DistanceFormula formula)
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);
            var startPoint = XY.Zero;
            var endPoint = new XY(grid.Width - 1, grid.Height - 1);
            var request = new PathSeekerRequest(startPoint, endPoint)
            {
                Formula = formula
            };

            var result = new PathSeeker(grid).FindPath(request);

            Assert.That(result.Path, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(PathSeekerResult.PathFound));
        }
    }
}
