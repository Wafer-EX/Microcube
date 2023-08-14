namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a text modifier that can combine many text modifiers and apply it to specific range of characters of the text.
    /// </summary>
    public class LimitedMultiTextModifier : MultiTextModifier
    {
        /// <summary>
        /// The first character that will be modified (include this).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// The last character that will be modifier (indlude this).
        /// </summary>
        public int End { get; set; }

        public LimitedMultiTextModifier(int start, int end, params ITextModifier[] modifiers) : base(modifiers)
        {
            Start = start;
            End = end;
        }

        public override Sprite ModifyCharacter(Sprite sprite, int index)
        {
            if (index >= Start && index <= End)
                return base.ModifyCharacter(sprite, index);

            return sprite;
        }
    }
}