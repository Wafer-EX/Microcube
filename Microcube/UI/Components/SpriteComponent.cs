using Microcube.Graphics.Raster;
using Silk.NET.Maths;

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

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            if (IsFitToDisplayedArea)
            {
                Sprite fittedSprite = Sprite;

                float widthDifference = displayedArea.Size.X / fittedSprite.ViewportArea.Size.X;
                float heightDifference = displayedArea.Size.Y / fittedSprite.ViewportArea.Size.Y;
                float sizeDifference = MathF.Min(widthDifference, heightDifference);

                fittedSprite.Scale = sizeDifference;

                fittedSprite.ViewportArea = new Rectangle<float>(
                    displayedArea.Origin - fittedSprite.ViewportArea.Size / 2.0f + displayedArea.Size / 2.0f,
                    fittedSprite.ViewportArea.Size);

                yield return fittedSprite;
            }
            else
            {
                Sprite alignedSprite = Sprite;

                // TODO: align sprite
                alignedSprite.ViewportArea = new Rectangle<float>(
                    displayedArea.Origin + displayedArea.Size / 2.0f - alignedSprite.ViewportArea.Size / 2.0f,
                    alignedSprite.ViewportArea.Size);

                yield return alignedSprite;
            }
        }
    }
}