using Silk.NET.Maths;

namespace Microcube.Graphics.Raster.TextModifiers
{
    public class WaveTextModifier : ITextModifier
    {
        private float elapsedTime = 0.0f;

        public Vector2D<float> Strength { get; set; }

        public Vector2D<float> WaveSize { get; set; }

        public Vector2D<float> Speed { get; set; }

        public WaveTextModifier(Vector2D<float> strength, Vector2D<float> waveSize, Vector2D<float> speed)
        {
            Strength = strength;
            WaveSize = waveSize;
            Speed = speed;
        }

        public Sprite ModifyCharacter(Sprite sprite, int index)
        {
            var offset = new Vector2D<float>(
                MathF.Sin((elapsedTime * Speed.X) + (index / WaveSize.X)) * Strength.X,
                MathF.Sin((elapsedTime * Speed.Y) + (index / WaveSize.Y)) * Strength.Y);

            sprite.ViewportArea = new Rectangle<float>(sprite.ViewportArea.Origin + offset, sprite.ViewportArea.Size);
            return sprite;
        }

        public void Update(float deltaTime) => elapsedTime += deltaTime;
    }
}