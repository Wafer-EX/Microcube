using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components;
using Silk.NET.Maths;

namespace Microcube.UI
{
    /// <summary>
    /// It's an entry point to any UI
    /// </summary>
    public class UIContext : IDisposable
    {
        private Component? child;

        /// <summary>
        /// Represents the UI width.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// Represents the UI height,
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Child component that must be displayed as the first component of the UI.
        /// </summary>
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

        /// <summary>
        /// Recursively updates all childs of the UI
        /// </summary>
        /// <param name="deltaTime">Speed of frame update</param>
        public void Update(float deltaTime) => Child?.Update(deltaTime);

        /// <summary>
        /// Recursively sends input to child components.
        /// </summary>
        /// <param name="actionBatch">All input information</param>
        public void Input(GameActionBatch actionBatch)
        {
            if (Child is IFocusable focusable)
                focusable.Input(actionBatch);
        }

        /// <summary>
        /// Recursively gets all sprites and primitives of child components that are ready to draw.
        /// </summary>
        /// <returns>Sprites and primitives of child components</returns>
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