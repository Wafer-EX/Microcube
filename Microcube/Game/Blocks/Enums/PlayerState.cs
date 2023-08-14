namespace Microcube.Game.Blocks.Enums
{
    /// <summary>
    /// Represents player state at this frame.
    /// </summary>
    public enum PlayerState : byte
    {
        /// <summary>
        /// Any external force to the player isn't applied except by other blocks (pushing and etc.)
        /// </summary>
        Standing = 1,

        /// <summary>
        /// Means that the player is moving at the frame, even the button isn't clicked.
        /// </summary>
        Moving = 2,

        /// <summary>
        /// Sets when it's nothing in bottom of the player, may be as result of pushing or moving and etc.
        /// </summary>
        Falling = 3,
    }
}