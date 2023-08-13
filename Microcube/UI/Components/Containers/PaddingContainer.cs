using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Silk.NET.Maths;

namespace Microcube.UI.Components.Containers
{
    public class PaddingContainer : Container
    {
        public float PaddingLeft { get; set; }

        public float PaddingRight { get; set; }

        public float PaddingTop { get; set; }

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