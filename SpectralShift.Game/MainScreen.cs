using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Configuration;
using osu.Game.Graphics;
using SpectralShift.Game.Edit;

namespace SpectralShift.Game
{
    public class MainScreen : Screen
    {
        [Cached]
        protected readonly OsuColour OsuColour = new OsuColour();

        [Cached]
        protected readonly SessionStatics Statistics = new SessionStatics();

        private readonly ObstacleContainer container = new ObstacleContainer();

        private ObstacleEditor editor;

        [BackgroundDependencyLoader]
        private void load()
        {
            editor = new ObstacleEditor(container);

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = OsuColour.Gray2,
                    RelativeSizeAxes = Axes.Both,
                },
                container,
                editor,
            };
        }
    }
}
