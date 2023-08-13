using Microcube.Graphics.Raster;
using Silk.NET.Maths;

namespace Microcube.UI.Components
{
    public abstract class Component
    {
        public Component? Parent { get; set; }

        public virtual void Update(float deltaTime) { }

        public abstract IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea);
    }
}