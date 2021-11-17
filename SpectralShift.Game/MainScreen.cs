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

            int requiredPaths = 1;

            while (paths.Count < requiredPaths)
            {
                Path path = new SmoothPath();
                path.PathRadius = 3;
                paths.Add(path);
                rayContainer.Add(path);
            }

            Vector2 cameraPos = new Vector2(500, 500);
            Vector2 cameraDir = Vector2.UnitX;
            Ray ray = new Ray(cameraPos, cameraDir);

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

            paths[0].AddVertex(ray.Origin);

            if (result.HasValue)
                paths[0].AddVertex(result.Value.Position);
            else
                paths[0].AddVertex(ray.Origin + ray.Direction * 10000);
        }
    }
}
