using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Microcube.Input;
using System.Drawing;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// Represents a stack layout that limits displayed components at the same time.
    /// </summary>
    public class LimitedStackLayout : StackLayout
    {
        private int _displayedOffset = 0;

        /// <summary>
        /// Count of components that should be displayed.
        /// </summary>
        public required int DisplayedCount { get; set; }

        public LimitedStackLayout() : base() { }

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            Index startIndex = _displayedOffset;
            Index endIndex = DisplayedCount + _displayedOffset;
            Range range = startIndex..endIndex;

            foreach (Sprite sprite in GetSpritesOfTheseComponents(displayedArea, Children.Take(range).ToArray()))
                yield return sprite;
        }

        public override void Input(GameActionBatch actionBatch)
        {
            base.Input(actionBatch);

            int selectedChildIndex = 0;
            for (int childIndex = 0; childIndex < Children.Count; childIndex++)
            {
                if (Children[childIndex] == FocusableChildren[SelectedFocusableIndex])
                {
                    selectedChildIndex = childIndex;
                    break;
                }
            }

            if (selectedChildIndex > DisplayedCount - 1 + _displayedOffset)
                _displayedOffset++;

            if (selectedChildIndex < _displayedOffset)
                _displayedOffset--;
        }
    }
}