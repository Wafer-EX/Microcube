using Microcube.Graphics.Raster;
using Microcube.UI.Components.Enums;
using Silk.NET.Maths;

namespace Microcube.UI.Components.Layouts
{
    public class WeightedStackLayout : StackLayout
    {
        public required float[] Weights { get; set; }

        public WeightedStackLayout() : base() { }

        protected override IEnumerable<Sprite> GetSpritesOfTheseComponents(Rectangle<float> displayedArea, IReadOnlyList<Component?> components)
        {
            if (!components.Any())
                yield break;

            float childWidth = displayedArea.Size.X / components.Count;
            float childHeight = displayedArea.Size.Y / components.Count;
            float weightFactor = components.Count;

            for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
            {
                Rectangle<float> componentDisplayedArea = GetComponentDisplayedArea(displayedArea, components.Count, componentIndex);

                float positionX = displayedArea.Origin.X;
                float positionY = displayedArea.Origin.Y;

                switch (Orientation)
                {
                    case StackLayoutOrientation.Horizontal:
                        for (int i = 0; i < componentIndex; i++)
                            positionX += childWidth * Weights[i] * weightFactor;

                        componentDisplayedArea.Origin.X = positionX;
                        componentDisplayedArea.Size.X *= Weights[componentIndex] * weightFactor;
                        break;
                    case StackLayoutOrientation.Vertical:
                        for (int i = 0; i < componentIndex; i++)
                            positionY += childHeight * Weights[i] * weightFactor;

                        componentDisplayedArea.Origin.Y = positionY;
                        componentDisplayedArea.Size.Y *= Weights[componentIndex] * weightFactor;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                foreach (var sprite in components[componentIndex]?.GetSprites(componentDisplayedArea) ?? Array.Empty<Sprite>())
                    yield return sprite;
            }
        }
    }
}