using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game.Blocks
{
    public class Ground : MovableBlock, IDynamic
    {
        public override RgbaColor TopColor
        {
            get
            {
                if (MathF.Round(MathF.Abs(Position.X)) % 2.0f == MathF.Round(MathF.Abs(Position.Z)) % 2.0f)
                    return new RgbaColor(Color.Red - 0.05f, Color.Green - 0.05f, Color.Blue - 0.05f, Color.Alpha);

                return Color;
            }
        }

        public override bool IsBarrier => true;

        public RgbaColor EdgesColor { get; set; }

        public Ground(Vector3D<float> position, RgbaColor color, MoveQueue? moveQueue = null) : base(position, color, moveQueue)
        {
            EdgesColor = new RgbaColor(1.0f, 0.0f, 0.0f, 1.0f);
        }

        public override float[] GetInstanceData()
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
                EdgesColor.Red, EdgesColor.Green, EdgesColor.Blue,

                // Model matrix (row1, row2, row3, row4)
                ModelMatrix.M11, ModelMatrix.M12, ModelMatrix.M13, ModelMatrix.M14,
                ModelMatrix.M21, ModelMatrix.M22, ModelMatrix.M23, ModelMatrix.M24,
                ModelMatrix.M31, ModelMatrix.M32, ModelMatrix.M33, ModelMatrix.M34,
                ModelMatrix.M41, ModelMatrix.M42, ModelMatrix.M43, ModelMatrix.M44,

                // Display edges
                MoveQueue?.IsMoving ?? false ? 1.0f : 0.0f,
            };
        }

        public override void Update(float deltaTime, Level level)
        {
            if (MoveQueue?.IsMoving ?? false)
                EdgesColor = (RgbaColor)((HsvaColor)EdgesColor).OffsetHue(420.0f * deltaTime);

            base.Update(deltaTime, level);
        }

        public static IEnumerable<Ground> GeneratePlane(Vector2D<float> pointA, Vector2D<float> pointB, float height, RgbaColor color)
        {
            float minX = MathF.Min(pointA.X, pointB.X);
            float maxX = MathF.Max(pointA.X, pointB.X);
            float minY = MathF.Min(pointA.Y, pointB.Y);
            float maxY = MathF.Max(pointA.Y, pointB.Y);

            for (float x = minX; x < maxX; x++)
            {
                for (float y = minY; y < maxY; y++)
                {
                    var position = new Vector3D<float>(x, height, y);
                    yield return new Ground(position, color);
                }
            }
        }
    }
}