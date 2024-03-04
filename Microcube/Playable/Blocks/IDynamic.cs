namespace Microcube.Playable.Blocks
{
    /// <summary>
    /// Represents a block that can be updated.
    /// </summary>
    public interface IDynamic
    {
        /// <summary>
        /// Updates the block.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
        /// <param name="level">Level where the block in.</param>
        public void Update(float deltaTime, Level level);
    }
}