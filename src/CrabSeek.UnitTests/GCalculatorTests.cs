using CrabSeek.Calculators;
using CrabSeek.Models;
using NUnit.Framework;

namespace CrabSeek.UnitTests
{
    internal class GCalculatorTests
    {
        [Test]
        public void CalculateGWithoutDiagonals_ShouldBeParentGPlusXYValue()
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);

            grid.EditNode(5, 5, 5);
            
            GCalculator gCalculator = new(grid);

            var node = new PathSeekerNode()
            {
                X = 5,
                Y = 5
            };

            var parentNode = new PathSeekerNode()
            {
                G = 5
            };

            int g = gCalculator.Calculate(node, parentNode, lessenDiagonals: false);
            
            Assert.That(g, Is.EqualTo(parentNode.G + grid[node.X, node.Y]));

        }

        [Test]
        public void CalculateGWithDiagonals_ShouldMultiplyByGIncrement()
        {
            var grid = new PathSeekerGrid(TestGrids.OpenGrid8x8);

            grid.EditNode(5, 5, 5);

            GCalculator gCalculator = new(grid);

            var node = new PathSeekerNode()
            {
                X = 5,
                Y = 5
            };

            var parentNode = new PathSeekerNode()
            {
                G = 5
            };

            int g = gCalculator.Calculate(node, parentNode, lessenDiagonals: true);

            Assert.That(g, Is.EqualTo((int)(parentNode.G + grid[node.X, node.Y] * Constants.G_DIAGONAL_INCREMENT)));
        }

        [Test]
        public void CalculateGLessenDirectionals_ShouldReturnZero()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            int gModifier = gCalculator.CalculateDirectionModifier(new PathSeekerNode(), lessenDirectionals: false, 0);

            Assert.That(gModifier, Is.Zero);
        }

        [Test]
        public void CalculateGAlignedWithParentHorizontally_ShouldReturnZero()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            var node = new PathSeekerNode()
            {
                X = 5,
                PX = 5
            };

            int gModifier = gCalculator.CalculateDirectionModifier(node, lessenDirectionals: true, 0);

            Assert.That(gModifier, Is.Zero);
        }

        [Test]
        public void CalculateGAlignedWithParentVertically_ShouldReturnZero()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            var node = new PathSeekerNode()
            {
                Y = 5,
                PY = 5
            };

            int gModifier = gCalculator.CalculateDirectionModifier(node, lessenDirectionals: true, 0);

            Assert.That(gModifier, Is.Zero);
        }

        [Test]
        public void CalculateGParentXMisalignedOffsetZero_ShouldReturnModifier()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            var node = new PathSeekerNode()
            {
                X = 5,
                PX = 7
            };

            int gModifier = gCalculator.CalculateDirectionModifier(node, lessenDirectionals: true, 0);

            Assert.That(gModifier, Is.EqualTo(Constants.G_HORIZONTAL_INCREMENT));
        }

        [Test]
        public void CalculateGParentYMisalignedOffsetZero_ShouldReturnZero()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            var node = new PathSeekerNode()
            {
                Y = 5,
                PY = 7
            };

            int gModifier = gCalculator.CalculateDirectionModifier(node, lessenDirectionals: true, 0);

            Assert.That(gModifier, Is.Zero);
        }

        [Test]
        public void CalculateGParentYMisalignedAndHorizontalOffset_ShouldReturnZero()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            var node = new PathSeekerNode()
            {
                X = 5,
                PX = 7
            };

            int gModifier = gCalculator.CalculateDirectionModifier(node, lessenDirectionals: true, 2);

            Assert.That(gModifier, Is.Zero);
        }

        [Test]
        public void CalculateGParentYMisalignedAndVerticalOffset_ShouldReturnModifier()
        {
            GCalculator gCalculator = new(new PathSeekerGrid(TestGrids.OpenGrid8x8));
            var node = new PathSeekerNode()
            {
                Y = 5,
                PY = 7
            };

            int gModifier = gCalculator.CalculateDirectionModifier(node, lessenDirectionals: true, 2);

            Assert.That(gModifier, Is.EqualTo(Constants.G_HORIZONTAL_INCREMENT));
        }
    }
}
