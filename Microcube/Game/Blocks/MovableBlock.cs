﻿using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents a block that can be moved by move queue.
    /// </summary>
    public abstract class MovableBlock : Block, IDynamic
    {
        private Vector3 position;
        private Vector3 offsettedPosition;
        private bool attachPlayer = false;

        public override Vector3 Position
        {
            get => MoveQueue != null ? offsettedPosition : position;
            set => position = value;
        }

        /// <summary>
        /// Move queue of the block that will move this block.
        /// </summary>
        public MoveQueue? MoveQueue { get; private set; }

        public MovableBlock(Vector3 position, RgbaColor color, MoveQueue? moveQueue = null) : base(position, color)
        {
            MoveQueue = moveQueue;
        }

        public virtual void Update(float deltaTime, Level level)
        {
            ArgumentNullException.ThrowIfNull(level, nameof(level));
            if (MoveQueue != null)
            {
                attachPlayer = level.Player.Position == offsettedPosition + new Vector3(0, 1.0f, 0);
                offsettedPosition = position + MoveQueue.Offset;

                ModelMatrix = Matrix4x4.CreateTranslation(offsettedPosition);

                // TODO: something is wrong with player attaching
                if (MoveQueue.IsMoving)
                {
                    float distance = Vector3.Distance(offsettedPosition, level.Player.Position);
                    float nextPositionDistance = Vector3.Distance(offsettedPosition, level.Player.NextPosition);

                    if (distance < 1.0f || nextPositionDistance < 1.0f || attachPlayer)
                        // TODO: calculate real push offset
                        level.Player.Push(MoveQueue.FrameOffset);
                }
                else if (attachPlayer)
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