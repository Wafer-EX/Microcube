using Silk.NET.Maths;

namespace Microcube.Game.Blocks.Moving
{
    /// <summary>
    /// Represents a queue of movements that can give offset of the frame or general offset.
    /// </summary>
    public class MoveQueue
    {
        private readonly Movement[] movements;
        private readonly Queue<Movement> movementQueue;
        private Vector3D<float> accurateOffset;

        /// <summary>
        /// Represents general offset of the move queue as result of all movements for the elapsed time.
        /// </summary>
        public Vector3D<float> Offset { get; private set; }

        /// <summary>
        /// Represents a movement for the specific frame.
        /// </summary>
        public Vector3D<float> FrameOffset
        {
            get
            {
                _ = movementQueue.TryPeek(out var movement);
                if (movement == null)
                    throw new InvalidOperationException();

                return movement.FrameOffset;
            }
        }

        /// <summary>
        /// Is the move queue repeat from start when it will be ended.
        /// </summary>
        public bool IsRepeatable { get; private set; }

        /// <summary>
        /// Is the move queue should work or not (like pause).
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Is any movement was happened at the frame.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                if (IsActive)
                {
                    _ = movementQueue.TryPeek(out var movement);
                    if (movement != null)
                        return movement.FrameOffset.LengthSquared != 0.0f;
                }

                return false;
            }
        }

        public MoveQueue(Movement[] movements, bool isRepeatable, bool isActive)
        {
            ArgumentNullException.ThrowIfNull(nameof(movements));
            this.movements = movements;
            this.movementQueue = new Queue<Movement>(movements);

            IsRepeatable = isRepeatable;
            IsActive = isActive;
        }

        /// <summary>
        /// Updates a movement of the move queue and store info about this.
        /// </summary>
        /// <param name="deltaTime">Time of the frame</param>
        public void Update(float deltaTime)
        {
            if (IsActive)
            {
                movementQueue.TryPeek(out Movement? currentMovement);
                if (currentMovement != null)
                {
                    currentMovement.Update(deltaTime);
                    Offset += currentMovement.FrameOffset;

                    if (currentMovement.IsTimeElapsed)
                    {
                        accurateOffset += currentMovement.FinalOffset;
                        Offset = accurateOffset;
                        _ = movementQueue.Dequeue();
                    }
                }
                else if (IsRepeatable)
                {
                    foreach (Movement movement in movements)
                    {
                        movementQueue.Enqueue(movement);
                        movement.Reset();
                    }
                    Offset = Vector3D<float>.Zero;
                    accurateOffset = Vector3D<float>.Zero;
                }
            }
        }
    }
}