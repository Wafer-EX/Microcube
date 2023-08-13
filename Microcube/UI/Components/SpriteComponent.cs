using Microcube.Graphics.Raster;
using Silk.NET.Maths;

namespace Microcube.UI.Components
{
    public class SpriteComponent : Component
    {
        public required Sprite Sprite { get; set; }

        public bool Fit { get; set; }

        public SpriteComponent() : base() => Fit = true;

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            if (Fit)
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