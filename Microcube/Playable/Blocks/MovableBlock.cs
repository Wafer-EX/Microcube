using Microcube.Graphics.ColorModels;
using Microcube.Playable.Blocks.Moving;
using System.Numerics;

namespace Microcube.Playable.Blocks
{
    /// <summary>
    /// Represents a block that can be moved by move queue.
    /// </summary>
    public abstract class MovableBlock(Vector3 position, RgbaColor color, MoveQueue? moveQueue = null) : Block(position, color), IDynamic
    {
        private Vector3 _position;
        private Vector3 _offsettedPosition;
        private bool _attachPlayer = false;

        public override Vector3 Position
        {
            get => MoveQueue != null ? _offsettedPosition : _position;
            set => _position = value;
        }

        /// <summary>
        /// Move queue of the block that will move this block.
        /// </summary>
        public MoveQueue? MoveQueue { get; private set; } = moveQueue;

        public virtual void Update(float deltaTime, Level level)
        {
            ArgumentNullException.ThrowIfNull(level, nameof(level));
            if (MoveQueue != null)
            {
                _attachPlayer = level.Player.Position == _offsettedPosition + new Vector3(0, 1.0f, 0);
                _offsettedPosition = _position + MoveQueue.Offset;

                ModelMatrix = Matrix4x4.CreateTranslation(_offsettedPosition);

                // TODO: something is wrong with player attaching
                if (MoveQueue.IsMoving)
                {
                    float distance = Vector3.Distance(_offsettedPosition, level.Player.Position);
                    float nextPositionDistance = Vector3.Distance(_offsettedPosition, level.Player.NextPosition);

                    if (distance < 1.0f || nextPositionDistance < 1.0f || _attachPlayer)
                        // TODO: calculate real push offset
                        level.Player.Push(MoveQueue.FrameOffset);
                }
                else if (_attachPlayer)
                {
                    level.Player.Position = new Vector3
                    {
                        X = MathF.Round(level.Player.Position.X),
                        Y = MathF.Round(level.Player.Position.Y),
                        Z = MathF.Round(level.Player.Position.Z),
                    };
                }
            }
        }
    }
}