using Silk.NET.Maths;

namespace Microcube.Graphics
{
    // TODO: should I make caching?
    public class Camera2D
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public Camera2D(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Matrix4X4<float> GetProjectionMatrix()
        {
            return Matrix4X4.CreateOrthographicOffCenter(0, Width, Height, 0, -1.0f, 1.0f);
        }
    }
}