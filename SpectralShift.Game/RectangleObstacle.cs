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

            // Axis aligned box ray intersection centered at the origin
            Vector2 corner = new Vector2(Scale.X * Width / 2, Scale.Y * Height / 2);
            Vector2 min = -corner;
            Vector2 max = corner;
            Vector2 inverseDirection = new Vector2(1 / rotatedRay.Direction.X, 1 / rotatedRay.Direction.Y);

            Vector2 t0 = Util.ElementwiseMultiply(min - rotatedRay.Origin, inverseDirection);
            Vector2 t1 = Util.ElementwiseMultiply(max - rotatedRay.Origin, inverseDirection);
            Vector2 tmin = Util.ElementwiseMin(t0, t1);
            Vector2 tmax = Util.ElementwiseMax(t0, t1);

            bool behindRay = Util.MinElement(tmax) < 0;

            if (behindRay || Util.MaxElement(tmin) > Util.MinElement(tmax))
                return null;

            bool originInRect = Math.Abs(rotatedRay.Origin.X) < Math.Abs(corner.X) &&
                                Math.Abs(rotatedRay.Origin.Y) < Math.Abs(corner.Y);
            Vector2 normal;

            if (Util.MaxElement(tmin) == tmin.X)
                normal = Vector2.UnitX * -Math.Sign(rotatedRay.Direction.X);
            else
                normal = Vector2.UnitY * -Math.Sign(rotatedRay.Direction.Y);

            normal = Util.Multiply(inverseRotation, normal);

            Vector2 position = ray.Origin + Util.MaxElement(tmin) * ray.Direction;

            return new IntersectionResult
            {
                Position = position,
                Normal = normal,
                InsideShape = originInRect,
                Material = Material,
                IndexOfRefraction = IndexOfRefraction,
            };
        }
    }
}
