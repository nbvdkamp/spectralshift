using osuTK.Graphics;

namespace SpectralShift.Game
{
    public enum Material
    {
        Diffuse,
        Reflective,
        Refractive,
    }

    public static class MaterialExtensions
    {
        public static Color4 GetColor(this Material material)
        {
            switch (material)
            {
                case Material.Diffuse:
                    return Color4.SeaGreen;

                case Material.Reflective:
                    return Color4.Aqua;

                case Material.Refractive:
                    return Color4.Sienna;
            }

            return Color4.Magenta;
        }
    }
}
