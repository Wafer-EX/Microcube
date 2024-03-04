using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components.Enums;
using Silk.NET.Maths;
using System.Drawing;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// The component that fits childs to one direction.
    /// </summary>
    public class StackLayout : Layout
    {
        private bool _isFocused = false;

        /// <summary>
        /// Represents a focused element of childs that are can be focused.
        /// </summary>
        protected int SelectedFocusableIndex { get; set; }

        public override bool IsFocused
        {
            get => _isFocused;
            set
            {
                _isFocused = value;

                foreach (IFocusable focusableChild in FocusableChildren)
                    focusableChild.IsFocused = false;

                if (_isFocused && FocusableChildren.Any())
                {
                    SelectedFocusableIndex = 0;
                    FocusableChildren[0].IsFocused = true;
                }
            }
        }

        public override IReadOnlyList<Component?> Children
        {
            get => base.Children;
            set
            {
                base.Children = value;
                if (IsFocused && FocusableChildren.Any())
                    FocusableChildren[0].IsFocused = true;
            }
        }

        /// <summary>
        /// Stack layout orientation, is vertical by default.
        /// </summary>
        public StackLayoutOrientation Orientation { get; set; }

        public StackLayout() : base() => Orientation = StackLayoutOrientation.Vertical;

        /// <summary>
        /// Calculates a displayed area of the specific child (by index).
        /// </summary>
        /// <param name="displayedArea">Displayed area of the component.</param>
        /// <param name="componentCount">Count of childs of this component.</param>
        /// <param name="index">Index of child.</param>
        /// <returns>Displayed area of specific child.</returns>
        protected RectangleF GetComponentDisplayedArea(RectangleF displayedArea, int componentCount, int index)
        {
            float positionX = displayedArea.X;
            float positionY = displayedArea.Y;
            float childWidth = displayedArea.Width / componentCount;
            float childHeight = displayedArea.Height / componentCount;

            displayedArea = new RectangleF(
                Orientation == StackLayoutOrientation.Horizontal ? positionX + childWidth * index : displayedArea.X,
                Orientation == StackLayoutOrientation.Vertical ? positionY + childHeight * index : displayedArea.Y,
                Orientation == StackLayoutOrientation.Horizontal ? childWidth : displayedArea.Width,
                Orientation == StackLayoutOrientation.Vertical ? childHeight : displayedArea.Height);

            return displayedArea;
        }

        /// <summary>
        /// Returns sprites of specific set of components. Is useful to render only small count of childs.
        /// </summary>
        /// <param name="displayedArea">Displayed area of the component.</param>
        /// <param name="components">All components that should be displayed.</param>
        /// <returns>Sprites of these components.</returns>
        protected virtual IEnumerable<Sprite> GetSpritesOfTheseComponents(RectangleF displayedArea, IReadOnlyList<Component?> components)
        {
            if (!components.Any())
                yield break;

            for (int i = 0; i < components.Count; i++)
            {
                RectangleF componentDisplayedArea = GetComponentDisplayedArea(displayedArea, components.Count, i);
                foreach (Sprite sprite in components[i]?.GetSprites(componentDisplayedArea) ?? [])
                    yield return sprite;
            }
        }

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            foreach (Sprite sprite in GetSpritesOfTheseComponents(displayedArea, Children))
                yield return sprite;
        }

        // TODO: refactor this because I don't understand anything here
        public override void Input(GameActionBatch actionBatch)
        {
            if (FocusableChildren.Any())
            {
                if (actionBatch.IsIncludeClick(GameAction.Escape) && FocusableChildren[SelectedFocusableIndex].IsLastFocused)
                {
                    IsFocused = false;
                    FocusableChildren[SelectedFocusableIndex].IsFocused = false;
                }
                else
                {
                    if (!FocusableChildren[SelectedFocusableIndex].IsLastFocused)
                    {
                        FocusableChildren[SelectedFocusableIndex].Input(actionBatch);
                    }
                    else if (IsFocused)
                    {
                        bool isPreviousClicked = Orientation == StackLayoutOrientation.Vertical ? actionBatch.IsIncludeClick(GameAction.Up) : actionBatch.IsIncludeClick(GameAction.Left);
                        bool isNextClicked = Orientation == StackLayoutOrientation.Vertical ? actionBatch.IsIncludeClick(GameAction.Down) : actionBatch.IsIncludeClick(GameAction.Right);

                        if (isPreviousClicked || isNextClicked)
                        {
                            FocusableChildren[SelectedFocusableIndex].IsFocused = false;
                            SelectedFocusableIndex += isPreviousClicked ? -1 : isNextClicked ? 1 : 0;

                            while (SelectedFocusableIndex < 0)
                                SelectedFocusableIndex += FocusableChildren.Count;

                            while (SelectedFocusableIndex >= FocusableChildren.Count)
                                SelectedFocusableIndex -= FocusableChildren.Count;

                            FocusableChildren[SelectedFocusableIndex].IsFocused = true;
                        }
                        else
                        {
                            FocusableChildren[SelectedFocusableIndex].Input(actionBatch);
                        }
                    }
                }
            }
        }
    }
}