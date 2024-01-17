using Microcube.Game.Blocks.Enums;
using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents a block that can fall when player is stepped on it.
    /// </summary>
    public class FallingPlate : Block, IDynamic
    {
        private Matrix4x4 modelMatrix;

        private FallingPlateState state = FallingPlateState.Nothing;
        private float elapsedTime = 0.0f;

        private float velocity = 0.0f;
        private float innerOffset = 0.0f;

        public override Matrix4x4 ModelMatrix
        {
            get => modelMatrix;
            set
            {
                modelMatrix = Matrix4x4.CreateScale(1.0f, 0.1f, 1.0f)
                    * Matrix4x4.CreateTranslation(0.0f, 0.45f, 0.0f)
                    * value;
            }
        }

        /// <summary>
        /// Color of the top side of the block.
        /// </summary>
        public override RgbaColor TopColor
        {
            get
            {
                // TODO: refactor or remove?
                if (MathF.Round(MathF.Abs(Position.X)) % 2.0f == MathF.Round(MathF.Abs(Position.Z)) % 2.0f)
                    return new RgbaColor(Color.Red - 0.05f, Color.Green - 0.05f, Color.Blue - 0.05f, Color.Alpha);

                return Color;
            }
        }

        public override bool IsBarrier => state != FallingPlateState.Falling;

        public FallingPlate(Vector3 position, RgbaColor color) : base(position, color) { }

        public void Update(float deltaTime, Level level)
        {
            if (state == FallingPlateState.Nothing)
            {
                // TODO: refactor?
                if (Vector3.Distance(level.Player.Position, Position + new Vector3(0.0f, 1.0f, 0.0f)) < 1.0f)
                    state = FallingPlateState.Triggered;
            }
            else if (state == FallingPlateState.Triggered)
            {
                elapsedTime += deltaTime;
                if (elapsedTime >= 1.0f)
                {
                    // TODO: should I remove this or replace to mass?
                    velocity -= 0.05f;
                    state = FallingPlateState.Falling;

                    var platePlanePosition = new Vector2(Position.X, Position.Z);
                    var playerPlanePosition = new Vector2(level.Player.Position.X, level.Player.Position.Z);
                    var planeDistance = Vector2.Distance(platePlanePosition, playerPlanePosition);

                    if (planeDistance < 1.0f)
                        level.Player.ProcessPosition(level.Player.Position);
                }
            }
            else if (state == FallingPlateState.Falling)
            {
                if (IsRender && velocity > -10.0f)
                {
                    // TODO: something is wrong with this physics, it's
                    // impossible to merge it with the player class
                    float mass = 1.0f;
                    float gravity = -9.81f;

                    velocity += velocity * mass * -gravity * deltaTime;
                    innerOffset += velocity;

                    ModelMatrix = Matrix4x4.CreateTranslation(Position.X, Position.Y + innerOffset, Position.Z);
                }
                else if (IsRender)
                    IsRender = false;
            }
        }
    }
}