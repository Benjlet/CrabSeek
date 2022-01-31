using CrabSeek.Models;
using NUnit.Framework;
using System;

namespace CrabSeek.UnitTests
{
    internal class PathSeekerGridTests
    {
        [Test]
        public void NullGrid_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new PathSeekerGrid(null));
        }

        [Test]
        public void NonBase2Grid_ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PathSeekerGrid(7));
        }

        [Test]
        public void CloseAllNodes_ShouldCloseAllNodes()
        {
            var byteGrid = new byte[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    byteGrid[i, j] = 1;

            var grid = new PathSeekerGrid(byteGrid);

            for (int i = 0; i < 8; i++)
                Assert.IsTrue(grid[i, i] == 1);

            grid.CloseAllNodes();

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, i] == 0);
        }

        [Test]
        public void OpenAllNodes_ShouldOpenAllNodes()
        {
            var grid = new PathSeekerGrid(8);

            for (int i = 0; i < 8; i++)
                Assert.IsTrue(grid[i, i] == 0);

            grid.OpenAllNodes();

            for (int i = 0; i < 8; i++)
                Assert.IsTrue(grid[i, i] == 1);
        }

        [Test]
        public void EditAllNodes_ShouldEditAllNodes()
        {

        }

        [Test]
        public void CloseDiagonal_ShouldCloseNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    byteGrid[i, j] = 1;

            var grid = new PathSeekerGrid(byteGrid);

            grid.CloseNodeLine(new XY(3, 3), new XY(6, 6));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == ((i == j && j.IsOneOf(3, 4, 5, 6)) ? 0 : 1));
        }

        [Test]
        public void OpenDiagonal_ShouldCloseNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.OpenNodeLine(new XY(3, 3), new XY(6, 6));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == ((i == j && j.IsOneOf(3, 4, 5, 6)) ? 1 : 0));
        }

        [Test]
        public void EditDiagonal_ShouldEditNodesInLine()
        {

        }

        [Test]
        public void CloseHorizontal_ShouldCloseNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    byteGrid[i, j] = 1;

            var grid = new PathSeekerGrid(byteGrid);

            grid.CloseNodeLine(new XY(3, 3), new XY(6, 3));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (i.IsOneOf(3, 4, 5, 6) && j == 3 ? 0 : 1));
        }

        [Test]
        public void OpenHorizontal_ShouldCloseNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.OpenNodeLine(new XY(3, 3), new XY(6, 3));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (i.IsOneOf(3, 4, 5, 6) && j == 3 ? 1 : 0));
        }

        [Test]
        public void EditHorizontal_ShouldEditNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.EditNodeLine(new XY(3, 3), new XY(6, 3), 4);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (i.IsOneOf(3, 4, 5, 6) && j == 3 ? 4 : 0));
        }

        [Test]
        public void CloseVertical_ShouldCloseNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    byteGrid[i, j] = 1;

            var grid = new PathSeekerGrid(byteGrid);

            grid.CloseNodeLine(new XY(3, 3), new XY(3, 6));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j.IsOneOf(3, 4, 5, 6) && i == 3 ? 0 : 1));
        }

        [Test]
        public void OpenVertical_ShouldOpenNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.OpenNodeLine(new XY(3, 3), new XY(3, 6));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j.IsOneOf(3, 4, 5, 6) && i == 3 ? 1 : 0));
        }

        [Test]
        public void EditVertical_ShouldEditNodesInLine()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.EditNodeLine(new XY(3, 3), new XY(3, 6), 4);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j.IsOneOf(3, 4, 5, 6) && i == 3 ? 4 : 0));
        }

        [Test]
        public void CloseNodeArea_ShouldCloseNodeArea()
        {
            var byteGrid = new byte[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    byteGrid[i, j] = 1;

            var grid = new PathSeekerGrid(byteGrid);

            grid.CloseNodeArea(new XY(3, 3), new XY(5, 5));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j.IsOneOf(3, 4, 5) && i.IsOneOf(3, 4, 5) ? 0 : 1));
        }

        [Test]
        public void OpenNodeArea_ShouldOpenNodeArea()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.OpenNodeArea(new XY(3, 3), new XY(5, 5));

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j.IsOneOf(3, 4, 5) && i.IsOneOf(3, 4, 5) ? 1 : 0));
        }

        [Test]
        public void EditNodeArea_ShouldEditNodeArea()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.EditNodeArea(new XY(5, 5), new XY(3, 3), 4);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j.IsOneOf(3, 4, 5) && i.IsOneOf(3, 4, 5) ? 4 : 0));
        }

        [Test]
        public void CloseNode_ShouldCloseNode()
        {
            var byteGrid = new byte[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    byteGrid[i, j] = 1;

            var grid = new PathSeekerGrid(byteGrid);

            grid.CloseNode(3, 3);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j == 3 && i == 3 ? 0 : 1));
        }

        [Test]
        public void OpenNode_ShouldOpenNode()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.OpenNode(3, 3);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j == 3 && i == 3 ? 1 : 0));
        }

        [Test]
        public void EditNode_ShouldEditNode()
        {
            var byteGrid = new byte[8, 8];

            var grid = new PathSeekerGrid(byteGrid);

            grid.EditNode(3, 3, 4);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Assert.IsTrue(grid[i, j] == (j == 3 && i == 3 ? 4 : 0));
        }
    }
}
