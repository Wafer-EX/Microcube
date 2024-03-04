using System.Numerics;

namespace Microcube.Game.Blocks.Moving
{
    public class Movement(float x, float y, float z, float time)
    {
        private readonly float _time = time;
        private float _elapsedTime;

        public Vector3 FrameOffset { get; private set; }

        public Vector3 FinalOffset { get; private set; } = new Vector3(x, y, z);

        public bool IsTimeElapsed { get; private set; }

        public void Update(float deltaTime)
        {
            if (!IsTimeElapsed)
            {
                _elapsedTime += deltaTime;
                FrameOffset = FinalOffset / _time * deltaTime;

                if (_elapsedTime > _time)
                {
                    IsTimeElapsed = true;
                    FrameOffset = Vector3.Zero;
                }
            }
        }

        public void Reset()
        {
            _elapsedTime = 0.0f;
            IsTimeElapsed = false;
        }
    }
}