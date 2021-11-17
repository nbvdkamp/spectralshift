using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace SpectralShift.Game
{
    public class CircleObstacle : Obstacle
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChild = Shape = new Circle
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
            float radius = Scale.Y * Width / 2;

            Vector2 u = center - ray.Origin;
            Vector2 u1 = Vector2.Dot(u, ray.Direction) * ray.Direction;
            Vector2 u2 = u - u1;
            float centerDistance = u2.Length;

            if (centerDistance > radius)
                return null;

            float m = (float)Math.Sqrt(radius * radius - centerDistance * centerDistance);
            Vector2 position = ray.Origin + u1 - m * ray.Direction;

            return new IntersectionResult
            {
                Position = position,
                Normal = (center - position).Normalized(),
            };
        }
    }
}
