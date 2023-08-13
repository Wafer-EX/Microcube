using Microcube.Graphics.Raster;
using Microcube.Input;

namespace Microcube.Extensions
{
    public static class IEnumerableExtensions
    {
        public static float[] ToSpriteData(this IEnumerable<Sprite> sprites)
        {
            var spriteData = new List<float>(sprites.Count() * 48);
            foreach (var sprite in sprites)
                spriteData.AddRange(sprite.GetData());

            return spriteData.ToArray();
        }
    }
}