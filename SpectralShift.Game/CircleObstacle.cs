using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

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
    }
}
