using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Silk.NET.Maths;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represents an empty component that provides to fill space by color or nothing.
    /// </summary>
    public class GlueComponent : Component
    {
        /// <summary>
        /// Background filling color
        /// </summary>
        public RgbaColor Color { get; set; }

        public GlueComponent() : base()
        {
            Color = RgbaColor.Transparent;
        }

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            if (Color == RgbaColor.Transparent)
                yield break;

            yield return new Sprite(displayedArea, Color);
        }
    }
}