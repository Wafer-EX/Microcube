namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a text modifier that can combine many text modifiers in this one.
    /// </summary>
    public class MultiTextModifier : ITextModifier
    {
        private readonly ITextModifier[] modifiers;

        public MultiTextModifier(params ITextModifier[] modifiers)
        {
            ArgumentNullException.ThrowIfNull(modifiers, nameof(modifiers));
            this.modifiers = modifiers;
        }

        public virtual Sprite ModifyCharacter(Sprite sprite, int index)
        {
            foreach (var modifier in modifiers)
                sprite = modifier.ModifyCharacter(sprite, index);

            return sprite;
        }

        public void Update(float deltaTime)
        {
            foreach (var modifier in modifiers)
                modifier.Update(deltaTime);
        }
    }
}