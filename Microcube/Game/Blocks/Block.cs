using Microcube.Graphics;
using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents an block class that can be in a level.
    /// </summary>
    public abstract class Block
    {
        /// <summary>
        /// Mesh of the block. It's the same for any blocks because... they're blocks.
        /// </summary>
        public static Mesh Mesh { get; private set; }

        /// <summary>
        /// Position of the block. Shouldn't change mesh, only transformation matrix.
        /// </summary>
        public virtual Vector3 Position { get; set; }

        /// <summary>
        /// Color of the block.
        /// </summary>
        public RgbaColor Color { get; set; }

        /// <summary>
        /// Color of the top side of the block. Is the same to color of all block by default.
        /// </summary>
        public virtual RgbaColor TopColor => Color;

        /// <summary>
        /// Model matrix of the block.
        /// </summary>
        public virtual Matrix4x4 ModelMatrix { get; set; }

        /// <summary>
        /// Represents should this block interact with player as barrier or not.
        /// If false, it will be always PlayerBarrier.Nothing to the player.
        /// </summary>
        public virtual bool IsBarrier { get; }

        /// <summary>
        /// Is render the block.
        /// </summary>
        public virtual bool IsRender { get; private protected set; }

        static Block() => Mesh = Mesh.CreateTexturedCube(1.0f);

        private protected Block(Vector3 position, RgbaColor color)
        {
            Position = position;
            Color = color;
            ModelMatrix = Matrix4x4.CreateTranslation(Position);
            IsRender = true;
        }

        /// <summary>
        /// Returns vertex data of this instance. It's for OpenGL instancing, it shouldn't include common vertex data.
        /// </summary>
        /// <returns>Vertex data of the instance.</returns>
        /// <exception cref="InvalidOperationException">If the block shouldn't rendered, this action is not allowed.</exception>
        public virtual float[] GetInstanceData()
        {
            if (!IsRender)
                throw new InvalidOperationException();

            return
            [
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
            ];
        }
    }
}