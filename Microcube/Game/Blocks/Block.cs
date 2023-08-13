using Microcube.Graphics;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game.Blocks
{
    public abstract class Block
    {
        public static Mesh Mesh { get; private set; }

        public virtual Vector3D<float> Position { get; set; }

        public RgbaColor Color { get; set; }

        public virtual RgbaColor TopColor => Color;

        public virtual Matrix4X4<float> ModelMatrix { get; set; }

        public virtual bool IsBarrier { get; }

        public virtual bool IsRender { get; private protected set; }

        static Block() => Mesh = Mesh.CreateTexturedCube(1.0f);

        private protected Block(Vector3D<float> position, RgbaColor color)
        {
            Position = position;
            Color = color;
            ModelMatrix = Matrix4X4.CreateTranslation(Position);
            IsRender = true;
        }

        public virtual float[] GetInstanceData()
        {
            if (!IsRender)
                throw new InvalidOperationException();

            return new float[]
            {
                // Color
                Color.Red, Color.Green, Color.Blue,

                // Top color
                TopColor.Red, TopColor.Green, TopColor.Blue,

                // Edges color
                0.0f, 0.0f, 0.0f,

                // Model matrix
                ModelMatrix.M11, ModelMatrix.M12, ModelMatrix.M13, ModelMatrix.M14,
                ModelMatrix.M21, ModelMatrix.M22, ModelMatrix.M23, ModelMatrix.M24,
                ModelMatrix.M31, ModelMatrix.M32, ModelMatrix.M33, ModelMatrix.M34,
                ModelMatrix.M41, ModelMatrix.M42, ModelMatrix.M43, ModelMatrix.M44,

                // Display edges
                0.0f,
            };
        }
    }
}