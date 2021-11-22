using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace SpectralShift.Game
{
    public class RectangleObstacle : Obstacle
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChild = Shape = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };

            Setup();
        }

        public override IntersectionResult? Intersects(Ray ray)
        {
            float angle = (float)Math.PI / 180f * Rotation;
            Matrix2 rotation = Matrix2.CreateRotation(angle);
            Matrix2 inverseRotation = Matrix2.CreateRotation(-angle);
            Ray rotatedRay = new Ray(Util.Multiply(rotation, ray.Origin - Position), Util.Multiply(rotation, ray.Direction));

            Vector2 corner = new Vector2(Scale.X * Width / 2, Scale.Y * Height / 2);
            AABBIntersectionResult? result = IntersectAabbAtOrigin(rotatedRay, corner);

            if (result.HasValue)
            {
                return new IntersectionResult
                {
                    Position = ray.Origin + result.Value.DistanceFromOrigin * ray.Direction,
                    Normal = Util.Multiply(inverseRotation, result.Value.Normal),
                    InsideShape = result.Value.InsideShape,
                    Material = Material,
                    IndexOfRefraction = IndexOfRefraction,
                };
            }

            return null;
        }

        public struct AABBIntersectionResult
        {
            public bool InsideShape;
            public Vector2 Normal;
            public float DistanceFromOrigin;
        }

        public static AABBIntersectionResult? IntersectAabbAtOrigin(Ray ray, Vector2 corner)
        {
            // Axis aligned box ray intersection centered at the origin
            Vector2 min = -corner;
            Vector2 max = corner;
            Vector2 inverseDirection = new Vector2(1 / ray.Direction.X, 1 / ray.Direction.Y);

            Vector2 t0 = Util.ElementwiseMultiply(min - ray.Origin, inverseDirection);
            Vector2 t1 = Util.ElementwiseMultiply(max - ray.Origin, inverseDirection);
            Vector2 tmin = Util.ElementwiseMin(t0, t1);
            Vector2 tmax = Util.ElementwiseMax(t0, t1);

            bool behindRay = Util.MinElement(tmax) < 0;

            if (behindRay || Util.MaxElement(tmin) > Util.MinElement(tmax))
                return null;

            bool originInRect = Math.Abs(ray.Origin.X) < Math.Abs(corner.X) &&
                                Math.Abs(ray.Origin.Y) < Math.Abs(corner.Y);
            // == Util.MaxElement(tmin) >= 0

            float distance;
            bool hitOnSide;

            if (!originInRect)
            {
                distance = Util.MaxElement(tmin);
                hitOnSide = Util.MaxElement(tmin) == tmin.X;
            }
            else
            {
                distance = Util.MinElement(tmax);
                hitOnSide = Util.MinElement(tmax) == tmax.X;
            }

            Vector2 normal;

            if (hitOnSide)
                normal = Vector2.UnitX * (originInRect ? 1 : -1) * Math.Sign(ray.Direction.X);
            else
                normal = Vector2.UnitY * (originInRect ? 1 : -1) * Math.Sign(ray.Direction.Y);

            return new AABBIntersectionResult
            {
                Normal = normal,
                InsideShape = originInRect,
                DistanceFromOrigin = distance,
            };
        }
    }
}
