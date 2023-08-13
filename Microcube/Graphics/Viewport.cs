using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics
{
    public class Viewport
    {
        public GL GLContext { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public float AspectRatio => (float)Width / Height;

        public Viewport(GL gl, uint width, uint height)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));

            GLContext = gl;
            Width = width;
            Height = height;
        }

        public Camera2D GetCamera()
        {
            return new Camera2D(Width, Height);
        }

        public Rectangle<float> FitToCenter(float width, float height)
        {
            float scaleFactor = MathF.Min(Width / width, Height / height);
            float finalWidth = width * scaleFactor;
            float finalHeight = height * scaleFactor;

            float offsetX = Width / 2.0f - finalWidth / 2.0f;
            float offsetY = Height / 2.0f - finalHeight / 2.0f;

            return new Rectangle<float>(offsetX, offsetY, finalWidth, finalHeight);
        }
    }
}