// Adapted from https://github.com/ppy/osu/blob/master/osu.Game/Skinning/Editor/SkinBlueprint.cs

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osuTK;

namespace SpectralShift.Game.Edit
{
    public class ObstacleBlueprint : SelectionBlueprint<Obstacle>
    {
        private Drawable drawable => Item;

        protected override bool ShouldBeAlive => drawable.IsAlive && Item.IsPresent;

        [Resolved]
        private OsuColour colours { get; set; }

        public ObstacleBlueprint(Obstacle obstacle)
            : base(obstacle)
        {
        }

        protected override void OnSelected()
        {
            // base logic hides selected blueprints when not selected, but object blueprints don't do that.
        }

        protected override void OnDeselected()
        {
            // base logic hides selected blueprints when not selected, but object blueprints don't do that.
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => drawable.ReceivePositionalInputAt(screenSpacePos);

        public override Vector2 ScreenSpaceSelectionPoint => drawable.ToScreenSpace(drawable.OriginPosition);

        public override Quad SelectionQuad => drawable.ScreenSpaceDrawQuad;
    }
}
