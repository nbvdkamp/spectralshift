using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace SpectralShift.Game.Edit
{
    public class ObstacleContainer : CompositeDrawable
    {
        private readonly Container<Obstacle> content = new Container<Obstacle>();

        public IBindableList<Obstacle> Components => components;

        private readonly BindableList<Obstacle> components = new BindableList<Obstacle>();

        public ObstacleContainer()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader(true)]
        private void load()
        {
            LoadComponentAsync(content, wrapper =>
            {
                AddInternal(wrapper);
                components.AddRange(wrapper.Children);
            });
        }

        public void Add(Obstacle obstacle)
        {
            content.Add(obstacle);
            components.Add(obstacle);
        }

        public void Remove(Obstacle obstacle)
        {
            content.Remove(obstacle);
            components.Remove(obstacle);
        }
    }
}
