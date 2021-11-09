using osu.Framework.Platform;
using osu.Framework;
using SpectralShift.Game;

namespace SpectralShift.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"SpectralShift"))
            using (osu.Framework.Game game = new SpectralShiftGame())
                host.Run(game);
        }
    }
}
