using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components.Enums;
using Silk.NET.Maths;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// The component that fits childs to one direction.
    /// </summary>
    public class StackLayout : Layout
    {
        private bool isFocused = false;

        /// <summary>
        /// Represents a focused element of childs that are can be focused.
        /// </summary>
        protected int SelectedFocusableIndex { get; set; }

        public override bool IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;

                foreach (IFocusable focusableChild in FocusableChilds)
                    focusableChild.IsFocused = false;

                if (isFocused && FocusableChilds.Any())
                {
                    SelectedFocusableIndex = 0;
                    FocusableChilds[0].IsFocused = true;
                }
            }
        }

        public override IReadOnlyList<Component?> Childs
        {
            get => base.Childs;
            set
            {
                base.Childs = value;
                if (IsFocused && FocusableChilds.Any())
                    FocusableChilds[0].IsFocused = true;
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
        protected Rectangle<float> GetComponentDisplayedArea(Rectangle<float> displayedArea, int componentCount, int index)
        {
            float positionX = displayedArea.Origin.X;
            float positionY = displayedArea.Origin.Y;
            float childWidth = displayedArea.Size.X / componentCount;
            float childHeight = displayedArea.Size.Y / componentCount;

            displayedArea = new Rectangle<float>(
                Orientation == StackLayoutOrientation.Horizontal ? positionX + childWidth * index : displayedArea.Origin.X,
                Orientation == StackLayoutOrientation.Vertical ? positionY + childHeight * index : displayedArea.Origin.Y,
                Orientation == StackLayoutOrientation.Horizontal ? childWidth : displayedArea.Size.X,
                Orientation == StackLayoutOrientation.Vertical ? childHeight : displayedArea.Size.Y);

            return displayedArea;
        }

        /// <summary>
        /// Returns sprites of specific set of components. Is useful to render only small count of childs.
        /// </summary>
        /// <param name="displayedArea">Displayed area of the component.</param>
        /// <param name="components">All components that should be displayed.</param>
        /// <returns>Sprites of these components.</returns>
        protected virtual IEnumerable<Sprite> GetSpritesOfTheseComponents(Rectangle<float> displayedArea, IReadOnlyList<Component?> components)
        {
            if (!components.Any())
                yield break;

            for (int i = 0; i < components.Count; i++)
            {
                Rectangle<float> componentDisplayedArea = GetComponentDisplayedArea(displayedArea, components.Count, i);
                foreach (var sprite in components[i]?.GetSprites(componentDisplayedArea) ?? Array.Empty<Sprite>())
                    yield return sprite;
            }
        }

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            if (BackgroundColor != RgbaColor.Transparent)
                yield return new Sprite(displayedArea, BackgroundColor);

            foreach (Sprite sprite in  GetSpritesOfTheseComponents(displayedArea, Childs))
                yield return sprite;
        }

        // TODO: refactor this because I don't understand anything here
        public override void Input(GameActionBatch actionBatch)
        {
            if (FocusableChilds.Any())
            {
                if (actionBatch.IsIncludeClick(GameAction.Escape) && FocusableChilds[SelectedFocusableIndex].IsLastFocused)
                {
                    IsFocused = false;
                    FocusableChilds[SelectedFocusableIndex].IsFocused = false;
                }
                else
                {
                    if (!FocusableChilds[SelectedFocusableIndex].IsLastFocused)
                    {
                        FocusableChilds[SelectedFocusableIndex].Input(actionBatch);
                    }
                    else if (IsFocused)
                    {
                        bool isPreviousClicked = Orientation == StackLayoutOrientation.Vertical ? actionBatch.IsIncludeClick(GameAction.Up) : actionBatch.IsIncludeClick(GameAction.Left);
                        bool isNextClicked = Orientation == StackLayoutOrientation.Vertical ? actionBatch.IsIncludeClick(GameAction.Down) : actionBatch.IsIncludeClick(GameAction.Right);

                        if (isPreviousClicked || isNextClicked)
                        {
                            FocusableChilds[SelectedFocusableIndex].IsFocused = false;
                            SelectedFocusableIndex += isPreviousClicked ? -1 : isNextClicked ? 1 : 0;

                            while (SelectedFocusableIndex < 0)
                                SelectedFocusableIndex += FocusableChilds.Count;

                            while (SelectedFocusableIndex >= FocusableChilds.Count)
                                SelectedFocusableIndex -= FocusableChilds.Count;

                            FocusableChilds[SelectedFocusableIndex].IsFocused = true;
                        }
                        else
                        {
                            FocusableChilds[SelectedFocusableIndex].Input(actionBatch);
                        }
                    }
                }
            }
        }
    }
}