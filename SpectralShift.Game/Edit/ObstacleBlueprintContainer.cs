// Adapted from https://github.com/ppy/osu/blob/master/osu.Game/Skinning/Editor/SkinBlueprintContainer.cs

// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENSE.OSU.txt file in the repository root for full licence text.

using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Screens.Edit.Compose.Components;

namespace SpectralShift.Game.Edit
{
    public class ObstacleBlueprintContainer : BlueprintContainer<Obstacle>
    {
        private readonly BindableList<Obstacle> targets;

        public ObstacleBlueprintContainer(ObstacleContainer obstacles)
        {
            targets = new BindableList<Obstacle> { BindTarget = obstacles.Components };
        }

        [BackgroundDependencyLoader(true)]
        private void load(ObstacleEditor editor)
        {
            SelectedItems.BindTo(editor.SelectedComponents);
        }

        protected override void LoadComplete()
        {
            targets.BindCollectionChanged(componentsChanged, true);
        }

        private void componentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.Cast<Obstacle>())
                        AddBlueprintFor(item);
                    break;

                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in e.OldItems.Cast<Obstacle>())
                        RemoveBlueprintFor(item);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.OldItems.Cast<Obstacle>())
                        RemoveBlueprintFor(item);

                    foreach (var item in e.NewItems.Cast<Obstacle>())
                        AddBlueprintFor(item);
                    break;
            }
        }

        protected override SelectionHandler<Obstacle> CreateSelectionHandler() => new ObstacleSelectionHandler();

        protected override SelectionBlueprint<Obstacle> CreateBlueprintFor(Obstacle obstacle)
            => new ObstacleBlueprint(obstacle);
    }
}
