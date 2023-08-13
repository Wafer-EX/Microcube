using Silk.NET.Maths;

namespace Microcube.Game.Blocks.Moving
{
    public class MoveQueue
    {
        private readonly Movement[] movements;
        private readonly Queue<Movement> movementQueue;
        private Vector3D<float> accurateOffset;

        public Vector3D<float> Offset { get; private set; }

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

        public bool IsRepeatable { get; private set; }

        public bool IsActive { get; set; }

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