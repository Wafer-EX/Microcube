using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Microcube.Input;
using Silk.NET.Maths;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// Represents a stack layout that limits displayed components at the same time.
    /// </summary>
    public class LimitedStackLayout : StackLayout
    {
        private int displayedOffset = 0;

        /// <summary>
        /// Count of components that should be displayed.
        /// </summary>
        public required int DisplayedCount { get; set; }

        public LimitedStackLayout() : base() { }

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            Index startIndex = displayedOffset;
            Index endIndex = DisplayedCount + displayedOffset;
            Range range = startIndex..endIndex;

            foreach (Sprite sprite in GetSpritesOfTheseComponents(displayedArea, Childs.Take(range).ToArray()))
                yield return sprite;
        }

        public override void Input(GameActionBatch actionBatch)
        {
            base.Input(actionBatch);

            int selectedChildIndex = 0;
            for (int childIndex = 0; childIndex < Childs.Count; childIndex++)
            {
                if (Childs[childIndex] == FocusableChilds[SelectedFocusableIndex])
                {
                    selectedChildIndex = childIndex;
                    break;
                }
            }

            if (selectedChildIndex > DisplayedCount - 1 + displayedOffset)
                displayedOffset++;

            if (selectedChildIndex < displayedOffset)
                displayedOffset--;
        }
    }
}