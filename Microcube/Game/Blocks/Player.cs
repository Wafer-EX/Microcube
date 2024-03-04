using Microcube.Game.Blocks.Enums;
using Microcube.Graphics.ColorModels;
using System.Numerics;

namespace Microcube.Game.Blocks
{
    /// <summary>
    /// Represents a player that will be controlled by a... player. I mean who will play this game.
    /// </summary>
    public class Player : Block
    {
        private readonly Level level;

        private PlayerState _state = PlayerState.Falling;
        private PlayerBarrier _barrier = PlayerBarrier.Nothing;

        private float _innerOffset = 0.0f;
        private float _velocity = 0.0f;

        private bool _isKeyPressed = false;
        private bool _isReversed = false;
        private bool _changeAxis = false;
        private bool _isPushed = false;

        private readonly float _mass = 0.01f;
        private readonly float _gravity = -9.81f;

        public override Vector3 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                // TODO: something is wrong when player falling and pushing my other block
                ModelMatrix = CalculateMovingMatrix(_innerOffset, _barrier, _changeAxis)
                    * Matrix4x4.CreateTranslation(Position);
            }
        }

        public override bool IsRender
        {
            get => base.IsRender && !level.IsFinished;
            private protected set => base.IsRender = value;
        }

        /// <summary>
        /// Position what will be setted when player will be died or a level will be started.
        /// </summary>
        public Vector3 StartPosition { get; set; }

        /// <summary>
        /// Fake position of the player that takes into account it's inner offset
        /// </summary>
        public Vector3 OffsettedPosition
        {
            get
            {
                switch (_state)
                {
                    case PlayerState.Moving:
                        if (_barrier == PlayerBarrier.Nothing)
                        {
                            float moveX = 0.0f, moveZ = _innerOffset;
                            if (_changeAxis)
                                (moveX, moveZ) = (moveZ, moveX);

                            return Position + new Vector3(moveX, 0.0f, moveZ);
                        }
                        else
                        {
                            Vector4 vector4D = Vector4.Transform(Vector4.UnitW, CalculateMovingMatrix(_innerOffset, _barrier, _changeAxis));
                            return Position + new Vector3(vector4D.X, vector4D.Y, vector4D.Z);
                        }
                    case PlayerState.Falling:
                        return new Vector3(Position.X, Position.Y + _innerOffset, Position.Z);
                }
                return Position;
            }
        }

        /// <summary>
        /// Next position that's expected when player will do step.
        /// </summary>
        public Vector3 NextPosition
        {
            get
            {
                if (_state == PlayerState.Moving && _innerOffset != 0.0f)
                {
                    float moveX = 0.0f;
                    float moveY = _barrier == PlayerBarrier.Step || _barrier == PlayerBarrier.Wall ? 1.0f : 0.0f;
                    float moveZ = _barrier != PlayerBarrier.Wall ? MathF.CopySign(1.0f, _innerOffset) : 0.0f;

                    if (_changeAxis)
                        (moveX, moveZ) = (moveZ, moveX);

                    return new Vector3(Position.X + moveX, Position.Y + moveY, Position.Z + moveZ);
                }
                return Position;
            }
        }

        // TODO: add acceleration and limit by max energy
        /// <summary>
        /// Energy of the block, I would say it represents a player speed.
        /// </summary>
        public float Energy { get; set; }

        public Player(Vector3 startPosition, RgbaColor color, Level level) : base(startPosition, color)
        {
            ArgumentNullException.ThrowIfNull(level, nameof(level));
            this.level = level;

            StartPosition = startPosition;
            Energy = 1.5f;
        }

        /// <summary>
        /// When it called, player is moving if it isn't blocked by the state or something like that.
        /// </summary>
        /// <param name="isReversed">Is change direction from forward to otherwise.</param>
        /// <param name="changeAxis">Is change axis from Z to X.</param>
        public void Move(bool isReversed, bool changeAxis)
        {
            if (_state != PlayerState.Falling)
            {
                _isReversed = isReversed;
                if (_innerOffset == 0.0f)
                    _changeAxis = changeAxis;

                if (_changeAxis == changeAxis && _barrier != PlayerBarrier.Unsuitable && _barrier != PlayerBarrier.Trap)
                    _isKeyPressed = true;
            }
        }

        /// <summary>
        /// Just pushes player, it's not the same to change position.
        /// It should be used when different block pushes this player.
        /// </summary>
        /// <param name="offset">Offset of the player.</param>
        public void Push(Vector3 offset)
        {
            Position += offset;
            _isPushed = true;
        }

        public void Update(float deltaTime)
        {
            // TODO: something is wrong with velocity on different framerates
            _velocity += _velocity * (_mass * _gravity) * deltaTime;
            Color = (RgbaColor)((HsvaColor)Color).OffsetHue(Energy * 240.0f * deltaTime);

            if (_state == PlayerState.Falling)
            {
                _velocity -= deltaTime;
                _innerOffset += _velocity;

                if (_innerOffset < -10.0f)
                    ProcessPosition(StartPosition);
                else
                {
                    Block? highestBlock = level.GetHighestBarrierFromHeight(Position.X, Position.Z, Position.Y);
                    if (highestBlock?.IsBarrier is true && OffsettedPosition.Y - highestBlock.Position.Y < 1.0f)
                        ProcessPosition(new Vector3(Position.X, highestBlock.Position.Y + 1.0f, Position.Z));
                }

                ModelMatrix = Matrix4x4.CreateTranslation(Position.X, Position.Y + _innerOffset, Position.Z);
            }
            else
            {
                if (_innerOffset == 0.0f && _state == PlayerState.Standing)
                    _barrier = EnvirnomentAnalysis.GetGlobalBarrierFromPosition(Position, level.GetBarrierBlockCoordinates(), _isReversed, _changeAxis);

                // TODO: rewrite it
                if (!_isPushed && _state == PlayerState.Standing && (Math.Truncate(Position.X) != 0.0f || Math.Truncate(Position.Y) != 0.0f))
                    ProcessPosition(new Vector3(MathF.Round(Position.X), Position.Y, MathF.Round(Position.Z)));

                float movingForceStrength = _velocity == 0.0f ? Energy : 0.50f;
                float weightForceStrength = 0.25f;

                if (_barrier == PlayerBarrier.Nothing || _barrier == PlayerBarrier.Step || _barrier == PlayerBarrier.Wall)
                {
                    if (_isKeyPressed)
                    {
                        float movingForce = (_isReversed ? -movingForceStrength : movingForceStrength) * deltaTime;
                        _velocity += movingForce;
                    }

                    if (_innerOffset != 0.0f)
                    {
                        float weightForce = _barrier switch
                        {
                            PlayerBarrier.Nothing => (MathF.Abs(_innerOffset) > 0.5f ? weightForceStrength : -weightForceStrength) * MathF.Sign(_innerOffset),
                            PlayerBarrier.Step => (MathF.Abs(_innerOffset) < 1.5f ? -weightForceStrength : weightForceStrength) * MathF.Sign(_innerOffset),
                            PlayerBarrier.Wall => MathF.CopySign(weightForceStrength, -_innerOffset),
                            _ => 0.0f
                        } * deltaTime;

                        _velocity += weightForce;
                    }
                }

                float previousInnerOffset = _innerOffset;
                _innerOffset += _velocity;

                if (_state == PlayerState.Moving && MathF.Sign(previousInnerOffset) != MathF.Sign(_innerOffset))
                    ProcessPosition(Position);
                else
                {
                    if (_innerOffset != 0.0f)
                        _state = PlayerState.Moving;

                    float criticalOffset = _barrier == PlayerBarrier.Step ? 2.0f : 1.0f;
                    if (MathF.Abs(_innerOffset) > criticalOffset)
                        ProcessPosition(NextPosition);
                }

                ModelMatrix = CalculateMovingMatrix(_innerOffset, _barrier, _changeAxis)
                    * Matrix4x4.CreateTranslation(Position);
            }

            _isKeyPressed = false;
            _isPushed = false;
        }

        /// <summary>
        /// Processes new position like when player was stepped to this position.
        /// </summary>
        /// <param name="position">New position.</param>
        public void ProcessPosition(Vector3 position)
        {
            _innerOffset = 0.0f;
            _velocity = 0.0f;
            _state = PlayerState.Standing;

            Position = position;

            Block? highestBlock = level.GetHighestBarrierFromHeight(Position.X, Position.Z, Position.Y);
            if (highestBlock == null || (highestBlock.IsBarrier && OffsettedPosition.Y - highestBlock.Position.Y > 1.0f))
                _state = PlayerState.Falling;
        }

        /// <summary>
        /// Get player model matrix when it's moving forward.
        /// </summary>
        /// <param name="offset">Inner offset of the player.</param>
        /// <param name="barrier">A barrier forward player.</param>
        /// <param name="changeAxis">Is change axis from Z to X.</param>
        /// <returns>Player model matrix</returns>
        public static Matrix4x4 CalculateMovingMatrix(float offset, PlayerBarrier barrier, bool changeAxis)
        {
            float translateY = 0.5f, translateZ = -MathF.CopySign(0.5f, offset);

            if (barrier == PlayerBarrier.Wall || barrier == PlayerBarrier.Step)
            {
                (translateY, translateZ) = (translateZ, -translateY);
                if (offset < 0.0f)
                {
                    translateY = -translateY;
                    translateZ = -translateZ;
                }
            }

            Matrix4x4 matrix = Matrix4x4.CreateTranslation(0.0f, translateY, translateZ)
                * Matrix4x4.CreateRotationX(offset * (MathF.PI / 2.0f))
                * Matrix4x4.CreateTranslation(0.0f, -translateY, -translateZ);

            if (changeAxis)
                matrix *= Matrix4x4.CreateRotationY(MathF.PI / 2.0f);

            return matrix;
        }
    }
}