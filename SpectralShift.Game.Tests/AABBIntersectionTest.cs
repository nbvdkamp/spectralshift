using System;
using NUnit.Framework;
using osuTK;

namespace SpectralShift.Game.Tests
{
    [TestFixture]
    public class AABBIntersectionTest
    {
        [Test]
        public void HitRight()
        {
            Ray ray = new Ray(-2 * Vector2.UnitX, Vector2.UnitX);
            assertHit(ray, Vector2.One, -Vector2.UnitX, false, 1);
        }

        [Test]
        public void HitDown()
        {
            Ray ray = new Ray(-2 * Vector2.UnitY, Vector2.UnitY);
            assertHit(ray, Vector2.One, -Vector2.UnitY, false, 1);
        }

        [Test]
        public void HitInside()
        {
            Ray ray = new Ray(Vector2.Zero, Vector2.UnitX);
            assertHit(ray, Vector2.One, Vector2.UnitX, true, 1);
        }

        [Test]
        public void HitInsideDown()
        {
            Ray ray = new Ray(Vector2.Zero, Vector2.UnitY);
            assertHit(ray, Vector2.One, Vector2.UnitY, true, 1);
        }

        [Test]
        public void HitInsideDownRightBottom()
        {
            Ray ray = new Ray(Vector2.Zero, new Vector2(1, 2).Normalized());
            assertHit(ray, Vector2.One, Vector2.UnitY, true, (float)Math.Sqrt(5f / 4));
        }

        [Test]
        public void HitInsideDownRightSide()
        {
            Ray ray = new Ray(Vector2.Zero, new Vector2(2, 1).Normalized());
            assertHit(ray, Vector2.One, Vector2.UnitX, true, (float)Math.Sqrt(5f / 4));
        }

        [Test]
        public void HitInsideDownRightSideWrongCase()
        {
            Ray ray = new Ray(new Vector2(-32.101746f, 14.414429f), Vector2.One.Normalized());
            assertHit(ray, 78.32393f * Vector2.One, Vector2.UnitY, true, 90.3816833f);
        }

        [Test]
        public void Miss()
        {
            Ray ray = new Ray(-3 * Vector2.UnitX, Vector2.One);
            assertMiss(ray, Vector2.One);
        }

        private void assertHit(Ray ray, Vector2 corner, Vector2 expectedNormal, bool expectedInsideShape, float expectedDistance)
        {
            var result = RectangleObstacle.IntersectAabbAtOrigin(ray, corner);
            Assert.NotNull(result);
            Assert.AreEqual(expectedNormal, result.Value.Normal);
            Assert.AreEqual(expectedInsideShape, result.Value.InsideShape);
            Assert.AreEqual(expectedDistance, result.Value.DistanceFromOrigin);
        }

        private void assertMiss(Ray ray, Vector2 corner)
        {
            var result = RectangleObstacle.IntersectAabbAtOrigin(ray, corner);
            Assert.IsNull(result);
        }
    }
}
