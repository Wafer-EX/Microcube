namespace Microcube.Playable.Blocks.Enums
{
    /// <summary>
    /// Represents state of a plate at the frame.
    /// </summary>
    public enum FallingPlateState : byte
    {
        /// <summary>
        /// Player can touch the plate, but count down isn't started.
        /// </summary>
        Nothing = 1,

        /// <summary>
        /// Player is already touched the plate and count down is started.
        /// </summary>
        Triggered = 2,

        /// <summary>
        /// Count down is already ended and player shouldn't interact with the plate.
        /// </summary>
        Falling = 3,
    }
}