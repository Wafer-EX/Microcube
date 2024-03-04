using Microcube.Graphics.Raster;

namespace Microcube.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Converts sprite collection to shader data.
        /// </summary>
        /// <param name="sprites">Sprites collection that must me converted to shader data.</param>
        /// <returns>Shader data.</returns>
        public static float[] ToSpriteData(this IEnumerable<Sprite> sprites)
        {
            var spriteData = new List<float>(sprites.Count() * 48);
            foreach (var sprite in sprites)
                spriteData.AddRange(sprite.GetData());

            return [.. spriteData];
        }
    }
}