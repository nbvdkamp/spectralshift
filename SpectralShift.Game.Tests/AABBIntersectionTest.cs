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
            Ray ray = new Ray(2 * Vector2.UnitY, -Vector2.UnitY);
            assertHit(ray, Vector2.One, Vector2.UnitY, false, 1);
        }

        [Test]
        public void HitInside()
        {
            Ray ray = new Ray(Vector2.Zero, Vector2.UnitX);
            assertHit(ray, Vector2.One, Vector2.UnitX, true, 1);
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
