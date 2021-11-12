// Adapted from https://github.com/ppy/osu/blob/master/osu.Game/Skinning/Editor/SkinEditor.cs

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Cursor;

namespace SpectralShift.Game.Edit
{
    [Cached(typeof(ObstacleEditor))]
    public class ObstacleEditor : VisibilityContainer
    {
        public const double TRANSITION_DURATION = 500;

        public readonly BindableList<Obstacle> SelectedComponents = new BindableList<Obstacle>();

        protected override bool StartHidden => true;

        private readonly ObstacleContainer targetContainer;

        private OsuTextFlowContainer headerText;

        [Resolved]
        private OsuColour colours { get; set; }

        public ObstacleEditor(ObstacleContainer targetContainer)
        {
            this.targetContainer = targetContainer;

            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new OsuContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    headerText = new OsuTextFlowContainer
                    {
                        TextAnchor = Anchor.TopCentre,
                        Padding = new MarginPadding(20),
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        RelativeSizeAxes = Axes.X
                    },
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize),
                            new Dimension()
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new ObstacleToolbox(700)
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    RequestPlacement = placeComponent
                                },
                                new ObstacleBlueprintContainer(targetContainer),
                            }
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Show();
        }

        private void placeComponent(Type type)
        {
            if (!(Activator.CreateInstance(type) is Obstacle component))
                throw new InvalidOperationException($"Attempted to instantiate a component for placement which was not an {typeof(Obstacle)}.");

            // give newly added components a sane starting location.
            component.Origin = Anchor.TopCentre;
            component.Anchor = Anchor.TopCentre;
            component.Position = targetContainer.DrawSize / 2;

            targetContainer.Add(component);

            SelectedComponents.Clear();
            SelectedComponents.Add(component);
        }

        protected override bool OnHover(HoverEvent e) => true;

        protected override bool OnMouseDown(MouseDownEvent e) => true;

        protected override void PopIn()
        {
            this.FadeIn(TRANSITION_DURATION, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(TRANSITION_DURATION, Easing.OutQuint);
        }

        public void DeleteItems(Obstacle[] items)
        {
            foreach (Obstacle item in items)
                targetContainer.Remove(item);
        }
    }
}
