namespace Microcube.Playable.Blocks.Enums
{
    /// <summary>
    /// Represents a barrier in the moving direction at the frame.
    /// </summary>
    public enum PlayerBarrier : byte
    {
        /// <summary>
        /// Player can move forward freely.
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Player can't move forward because the barrier doesn't fits to the player by position.
        /// For example, the distance between player and barrier is 1.5f, not 1.0f or 2.0f.
        /// </summary>
        Unsuitable = 1,

        /// <summary>
        /// Represents a single block forward player, and the player can step on the block.
        /// </summary>
        Step = 2,

        /// <summary>
        /// Represents a two blocks forward player that fits by horizontal, but blocks to step.
        /// Doesn't means that the blocks should be fitted by vertically, they just should block
        /// the way as wall.
        /// </summary>
        Wall = 3,

        /// <summary>
        /// Player can't move in any direction.
        /// </summary>
        Trap = 4,
    }
}