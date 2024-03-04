using Microcube.Graphics.Raster;
using Microcube.UI.Components.Enums;
using Silk.NET.Maths;
using System.Drawing;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// The component can do the same as stack layout but here can be applied a weights to childs.
    /// </summary>
    public class WeightedStackLayout : StackLayout
    {
        /// <summary>
        /// Weights of the childs. The count of the weights should match to child count, also they should be 1.0f if sum it.
        /// </summary>
        public required float[] Weights { get; set; }

        public WeightedStackLayout() : base() { }

        protected override IEnumerable<Sprite> GetSpritesOfTheseComponents(RectangleF displayedArea, IReadOnlyList<Component?> components)
        {
            if (!components.Any())
                yield break;

            float childWidth = displayedArea.Width / components.Count;
            float childHeight = displayedArea.Height / components.Count;
            float weightFactor = components.Count;

            for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
            {
                RectangleF componentDisplayedArea = GetComponentDisplayedArea(displayedArea, components.Count, componentIndex);

                float positionX = displayedArea.X;
                float positionY = displayedArea.Y;

                switch (Orientation)
                {
                    case StackLayoutOrientation.Horizontal:
                        for (int i = 0; i < componentIndex; i++)
                            positionX += childWidth * Weights[i] * weightFactor;

                        componentDisplayedArea.X = positionX;
                        componentDisplayedArea.Width *= Weights[componentIndex] * weightFactor;
                        break;
                    case StackLayoutOrientation.Vertical:
                        for (int i = 0; i < componentIndex; i++)
                            positionY += childHeight * Weights[i] * weightFactor;

                        componentDisplayedArea.Y = positionY;
                        componentDisplayedArea.Height *= Weights[componentIndex] * weightFactor;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                foreach (var sprite in components[componentIndex]?.GetSprites(componentDisplayedArea) ?? [])
                    yield return sprite;
            }
        }
    }
}