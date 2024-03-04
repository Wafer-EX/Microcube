using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents a button on the ground that can be pressed by player and start move queue.
    /// </summary>
    public class TriggerButton : Block, IDynamic
    {
        private readonly MoveQueue _moveQueue;

        /// <summary>
        /// Is the button pressed at this moment or not.
        /// </summary>
        public bool IsPressed { get; private set; }

        // TODO: add second constructor with other trigger button? (to unpress)
        public TriggerButton(Vector3 position, RgbaColor color, MoveQueue moveQueue) : base(position, color)
        {
            ArgumentNullException.ThrowIfNull(moveQueue, nameof(moveQueue));
            _moveQueue = moveQueue;

            ModelMatrix = Matrix4x4.CreateScale(0.55f, 0.1f, 0.55f)
                * Matrix4x4.CreateTranslation(Position.X, Position.Y - 0.4f, Position.Z);
        }

        public void Update(float deltaTime, Level level)
        {
            if (!IsPressed)
            {
                if (Vector3.Distance(level.Player.Position, Position) < 1.0f)
                {
                    _moveQueue.IsActive = true;

                    IsPressed = true;
                    ModelMatrix = Matrix4x4.CreateScale(0.55f, 0.01f, 0.55f)
                        * Matrix4x4.CreateTranslation(Position.X, Position.Y - 0.49f, Position.Z);
                }
                // TODO: add unpress after time?
            }
        }
    }
}