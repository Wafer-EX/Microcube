using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Silk.NET.Maths;

namespace Microcube.UI.Components.Containers
{
    /// <summary>
    /// A container that can set padding in the sides of the child.
    /// </summary>
    public class PaddingContainer : Container
    {
        /// <summary>
        /// Left padding of the container.
        /// </summary>
        public float PaddingLeft { get; set; }

        /// <summary>
        /// Right padding of the container.
        /// </summary>
        public float PaddingRight { get; set; }

        /// <summary>
        /// Top padding of the container.
        /// </summary>
        public float PaddingTop { get; set; }

        /// <summary>
        /// Bottom padding of the container.
        /// </summary>
        public float PaddingBottom { get; set; }

        public PaddingContainer() : base() { }

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            displayedArea = new Rectangle<float>(
                displayedArea.Origin.X + PaddingLeft,
                displayedArea.Origin.Y + PaddingTop,
                displayedArea.Size.X - PaddingLeft - PaddingRight,
                displayedArea.Size.Y - PaddingTop - PaddingBottom);

            foreach (var sprite in Child?.GetSprites(displayedArea) ?? Array.Empty<Sprite>())
                yield return sprite;
        }
    }
}