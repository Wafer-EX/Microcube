using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    public class Finish : Block, IDynamic
    {
        private RgbaColor topSideColor;
        private readonly bool isCenter;

        /// <summary>
        /// Color of the top side of the finish.
        /// </summary>
        public override RgbaColor TopColor
        {
            get
            {
                if (isCenter)
                {
                    return new RgbaColor
                    {
                        Red = 1.0f - topSideColor.Red + 0.25f,
                        Green = 1.0f - topSideColor.Green + 0.25f,
                        Blue = 1.0f - topSideColor.Blue + 0.25f,
                    };
                }

                return topSideColor;
            }
        }

        public override bool IsBarrier => true;

        public Finish(Vector3 position, RgbaColor color, bool isCenter) : base(position, color)
        {
            this.isCenter = isCenter;
            topSideColor = new RgbaColor(1.0f, 0.25f, 0.25f, 1.0f);
        }

        public void Update(float deltaTime, Level level)
        {
            topSideColor = (RgbaColor)((HsvaColor)topSideColor).OffsetHue(125.0f * deltaTime);

            if (isCenter)
            {
                if (Vector3.Distance(level.Player.Position, Position + new Vector3(0.0f, 1.0f, 0.0f)) < 1.0f)
                    level.Finish();
            }
        }

        /// <summary>
        /// Generates a finish plane with finish block at center.
        /// </summary>
        /// <param name="position">Position of the finish plane.</param>
        /// <param name="color">Color of ground block.</param>
        /// <returns>List of blocks from the generated finish plane.</returns>
        public static IEnumerable<Finish> GenerateFinish(Vector3 position, RgbaColor color)
        {
            for (float x = -1; x <= 1; x++)
            {
                for (float z = -1; z <= 1; z++)
                {
                    var blockPosition = new Vector3(position.X + x, position.Y, position.Z + z);
                    bool isCenter = x == 0.0f && z == 0.0f;
                    yield return new Finish(blockPosition, color, isCenter);
                }
            }
        }
    }
}