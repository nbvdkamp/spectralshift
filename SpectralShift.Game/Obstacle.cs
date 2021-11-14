using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace SpectralShift.Game
{
    public abstract class Obstacle : CompositeDrawable
    {
        protected Drawable Shape;

        private Material material;

        public Material Material
        {
            get => material;
            set
            {
                material = value;

                Shape.Colour = material.GetColor();
            }
        }

        public void Setup()
        {
            Shape.Size = new Vector2(50, 50);
            Material = Material.Diffuse;
        }
    }
}
