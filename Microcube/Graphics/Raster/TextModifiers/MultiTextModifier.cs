namespace Microcube.Graphics.Raster.TextModifiers
{
    /// <summary>
    /// Represents a text modifier that can combine many text modifiers in this one.
    /// </summary>
    public class MultiTextModifier : ITextModifier
    {
        private readonly ITextModifier[] _modifiers;

        public MultiTextModifier(params ITextModifier[] modifiers)
        {
            ArgumentNullException.ThrowIfNull(modifiers, nameof(modifiers));
            _modifiers = modifiers;
        }

        public virtual Sprite ModifyCharacter(Sprite sprite, int index)
        {
            foreach (var modifier in _modifiers)
                sprite = modifier.ModifyCharacter(sprite, index);

            return sprite;
        }

        public void Update(float deltaTime)
        {
            foreach (var modifier in _modifiers)
                modifier.Update(deltaTime);
        }
    }
}