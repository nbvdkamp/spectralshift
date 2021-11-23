using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace SpectralShift.Game
{
    public class Circle : Obstacle
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChild = Shape = new osu.Framework.Graphics.Shapes.Circle
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };

            Setup();
        }

        public override IntersectionResult? Intersects(Ray ray)
        {
            // https://www.bluebill.net/circle_ray_intersection.html
            Vector2 center = Position;
            float radius = Math.Abs(Scale.Y) * Width / 2;

            Vector2 u = center - ray.Origin;
            float uDotDir = Vector2.Dot(u, ray.Direction);

            Vector2 u1 = uDotDir * ray.Direction;
            Vector2 u2 = u - u1;
            float centerDistance = u2.Length;

            bool originInCircle = u.Length < radius;
            bool centerBehindRay = uDotDir < 0;

            if (centerBehindRay && !originInCircle)
                return null;

            if (centerDistance > radius)
                return null;

            float m = (float)Math.Sqrt(radius * radius - centerDistance * centerDistance);
            Vector2 position = ray.Origin + u1;

            if (m < u1.Length)
                position -= m * ray.Direction;
            else
                position += m * ray.Direction;

            return new IntersectionResult
            {
                Position = position,
                Normal = (position - center).Normalized(),
                InsideShape = originInCircle,
                Material = Material,
            };
        }
    }
}
