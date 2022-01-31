using CrabSeek.Calculators;
using CrabSeek.Models;
using NUnit.Framework;
using System;

namespace CrabSeek.UnitTests
{
    internal class HCalculatorTests
    {
        [Test]
        public void CalculateHWithDiagonalFormula_ShouldApplyDiagonalFormula()
        {
            PathSeekerRequest request = new(new XY(3, 3), new XY(4, 4))
            {
                Formula = DistanceFormula.Diagonal
            };

            HCalculator gCalculator = new(request);

            PathSeekerNode node = new() { X = 5, Y = 7 };

            int hModifier = gCalculator.CalculateHeuristic(node);

            Assert.That(hModifier, Is.EqualTo(8));
        }

        [Test]
        public void CalculateHWithEuclideanFormula_ShouldApplyEuclideanFormula()
        {
            PathSeekerRequest request = new(new XY(3, 3), new XY(4, 4))
            {
                Formula = DistanceFormula.Euclidean
            };

            HCalculator gCalculator = new(request);

            PathSeekerNode node = new() { X = 5, Y = 7 };

            int hModifier = gCalculator.CalculateHeuristic(node);

            Assert.That(hModifier, Is.EqualTo(6));
        }

        [Test]
        public void CalculateHWithManhattanFormula_ShouldApplyManhattanFormula()
        {
            PathSeekerRequest request = new(new XY(3, 3), new XY(4, 4))
            {
                Formula = DistanceFormula.Manhattan
            };

            HCalculator gCalculator = new(request);

            PathSeekerNode node = new() { X = 5, Y = 7 };

            int hModifier = gCalculator.CalculateHeuristic(node);

            Assert.That(hModifier, Is.EqualTo(8));
        }

        [Test]
        public void CalculateHWithTieBreaker_ShouldApplyTieBreaker()
        {
            PathSeekerRequest request = new(new XY(3, 3), new XY(4, 4))
            {
                UseTieBreaker = true
            };

            HCalculator gCalculator = new(request);

            PathSeekerNode node = new() { PX = 5, PY = 7 };

            int hTieBreaker = gCalculator.CalculateTieBreaker(node);
            int expected = (int)Math.Abs(2 * Constants.H_TIEBREAKER_INCREMENT);

            Assert.That(hTieBreaker, Is.EqualTo(expected));
        }
    }
}
