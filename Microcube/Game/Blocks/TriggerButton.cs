using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game.Blocks
{
    public class TriggerButton : Block, IDynamic
    {
        private readonly MoveQueue moveQueue;

        public bool IsPressed { get; private set; }

        // TODO: add second constructor with other trigger button? (to unpress)
        public TriggerButton(Vector3D<float> position, RgbaColor color, MoveQueue moveQueue) : base(position, color)
        {
            ArgumentNullException.ThrowIfNull(moveQueue, nameof(moveQueue));
            this.moveQueue = moveQueue;

            ModelMatrix = Matrix4X4.CreateScale(0.55f, 0.1f, 0.55f)
                * Matrix4X4.CreateTranslation(Position.X, Position.Y - 0.4f, Position.Z);
        }

        public void Update(float deltaTime, Level level)
        {
            if (!IsPressed)
            {
                if (Vector3D.Distance(level.Player.Position, Position) < 1.0f)
                {
                    moveQueue.IsActive = true;

                    IsPressed = true;
                    ModelMatrix = Matrix4X4.CreateScale(0.55f, 0.01f, 0.55f)
                        * Matrix4X4.CreateTranslation(Position.X, Position.Y - 0.49f, Position.Z);
                }
                // TODO: add unpress after time?
            }
        }
    }
}