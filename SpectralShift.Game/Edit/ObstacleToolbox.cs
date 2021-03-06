// Adapted from https://github.com/ppy/osu/blob/master/osu.Game/Skinning/Editor/SkinComponentToolbox.cs

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENSE.OSU.txt file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osuTK;
using osuTK.Graphics;

namespace SpectralShift.Game.Edit
{
    public class ObstacleToolbox : ToolboxGroup
    {
        public Action<Type> RequestPlacement;

        private const float component_display_scale = 0.8f;

        public ObstacleToolbox()
            : base("Obstacles")
        {
            RelativeSizeAxes = Axes.None;
            Width = 200;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            FillFlowContainer fill;

            Child = fill = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(20)
            };

            var obstacleTypes = new[]
            {
                typeof(Rectangle),
                typeof(Circle),
            };

            foreach (var type in obstacleTypes)
            {
                var component = attemptAddComponent(type);

                if (component != null)
                {
                    component.RequestPlacement = t => RequestPlacement?.Invoke(t);
                    fill.Add(component);
                }
            }
        }

        private static ToolboxComponentButton attemptAddComponent(Type type)
        {
            try
            {
                var instance = (Drawable)Activator.CreateInstance(type);

                Debug.Assert(instance != null);

                return new ToolboxComponentButton(instance);
            }
            catch
            {
                return null;
            }
        }

        private class ToolboxComponentButton : OsuButton
        {
            protected override bool ShouldBeConsideredForInput(Drawable child) => false;

            public override bool PropagateNonPositionalInputSubTree => false;

            private readonly Drawable component;

            public Action<Type> RequestPlacement;

            private Container innerContainer;

            public ToolboxComponentButton(Drawable component)
            {
                this.component = component;

                Enabled.Value = true;

                RelativeSizeAxes = Axes.X;
                Height = 70;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                BackgroundColour = colours.Gray3;
                Content.EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 2,
                    Offset = new Vector2(0, 1),
                    Colour = Color4.Black.Opacity(0.5f)
                };

                AddRange(new Drawable[]
                {
                    new OsuSpriteText
                    {
                        Text = component.GetType().Name,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                    innerContainer = new Container
                    {
                        Y = 10,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(component_display_scale),
                        Masking = true,
                        Child = component
                    }
                });

                // adjust provided component to fit / display in a known state.
                component.Anchor = Anchor.Centre;
                component.Origin = Anchor.Centre;
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                if (component.RelativeSizeAxes != Axes.None)
                {
                    innerContainer.AutoSizeAxes = Axes.None;
                    innerContainer.Height = 100;
                }
            }

            protected override bool OnClick(ClickEvent e)
            {
                RequestPlacement?.Invoke(component.GetType());
                return true;
            }
        }
    }
}
