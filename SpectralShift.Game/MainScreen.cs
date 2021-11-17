using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osuTK;
using osuTK.Graphics;
using SpectralShift.Game.Edit;

namespace SpectralShift.Game
{
    public class MainScreen : Screen
    {
        [Cached]
        protected readonly OsuColour OsuColour = new OsuColour();

        [Cached]
        protected readonly SessionStatics Statistics = new SessionStatics();

        private ObstacleContainer obstacleContainer;

        private Container rayContainer;

        private readonly List<Path> paths = new List<Path>();

        private ObstacleEditor editor;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = OsuColour.Gray2,
                    RelativeSizeAxes = Axes.Both,
                },
                obstacleContainer = new ObstacleContainer(),
                rayContainer = new Container(),
                editor = new ObstacleEditor(obstacleContainer),
            };
        }

        protected override void Update()
        {
            base.Update();

            foreach (var path in paths)
                path.ClearVertices();

            int requiredPaths = 4;

            while (paths.Count < requiredPaths)
            {
                Path path = new SmoothPath();
                path.PathRadius = 3;
                paths.Add(path);
                rayContainer.Add(path);
            }

            Vector2 cameraPos = new Vector2(500, 500);
            Vector2 cameraDir = Vector2.One.Normalized();
            Ray ray = new Ray(cameraPos, cameraDir);

            for (int i = 0; i < 4; i++)
            {
                IntersectionResult? result = findNearestIntersection(ray);

                paths[i].AddVertex(ray.Origin);

                if (i % 2 == 1)
                    paths[i].Colour = Color4.Red;

                if (result.HasValue)
                {
                    Vector2 normal = (result.Value.InsideShape ? -1 : 1) * result.Value.Normal;
                    float iorRatio = (result.Value.InsideShape ? 1 / result.Value.IndexOfRefraction : result.Value.IndexOfRefraction);

                    // Nudge new position slightly away from the circle to avoid immediate intersection
                    ray.Origin = result.Value.Position - 0.01f * normal;
                    ray.Direction = refract(ray.Direction, normal, iorRatio);

                    paths[i].AddVertex(result.Value.Position);
                }
                else
                {
                    paths[i].AddVertex(ray.Origin + ray.Direction * 10000);
                    break;
                }
            }
        }

        private IntersectionResult? findNearestIntersection(Ray ray)
        {
            float distance = float.MaxValue;
            IntersectionResult? result = null;

            foreach (Obstacle obstacle in obstacleContainer.Components)
            {
                var intersection = obstacle.Intersects(ray);
                float d;

                if (intersection.HasValue && (d = (ray.Origin - intersection.Value.Position).Length) < distance)
                {
                    distance = d;
                    result = intersection.Value;
                }
            }

            return result;
        }

        private Vector2 refract(Vector2 incident, Vector2 normal, float eta)
        {
            float iDotN = Vector2.Dot(incident, normal);
            float k = 1.0f - eta * eta * (1.0f - iDotN * iDotN);

            if (k < 0)
                return Vector2.Zero;

            return eta * incident - (eta * iDotN + (float)Math.Sqrt(k)) * normal;
        }
    }
}
