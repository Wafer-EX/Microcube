using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components;
using Silk.NET.Maths;
using System.Drawing;
using System.Numerics;

namespace Microcube.UI
{
    /// <summary>
    /// It's an entry point to any UI.
    /// </summary>
    public class UIContext(uint width, uint height) : IDisposable
    {
        private Component? _child;

        /// <summary>
        /// Represents the UI width.
        /// </summary>
        public uint Width { get; set; } = width;

        /// <summary>
        /// Represents the UI height,
        /// </summary>
        public uint Height { get; set; } = height;

        /// <summary>
        /// Child component that must be displayed as the first component of the UI.
        /// </summary>
        public Component? Child
        {
            get => _child;
            set
            {
                _child = value;
                if (_child is IFocusable focusable)
                    focusable.IsFocused = true;
            }
        }

        /// <summary>
        /// Recursively updates all childs of the UI.
        /// </summary>
        /// <param name="deltaTime">Speed of frame update.</param>
        public void Update(float deltaTime) => Child?.Update(deltaTime);

        /// <summary>
        /// Recursively sends input to child components.
        /// </summary>
        /// <param name="actionBatch">All input information.</param>
        public void Input(GameActionBatch actionBatch)
        {
            if (Child is IFocusable focusable)
                focusable.Input(actionBatch);
        }

        /// <summary>
        /// Recursively gets all sprites and primitives of child components that are ready to draw.
        /// </summary>
        /// <returns>Sprites and primitives of child components.</returns>
        public IEnumerable<Sprite> GetSprites()
        {
            if (Child == null)
                yield break;

            var displayedArea = new RectangleF(0, 0, Width, Height);

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