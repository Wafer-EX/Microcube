using Silk.NET.OpenGL;
using System.Drawing;

namespace Microcube.Graphics
{
    /// <summary>
    /// Represents an main viewport info, the area of the window.
    /// </summary>
    public class Viewport
    {
        /// <summary>
        /// OpenGL context.
        /// </summary>
        public GL GLContext { get; set; }

        /// <summary>
        /// Width of the viewport.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// Height of the viewport.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Aspect ratio of the viewport.
        /// </summary>
        public float AspectRatio => (float)Width / Height;

        public Viewport(GL gl, uint width, uint height)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));

            GLContext = gl;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Get camera of the viewport to render something using it.
        /// </summary>
        /// <returns>Camera of the viewport.</returns>
        public Camera2D GetCamera()
        {
            return new Camera2D(Width, Height);
        }

        /// <summary>
        /// Fit square with specific resolution to the center of the viewport.
        /// </summary>
        /// <param name="width">Width of the square.</param>
        /// <param name="height">Height of the square.</param>
        /// <returns>Fitted square at center of the viewport.</returns>
        public RectangleF FitToCenter(float width, float height)
        {
            float scaleFactor = MathF.Min(Width / width, Height / height);
            float finalWidth = width * scaleFactor;
            float finalHeight = height * scaleFactor;

            float offsetX = Width / 2.0f - finalWidth / 2.0f;
            float offsetY = Height / 2.0f - finalHeight / 2.0f;

            return new RectangleF(offsetX, offsetY, finalWidth, finalHeight);
        }
    }
}