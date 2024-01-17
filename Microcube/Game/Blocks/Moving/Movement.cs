using System.Numerics;

namespace Microcube.Game.Blocks.Moving
{
    public class Movement
    {
        private readonly float time;
        private float elapsedTime;

        public Vector3 FrameOffset { get; private set; }

        public Vector3 FinalOffset { get; private set; }

        public bool IsTimeElapsed { get; private set; }

        public Movement(float x, float y, float z, float time)
        {
            this.time = time;
            FinalOffset = new Vector3(x, y, z);
        }

        public void Update(float deltaTime)
        {
            if (!IsTimeElapsed)
            {
                elapsedTime += deltaTime;
                FrameOffset = FinalOffset / time * deltaTime;

                if (elapsedTime > time)
                {
                    IsTimeElapsed = true;
                    FrameOffset = Vector3.Zero;
                }
            }
        }

        public void Reset()
        {
            elapsedTime = 0.0f;
            IsTimeElapsed = false;
        }
    }
}