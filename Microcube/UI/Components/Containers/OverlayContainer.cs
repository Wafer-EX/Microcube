using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Silk.NET.Maths;
using System.Drawing;

namespace Microcube.UI.Components.Containers
{
    /// <summary>
    /// A container that can display second component as overlay, but don't send input there.
    /// </summary>
    public class OverlayContainer : Container
    {
        /// <summary>
        /// External component to display.
        /// </summary>
        public required Component? OverlayComponent { get; set; }

        public OverlayContainer() : base() { }

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            foreach (Sprite sprite in Child?.GetSprites(displayedArea) ?? [])
                yield return sprite;

            foreach (Sprite sprite in OverlayComponent?.GetSprites(displayedArea) ?? [])
                yield return sprite;
        }
    }
}