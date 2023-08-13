using Silk.NET.Maths;

namespace Microcube.Game.Blocks.Moving
{
    public class Movement
    {
        private readonly float time;
        private float elapsedTime;

        public Vector3D<float> FrameOffset { get; private set; }

        public Vector3D<float> FinalOffset { get; private set; }

        public bool IsTimeElapsed { get; private set; }

        public Movement(float x, float y, float z, float time)
        {
            this.time = time;
            FinalOffset = new Vector3D<float>(x, y, z);
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
                    FrameOffset = Vector3D<float>.Zero;
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