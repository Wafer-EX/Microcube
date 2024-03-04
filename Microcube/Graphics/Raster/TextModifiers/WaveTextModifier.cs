using System.Drawing;
using System.Numerics;

namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a text modifier that can move characters by waves.
    /// </summary>
    public class WaveTextModifier(Vector2 strength, Vector2 waveSize, Vector2 speed) : ITextModifier
    {
        private float _elapsedTime = 0.0f;

        /// <summary>
        /// Strength of the modifier, in both coorditanes.
        /// </summary>
        public Vector2 Strength { get; set; } = strength;

        /// <summary>
        /// Wave size (frequency) of the modifier, in both coordinates.
        /// </summary>
        public Vector2 WaveSize { get; set; } = waveSize;

        /// <summary>
        /// Wave speed of the modifier, in both coordinates.
        /// </summary>
        public Vector2 Speed { get; set; } = speed;

        public Sprite ModifyCharacter(Sprite sprite, int index)
        {
            Vector2 offset = new(
                MathF.Sin((_elapsedTime * Speed.X) + (index / WaveSize.X)) * Strength.X,
                MathF.Sin((_elapsedTime * Speed.Y) + (index / WaveSize.Y)) * Strength.Y);

            Vector2 offsettedPosition = sprite.ViewportArea.Location.ToVector2() + offset;

            sprite.ViewportArea = new RectangleF(new PointF(offsettedPosition), sprite.ViewportArea.Size);
            return sprite;
        }

        public void Update(float deltaTime) => _elapsedTime += deltaTime;
    }
}