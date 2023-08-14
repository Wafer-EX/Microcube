namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a modifier of text characters that can change sprite properties.
    /// </summary>
    public interface ITextModifier
    {
        /// <summary>
        /// Modify sprite of the specific character that is defined by index (position) of the character.
        /// </summary>
        /// <param name="sprite">Sprite of the character.</param>
        /// <param name="index">Index (position) of the character.</param>
        /// <returns></returns>
        public Sprite ModifyCharacter(Sprite sprite, int index);

        /// <summary>
        /// Updates this text modifier.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
        public void Update(float deltaTime);
    }
}