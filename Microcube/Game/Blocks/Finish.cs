using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game.Blocks
{
    public class Finish : Block, IDynamic
    {
        private RgbaColor topSideColor;
        private readonly bool isCenter;

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

        public Finish(Vector3D<float> position, RgbaColor color, bool isCenter) : base(position, color)
        {
            this.isCenter = isCenter;
            topSideColor = new RgbaColor(1.0f, 0.25f, 0.25f, 1.0f);
        }

        public void Update(float deltaTime, Level level)
        {
            topSideColor = (RgbaColor)((HsvaColor)topSideColor).OffsetHue(125.0f * deltaTime);

            if (isCenter)
            {
                if (Vector3D.Distance(level.Player.Position, Position + new Vector3D<float>(0.0f, 1.0f, 0.0f)) < 1.0f)
                    level.Finish();
            }
        }

        public static IEnumerable<Finish> GenerateFinish(Vector3D<float> position, RgbaColor color)
        {
            for (float x = -1; x <= 1; x++)
            {
                for (float z = -1; z <= 1; z++)
                {
                    var blockPosition = new Vector3D<float>(position.X + x, position.Y, position.Z + z);
                    bool isCenter = x == 0.0f && z == 0.0f;
                    yield return new Finish(blockPosition, color, isCenter);
                }
            }
        }
    }
}