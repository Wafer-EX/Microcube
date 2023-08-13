using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components;
using Silk.NET.Maths;

namespace Microcube.UI
{
    public class UIContext : IDisposable
    {
        private Component? child;

        public uint Width { get; set; }

        public uint Height { get; set; }

        public Component? Child
        {
            get => child;
            set
            {
                child = value;
                if (child is IFocusable focusable)
                    focusable.IsFocused = true;
            }
        }

        public UIContext(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        public void Update(float deltaTime) => Child?.Update(deltaTime);

        public void Input(GameActionBatch actionBatch)
        {
            if (Child is IFocusable focusable)
                focusable.Input(actionBatch);
        }

        public IEnumerable<Sprite> GetSprites()
        {
            if (Child == null)
                yield break;

            var displayedArea = new Rectangle<float>(Vector2D<float>.Zero, Width, Height);

            foreach (Sprite sprite in Child.GetSprites(displayedArea))
                yield return sprite;
        }

        public void Dispose()
        {
            // TODO: dispose child?
            GC.SuppressFinalize(this);
        }
    }
}