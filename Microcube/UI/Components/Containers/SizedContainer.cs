using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Silk.NET.Maths;
using System.Drawing;
using System.Numerics;

namespace Microcube.UI.Components.Containers
{
    /// <summary>
    /// Represents an container where the child is with specific size that can be aligned.
    /// </summary>
    public class SizedContainer : Container
    {
        /// <summary>
        /// Size of the container.
        /// </summary>
        public required Vector2 Size { get; set; }

        /// <summary>
        /// Horizontal alignment of the sized area.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Vertical alignment of the sized area.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        public SizedContainer() : base()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            float width = MathF.Min(Size.X, displayedArea.Width);
            float height = MathF.Min(Size.Y, displayedArea.Height);

            float offsetX = HorizontalAlignment switch
            {
                HorizontalAlignment.Left => 0.0f,
                HorizontalAlignment.Center => displayedArea.Width / 2.0f - width / 2.0f,
                HorizontalAlignment.Right => displayedArea.Width - width,
                _ => throw new NotImplementedException()
            };

            float offsetY = VerticalAlignment switch
            {
                VerticalAlignment.Top => 0.0f,
                VerticalAlignment.Middle => displayedArea.Height / 2.0f - height / 2.0f,
                VerticalAlignment.Bottom => displayedArea.Height - height,
                _ => throw new NotImplementedException()
            };

            displayedArea = new RectangleF()
            {
                Location = new PointF(displayedArea.Location.ToVector2() + new Vector2(offsetX, offsetY)),
                Size = new SizeF(width, height),
            };

            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            foreach (var sprite in Child?.GetSprites(displayedArea) ?? [])
                yield return sprite;
        }
    }
}