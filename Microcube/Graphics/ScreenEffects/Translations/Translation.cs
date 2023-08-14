using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects.Translations
{
    /// <summary>
    /// Represents a translation that can be applies as screen effect.
    /// </summary>
    public abstract class Translation : ScreenEffect
    {
        /// <summary>
        /// Elapsed time fo the translation. Can be from 0 to 1.
        /// </summary>
        public float ElapsedTime { get; set; }

        /// <summary>
        /// Speed of this translation.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Is this translation should be reversed in the time.
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// When the translation intersects the center, it means that
        /// a scene should be switched.
        /// </summary>
        public bool IsIntersectedCenter { get; private set; }

        /// <summary>
        /// Is the translation works or just paused.
        /// </summary>
        public bool IsEnabled { get; set; }

        public Translation(GL gl, string vertexShaderPath, string fragmentShaderPath) : base(gl, vertexShaderPath, fragmentShaderPath)
        {
            ElapsedTime = 0.0f;
            Speed = 1.0f;
            IsReversed = false;
        }

        /// <summary>
        /// Update this translation.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
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

        /// <summary>
        /// Reset this translation to start.
        /// </summary>
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