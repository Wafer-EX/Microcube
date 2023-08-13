using Microcube.Graphics.ColorModels;

namespace Microcube.Graphics.Raster.TextModifiers
{
    public class ColorTextModifier : ITextModifier
    {
        public RgbaColor Color { get; set; }

        public float OffsetSpeed { get; set; }

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