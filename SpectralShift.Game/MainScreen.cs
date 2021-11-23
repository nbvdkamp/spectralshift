using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osuTK;
using osuTK.Graphics;
using SpectralShift.Game.Edit;
using SpectralShift.Game.Graphics;

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

            Vector2 cameraPos = new Vector2(500, 500);
            Vector2 cameraDir = Vector2.One.Normalized();
            Ray[] rays =
            {
                new Ray(cameraPos, cameraDir),
                new Ray(Vector2.Zero, Vector2.One),
            };
            bool[] raysActive = { true, false };
            bool split = false;
            int pathIndex = 0;

            for (int depth = 0; depth < 4; depth++)
            {
                for (int i = 0; i < (split ? 2 : 1); i++)
                {
                    if (!(raysActive[0] || raysActive[1]))
                        break;

                    if (!raysActive[i])
                        continue;

                    IntersectionResult? result = findNearestIntersection(rays[i]);

                    if (paths.Count - 1 < pathIndex)
                        addPath();

                    paths[pathIndex].AddVertex(rays[i].Origin);

                    if (split)
                        paths[pathIndex].Colour = (i == 0 ? Color4.Blue : Color4.Green);
                    else
                        paths[pathIndex].Colour = Color4.White;

                    if (result.HasValue)
                    {
                        switch (result.Value.Material)
                        {
                            case Material.Diffuse:
                                rays[i] = result.Value.ReflectedRay(rays[i].Direction);
                                break;

                            case Material.Reflective:
                                rays[i] = result.Value.ReflectedRay(rays[i].Direction);
                                break;

                            case Material.Refractive:
                                if (!split)
                                {
                                    split = true;
                                    raysActive[1] = true;
                                    rays[0] = result.Value.RefractedRay(rays[0].Direction, 300);
                                    rays[1] = result.Value.RefractedRay(rays[0].Direction, 600);
                                    // Prevent second ray getting an extra bounce
                                    i = 2;
                                }
                                else
                                {
                                    rays[i] = result.Value.RefractedRay(rays[i].Direction, (1 + i) * 300);
                                }

                                break;
                        }

                        paths[pathIndex].AddVertex(result.Value.Position);
                    }
                    else
                    {
                        paths[pathIndex].AddVertex(rays[i].Origin + rays[i].Direction * 10000);
                        raysActive[i] = false;
                    }

                    pathIndex++;
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

        private void addPath()
        {
            Path path = new SmoothPath();
            path.PathRadius = 3;
            paths.Add(path);
            rayContainer.Add(path);
        }
    }
}
