using Silk.NET.Maths;
using System.Numerics;

namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a text modifier that can move characters by waves.
    /// </summary>
    public class WaveTextModifier : ITextModifier
    {
        private float elapsedTime = 0.0f;

        /// <summary>
        /// Strength of the modifier, in both coorditanes.
        /// </summary>
        public Vector2 Strength { get; set; }

        /// <summary>
        /// Wave size (frequency) of the modifier, in both coordinates.
        /// </summary>
        public Vector2 WaveSize { get; set; }

        /// <summary>
        /// Wave speed of the modifier, in both coordinates.
        /// </summary>
        public Vector2 Speed { get; set; }

        public WaveTextModifier(Vector2 strength, Vector2 waveSize, Vector2 speed)
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