using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Microcube.Graphics.Raster.TextModifiers;
using System.Drawing;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represents a component that shows a text inside.
    /// </summary>
    public class TextComponent : Component
    {
        /// <summary>
        /// The text that will be displayed in the component.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The font that will be used when rendering. This component switches some font parameters,
        /// so it's not recommended to use global fonts here that are used somewhere else than in UI.
        /// </summary>
        public required BitmapFont Font { get; set; }

        /// <summary>
        /// The color of the drawing text
        /// </summary>
        public required RgbaColor Color { get; set; }

        /// <summary>
        /// Text modifier that will be applied to the font, also it will be updated inside the component,
        /// so it's not recommended to use global text modifiers here.
        /// </summary>
        public required ITextModifier? TextModifier { get; set; }

        /// <summary>
        /// Horizontal aligment of the text. Is left by default.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Vertical alignment of the text. Is top by default.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        public TextComponent() : base()
        {
            Text = string.Empty;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }

        public override void Update(float deltaTime) => TextModifier?.Update(deltaTime);

        public override IEnumerable<Sprite> GetSprites(RectangleF displayedArea)
            => Font.GetSprites(Text, displayedArea, Color, TextModifier, HorizontalAlignment, VerticalAlignment);
    }
}