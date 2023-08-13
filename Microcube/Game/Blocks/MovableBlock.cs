using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game.Blocks
{
    public abstract class MovableBlock : Block, IDynamic
    {
        private Vector3D<float> position;
        private Vector3D<float> offsettedPosition;
        private bool attachPlayer = false;

        public override Vector3D<float> Position
        {
            get => MoveQueue != null ? offsettedPosition : position;
            set => position = value;
        }

        public MoveQueue? MoveQueue { get; private set; }

        public MovableBlock(Vector3D<float> position, RgbaColor color, MoveQueue? moveQueue = null) : base(position, color)
        {
            MoveQueue = moveQueue;
        }

        public virtual void Update(float deltaTime, Level level)
        {
            ArgumentNullException.ThrowIfNull(level, nameof(level));
            if (MoveQueue != null)
            {
                attachPlayer = level.Player.Position == offsettedPosition + new Vector3D<float>(0, 1.0f, 0);
                offsettedPosition = position + MoveQueue.Offset;

                ModelMatrix = Matrix4X4.CreateTranslation(offsettedPosition);

                // TODO: something is wrong with player attaching
                if (MoveQueue.IsMoving)
                {
                    float distance = Vector3D.Distance(offsettedPosition, level.Player.Position);
                    float nextPositionDistance = Vector3D.Distance(offsettedPosition, level.Player.NextPosition);

                    if (distance < 1.0f || nextPositionDistance < 1.0f || attachPlayer)
                        // TODO: calculate real push offset
                        level.Player.Push(MoveQueue.FrameOffset);
                }
                else if (attachPlayer)
                {
                    level.Player.Position = new Vector3D<float>
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