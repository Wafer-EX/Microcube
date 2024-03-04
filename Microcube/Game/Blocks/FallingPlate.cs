using Microcube.Game.Blocks.Enums;
using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents a block that can fall when player is stepped on it.
    /// </summary>
    public class FallingPlate(Vector3 position, RgbaColor color) : Block(position, color), IDynamic
    {
        private Matrix4x4 _modelMatrix;

        private FallingPlateState _state = FallingPlateState.Nothing;
        private float _elapsedTime = 0.0f;

        private float _velocity = 0.0f;
        private float _innerOffset = 0.0f;

        public override Matrix4x4 ModelMatrix
        {
            get => _modelMatrix;
            set
            {
                _modelMatrix = Matrix4x4.CreateScale(1.0f, 0.1f, 1.0f)
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

        public override bool IsBarrier => _state != FallingPlateState.Falling;

        public void Update(float deltaTime, Level level)
        {
            if (_state == FallingPlateState.Nothing)
            {
                // TODO: refactor?
                if (Vector3.Distance(level.Player.Position, Position + new Vector3(0.0f, 1.0f, 0.0f)) < 1.0f)
                    _state = FallingPlateState.Triggered;
            }
            else if (_state == FallingPlateState.Triggered)
            {
                _elapsedTime += deltaTime;
                if (_elapsedTime >= 1.0f)
                {
                    // TODO: should I remove this or replace to mass?
                    _velocity -= 0.05f;
                    _state = FallingPlateState.Falling;

                    Vector2 platePlanePosition = new(Position.X, Position.Z);
                    Vector2 playerPlanePosition = new(level.Player.Position.X, level.Player.Position.Z);
                    float planeDistance = Vector2.Distance(platePlanePosition, playerPlanePosition);

                    if (planeDistance < 1.0f)
                        level.Player.ProcessPosition(level.Player.Position);
                }
            }
            else if (_state == FallingPlateState.Falling)
            {
                if (IsRender && _velocity > -10.0f)
                {
                    // TODO: something is wrong with this physics, it's
                    // impossible to merge it with the player class
                    float mass = 1.0f;
                    float gravity = -9.81f;

                    _velocity += _velocity * mass * -gravity * deltaTime;
                    _innerOffset += _velocity;

                    ModelMatrix = Matrix4x4.CreateTranslation(Position.X, Position.Y + _innerOffset, Position.Z);
                }
                else if (IsRender)
                    IsRender = false;
            }
        }
    }
}