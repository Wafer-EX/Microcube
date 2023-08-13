using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects.Translations
{
    public abstract class Translation : ScreenEffect
    {
        public float ElapsedTime { get; set; }

        public float Speed { get; set; }

        public bool IsReversed { get; set; }

        public bool IsIntersectedCenter { get; private set; }

        public bool IsEnabled { get; set; }

        public Translation(GL gl, string vertexShaderPath, string fragmentShaderPath) : base(gl, vertexShaderPath, fragmentShaderPath)
        {
            ElapsedTime = 0.0f;
            Speed = 1.0f;
            IsReversed = false;
        }

        public void Update(float deltaTime)
        {
            if (IsEnabled)
            {
                float previousTime = ElapsedTime;

                ElapsedTime += (IsReversed ? -deltaTime : deltaTime) * Speed;
                ElapsedTime = Clamp(ElapsedTime, 0.0f, 1.0f, out bool isClamped);
                IsIntersectedCenter = previousTime > 0.5f && ElapsedTime <= 0.5f || previousTime <= 0.5f && ElapsedTime > 0.5f;

                if (isClamped)
                {
                    Reset();
                }
            }
        }

        public void Reset()
        {
            IsEnabled = false;
            IsIntersectedCenter = false;
            ElapsedTime = IsReversed ? 1.0f : 0.0f;
        }

        private static float Clamp(float value, float min, float max, out bool isClamped)
        {
            isClamped = value < min || value > max;
            return value < min ? min : value > max ? max : value;
        }
    }
}