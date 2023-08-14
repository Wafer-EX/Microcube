using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Silk.NET.Maths;

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
        public required Vector2D<float> Size { get; set; }

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

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            float width = MathF.Min(Size.X, displayedArea.Size.X);
            float height = MathF.Min(Size.Y, displayedArea.Size.Y);

            float offsetX = HorizontalAlignment switch
            {
                HorizontalAlignment.Left => 0.0f,
                HorizontalAlignment.Center => displayedArea.Size.X / 2.0f - width / 2.0f,
                HorizontalAlignment.Right => displayedArea.Size.X - width,
                _ => throw new NotImplementedException()
            };

            float offsetY = VerticalAlignment switch
            {
                VerticalAlignment.Top => 0.0f,
                VerticalAlignment.Middle => displayedArea.Size.Y / 2.0f - height / 2.0f,
                VerticalAlignment.Bottom => displayedArea.Size.Y - height,
                _ => throw new NotImplementedException()
            };

            displayedArea = new Rectangle<float>()
            {
                Origin = displayedArea.Origin + new Vector2D<float>(offsetX, offsetY),
                Size = new Vector2D<float>(width, height),
            };

            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            foreach (var sprite in Child?.GetSprites(displayedArea) ?? Array.Empty<Sprite>())
                yield return sprite;
        }
    }
}