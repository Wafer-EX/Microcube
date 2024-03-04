using Microcube.Graphics.Raster;
using Silk.NET.Maths;
using System.Drawing;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represents component that shows a sprite inside displayed area
    /// </summary>
    public class SpriteComponent : Component
    {
        /// <summary>
        /// Sprite that will be displayed inside this component.
        /// </summary>
        public required Sprite Sprite { get; set; }

        /// <summary>
        /// Flag that enables sprite fitting to displayed area. Is true by default.
        /// </summary>
        public bool IsFitToDisplayedArea { get; set; }

        public SpriteComponent() : base() => IsFitToDisplayedArea = true;

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (IsFitToDisplayedArea)
            {
                Sprite fittedSprite = Sprite;

                float widthDifference = displayedArea.Width / fittedSprite.ViewportArea.Width;
                float heightDifference = displayedArea.Height / fittedSprite.ViewportArea.Height;
                float sizeDifference = MathF.Min(widthDifference, heightDifference);

                fittedSprite.Scale = sizeDifference;

                fittedSprite.ViewportArea = new RectangleF(
                    displayedArea.Location - fittedSprite.ViewportArea.Size / 2.0f + displayedArea.Size / 2.0f,
                    fittedSprite.ViewportArea.Size);

                yield return fittedSprite;
            }
            else
            {
                Sprite alignedSprite = Sprite;

                // TODO: align sprite
                alignedSprite.ViewportArea = new RectangleF(
                    displayedArea.Location + displayedArea.Size / 2.0f - alignedSprite.ViewportArea.Size / 2.0f,
                    alignedSprite.ViewportArea.Size);

                yield return alignedSprite;
            }
        }
    }
}