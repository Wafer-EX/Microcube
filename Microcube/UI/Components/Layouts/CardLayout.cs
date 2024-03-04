using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Microcube.Input;
using System.Drawing;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// Represents a layout that shows only one component at the same time and can switch it.
    /// </summary>
    public class CardLayout : Layout
    {
        private int _selectedIndex = 0;

        public override bool IsFocused { get; set; }

        /// <summary>
        /// Index of selected component. Process any value to fit it to component range.
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (Children[_selectedIndex] is IFocusable previousFocusable)
                    previousFocusable.IsFocused = false;

                _selectedIndex = value;

                while (_selectedIndex >= Children.Count)
                    _selectedIndex -= Children.Count;

                while (_selectedIndex < 0)
                    _selectedIndex += Children.Count;

                if (IsFocused && Children[_selectedIndex] is IFocusable newFocusable)
                    newFocusable.IsFocused = true;
            }
        }

        public override IReadOnlyList<Component?> Children
        {
            get => base.Children;
            set
            {
                base.Children = value;
                if (base.Children.Any())
                    SelectedIndex = 0;
            }
        }

        public CardLayout() : base() { }

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            Component? selectedChild = Children[SelectedIndex];
            foreach (Sprite sprite in selectedChild?.GetSprites(displayedArea) ?? [])
                yield return sprite;
        }

        public override void Input(GameActionBatch actionBatch)
        {
            if (Children.Any() && Children[_selectedIndex] is IFocusable focusable)
            {
                if (actionBatch.IsIncludeClick(GameAction.Escape) && focusable.IsLastFocused)
                    focusable.IsFocused = false;
                else
                {
                    if (actionBatch.IsIncludeClick(GameAction.Enter) && !focusable.IsFocused)
                        focusable.IsFocused = true;
                    else
                        focusable.Input(actionBatch);
                }
            }
        }
    }
}