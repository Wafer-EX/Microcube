using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Microcube.Graphics.Raster.TextModifiers;
using Silk.NET.Maths;

namespace Microcube.UI.Components
{
    public class TextComponent : Component
    {
        public string Text { get; set; }

        public required BitmapFont Font { get; set; }

        public required RgbaColor Color { get; set; }

        public required ITextModifier? TextModifier { get; set; }

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }

        public TextComponent() : base()
        {
            Text = string.Empty;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }

        public override void Update(float deltaTime) => TextModifier?.Update(deltaTime);

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            Font.Color = Color;
            Font.TextModifier = TextModifier;

            return Font.GetSprites(Text, displayedArea, HorizontalAlignment, VerticalAlignment);
        }
    }
}