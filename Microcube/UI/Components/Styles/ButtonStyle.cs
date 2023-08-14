using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster.TextModifiers;

namespace Microcube.UI.Components.Styles
{
    /// <summary>
    /// Represents a button style that will be used when drawing.
    /// </summary>
    public struct ButtonStyle
    {
        /// <summary>
        /// Color of the text inside button.
        /// </summary>
        public required RgbaColor TextColor { get; set; }

        /// <summary>
        /// Color of filling of button space, i.e. background color. (button fills all available space as each component)
        /// </summary>
        public required RgbaColor BackgroundColor { get; set; }

        /// <summary>
        /// Text modifier that will be used when text rendering. It's not recommended to use global modifier,
        /// because button component can switch some modifier parameters.
        /// </summary>
        public required ITextModifier? TextModifier { get; set; }

        public static readonly ButtonStyle DefaultFocusedStyle = new()
        {
            TextColor = RgbaColor.Black,
            BackgroundColor = RgbaColor.White,
            TextModifier = null
        };

        public static readonly ButtonStyle DefaultUnfocusedStyle = new()
        {
            TextColor = RgbaColor.White,
            BackgroundColor = RgbaColor.Black,
            TextModifier = null
        };
    }
}