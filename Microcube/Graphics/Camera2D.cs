using System.Numerics;

namespace Microcube.Graphics
{
    /// <summary>
    /// Represents a 2D camera that is used to render flat 2D objects (like UI).
    /// </summary>
    public class Camera2D
    {
        /// <summary>
        /// Width of the camera.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Height of the camera.
        /// </summary>
        public float Height { get; set; }

        public Camera2D(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Get projection matrix to use it in a shader.
        /// </summary>
        /// <returns>Projection matrix.</returns>
        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreateOrthographicOffCenter(0, Width, Height, 0, -1.0f, 1.0f);
        }
    }
}