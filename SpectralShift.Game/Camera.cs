using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace SpectralShift.Game
{
    public class Camera : CompositeDrawable
    {
        private CameraHandle positionHandle;

        private CameraHandle rotationHandle;

        public Vector2 Direction = Vector2.UnitX;

        private const int handle_radius = 30;

        [BackgroundDependencyLoader]
        private void load()
        {
            Origin = Anchor.Centre;
            AutoSizeAxes = Axes.Both;

            InternalChildren = new[]
            {
                positionHandle = new CameraHandle
                {
                    Position = 0.5f * handle_radius * Vector2.One,
                    HandleDrag = dragPosition,
                },
                rotationHandle = new CameraHandle
                {
                    Position = new Vector2(1.5f * handle_radius, 0.5f * handle_radius),
                    Scale = new Vector2(0.5f, 0.5f),
                    HandleDrag = dragRotation,
                }
            };
        }

        public override Vector2 Size => positionHandle.Size;

        private void dragPosition(DragEvent e) => Position += e.Delta;

        private void dragRotation(DragEvent e)
        {
            Direction = e.LastMousePosition.Normalized();
            rotationHandle.Position = handle_radius * 0.5f * Vector2.One + handle_radius * Direction;
        }

        private class CameraHandle : CompositeDrawable
        {
            public Action<DragEvent> HandleDrag;

            [BackgroundDependencyLoader]
            private void load()
            {
                Origin = Anchor.Centre;
                AutoSizeAxes = Axes.Both;

                InternalChildren = new[]
                {
                    new osu.Framework.Graphics.Shapes.Circle
                    {
                        Size = handle_radius * Vector2.One,
                        Colour = Colour4.White,
                    }
                };
            }

            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
            {
                HandleDrag?.Invoke(e);
                base.OnDrag(e);
            }
        }
    }
}
