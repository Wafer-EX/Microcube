using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using System.Drawing;

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

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            displayedArea = new RectangleF(
                displayedArea.X + PaddingLeft,
                displayedArea.Y + PaddingTop,
                displayedArea.Width - PaddingLeft - PaddingRight,
                displayedArea.Height - PaddingTop - PaddingBottom);

            foreach (var sprite in Child?.GetSprites(displayedArea) ?? [])
                yield return sprite;
        }
    }
}