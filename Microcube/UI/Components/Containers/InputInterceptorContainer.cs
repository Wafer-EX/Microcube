using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Microcube.Input;
using Silk.NET.Maths;
using System.Drawing;

namespace Microcube.UI.Components.Containers
{
    /// <summary>
    /// Container that do nothing with displayed area but can intercept input.
    /// Is useful to change actions in the chain of components.
    /// </summary>
    public class InputInterceptorContainer : Container
    {
        /// <summary>
        /// This delegate should return true if input was intercepted, otherwise false.
        /// </summary>
        public required Predicate<GameActionBatch>? OnInterception { get; set; }

        public InputInterceptorContainer() : base() { }

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            if (Child != null)
            {
                foreach (Sprite sprite in Child.GetSprites(displayedArea))
                    yield return sprite;
            }
        }

        public override void Input(GameActionBatch actionBatch)
        {
            bool isIntercepted = OnInterception?.Invoke(actionBatch) ?? false;
            if (!isIntercepted)
                base.Input(actionBatch);
        }
    }
}