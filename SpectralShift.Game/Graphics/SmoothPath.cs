// Adapted from https://github.com/ppy/osu-framework/blob/master/osu.Framework/Graphics/Lines/SmoothPath.cs

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SpectralShift.Game.Graphics
{
    public class SmoothPath : Path
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            validateTexture();
        }

        public override float PathRadius
        {
            get => base.PathRadius;
            set
            {
                if (base.PathRadius == value)
                    return;

                base.PathRadius = value;

                InvalidateTexture();
            }
        }

        private readonly Cached textureCache = new Cached();

        protected void InvalidateTexture()
        {
            textureCache.Invalidate();
            Invalidate(Invalidation.DrawNode);
        }

        private void validateTexture()
        {
            if (textureCache.IsValid)
                return;

            int textureWidth = (int)PathRadius * 2;

            //initialise background
            var raw = new Image<Rgba32>(textureWidth, 1);

            const float aa_portion = 0.02f;

            for (int i = 0; i < textureWidth; i++)
            {
                float progress = (float)i / (textureWidth - 1);

                var colour = ColourAt(progress);
                raw[i, 0] = new Rgba32(colour.R, colour.G, colour.B, colour.A * Math.Min(progress / aa_portion, 1));
            }

            var texture = new DisposableTexture(textureWidth, 1, true);
            texture.SetData(new TextureUpload(raw));
            Texture = texture;

            textureCache.Validate();
        }

        /// <summary>
        /// Retrieves the colour from a position in the texture of the <see cref="Path"/>.
        /// </summary>
        /// <param name="position">The position within the texture. 0 indicates the outermost-point of the path, 1 indicates the centre of the path.</param>
        protected virtual Color4 ColourAt(float position) => Color4.White;
    }
}
