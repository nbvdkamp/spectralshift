using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace SpectralShift.Game
{
    public class MainScreen : Screen
    {
        private Obstacle o;
        private Container<SelectionBlueprint<Obstacle>> container;

        [BackgroundDependencyLoader]
        private void load()
        {
            container = new Container<SelectionBlueprint<Obstacle>>();
            o = new Obstacle();
            container.Add(new SelectionBlueprint<Obstacle>(o));

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Violet,
                    RelativeSizeAxes = Axes.Both,
                },
                container,
                new SpriteText
                {
                    Y = 20,
                    Text = "Main Screenu",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                new SpinningBox
                {
                    Anchor = Anchor.Centre,
                }
            };
        }
    }
}
