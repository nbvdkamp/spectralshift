using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

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
    }
}
