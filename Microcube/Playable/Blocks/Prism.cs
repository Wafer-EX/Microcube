using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Playable.Blocks
{
    /// <summary>
    /// Represents a prism that can be collected by player
    /// </summary>
    public class Prism(Vector3 position) : Block(position, new RgbaColor(1.0f, 0.0f, 0.0f, 1.0f)), IDynamic
    {
        private float _elapsedTime = 0.0f;

        /// <summary>
        /// Shows is the prism was collected.
        /// </summary>
        public bool IsCollected { get; private set; }

        public void Update(float deltaTime, Level level)
        {
            Color = (RgbaColor)((HsvaColor)Color).OffsetHue(420.0f * deltaTime);

            if (!IsCollected)
            {
                if (Vector3.Distance(level.Player.OffsettedPosition, Position) < 0.75f)
                {
                    IsCollected = true;
                    IsRender = false;
                    level.CollectPrism();
                }
                else
                {
                    _elapsedTime += deltaTime;

                    ModelMatrix = Matrix4x4.CreateScale(0.25f)
                        * Matrix4x4.CreateRotationY(_elapsedTime * 2.0f)
                        * Matrix4x4.CreateTranslation(0.0f, MathF.Sin(_elapsedTime * 2.0f) / 5.0f, 0.0f)
                        * Matrix4x4.CreateTranslation(Position);
                }
            }
        }
    }
}