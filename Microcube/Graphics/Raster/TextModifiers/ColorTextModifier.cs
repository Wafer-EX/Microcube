using Microcube.Graphics.ColorModels;

namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a text modifier that can change color to the specific and apply hue to this.
    /// </summary>
    public class ColorTextModifier : ITextModifier
    {
        /// <summary>
        /// Specific color of the text.
        /// </summary>
        public RgbaColor Color { get; set; }

        /// <summary>
        /// Hue offset speed of the color.
        /// </summary>
        public float OffsetSpeed { get; set; }

        /// <summary>
        /// Hue difference between characters.
        /// </summary>
        public float OffsetPerCharacter { get; set; }

        public ColorTextModifier(RgbaColor color, float offsetSpeed, float offsetPerCharacter)
        {
            Color = color;
            OffsetSpeed = offsetSpeed;
            OffsetPerCharacter = offsetPerCharacter;
        }

        public Sprite ModifyCharacter(Sprite sprite, int index)
        {
            sprite.Color = Color.OffsetHue(OffsetPerCharacter * index);
            return sprite;
        }

        public void Update(float deltaTime)
        {
            if (OffsetSpeed != 0.0f)
                Color = Color.OffsetHue(OffsetSpeed * deltaTime);
        }
    }
}