using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents a prism that can be collected by player
    /// </summary>
    public class Prism : Block, IDynamic
    {
        private float elapsedTime = 0.0f;

        /// <summary>
        /// Shows is the prism was collected.
        /// </summary>
        public bool IsCollected { get; private set; }

        public Prism(Vector3D<float> position) : base(position, new RgbaColor(1.0f, 0.0f, 0.0f, 1.0f)) { }

        public void Update(float deltaTime, Level level)
        {
            Color = (RgbaColor)((HsvaColor)Color).OffsetHue(420.0f * deltaTime);

            if (!IsCollected)
            {
                if (Vector3D.Distance(level.Player.OffsettedPosition, Position) < 0.75f)
                {
                    IsCollected = true;
                    IsRender = false;
                    level.CollectPrism();
                }
                else
                {
                    elapsedTime += deltaTime;

                    ModelMatrix = Matrix4X4.CreateScale(0.25f)
                        * Matrix4X4.CreateRotationY(elapsedTime * 2.0f)
                        * Matrix4X4.CreateTranslation(0.0f, MathF.Sin(elapsedTime * 2.0f) / 5.0f, 0.0f)
                        * Matrix4X4.CreateTranslation(Position);
                }
            }
        }
    }
}