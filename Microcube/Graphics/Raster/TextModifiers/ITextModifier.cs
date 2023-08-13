namespace Microcube.Graphics.Raster.TextModifiers
{
    public interface ITextModifier
    {
        public Sprite ModifyCharacter(Sprite sprite, int index);

        public void Update(float deltaTime);
    }
}