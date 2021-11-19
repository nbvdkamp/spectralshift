using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace SpectralShift.Game
{
    public struct IntersectionResult
    {
        public Vector2 Position;
        public Vector2 Normal;
        public bool InsideShape;
        public Material Material;
        public float IndexOfRefraction;

        public Ray RefractedRay(Vector2 incident, float wavelength)
        {
            Vector2 normal = (InsideShape ? -1 : 1) * Normal;
            float iorRatio = (InsideShape ? 1 / IndexOfRefraction : IndexOfRefraction);

            // Nudge new position slightly away from the circle to avoid immediate intersection
            Vector2 origin = Position - 0.01f * normal;
            Vector2 direction = Util.Refract(incident, normal, iorRatio);

            return new Ray(origin, direction);
        }

        public Ray ReflectedRay(Vector2 incident)
        {
            Vector2 normal = (InsideShape ? -1 : 1) * Normal;

            // Nudge new position slightly away from the circle to avoid immediate intersection
            Vector2 origin = Position + 0.1f * normal;
            Vector2 direction = Util.Reflect(incident, normal);

            return new Ray(origin, direction);
        }
    }

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

        public float IndexOfRefraction { get; set; }

        public void Setup()
        {
            Shape.Size = new Vector2(50, 50);
            Material = Material.Diffuse;
            IndexOfRefraction = 1.2f;
        }

        public abstract IntersectionResult? Intersects(Ray ray);
    }
}
