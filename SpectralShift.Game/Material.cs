using osu.Framework.Graphics;

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
        public static Colour4 GetColor(this Material material)
        {
            switch (material)
            {
                case Material.Diffuse:
                    return Colour4.FromHex("#F4B183");

                case Material.Reflective:
                    return Colour4.FromHex("#F4B183");

                case Material.Refractive:
                    return Colour4.FromHex("#E3E3E3");
            }

            return Colour4.Magenta;
        }
    }
}
