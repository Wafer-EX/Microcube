using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster.TextModifiers;

namespace Microcube.UI.Components.Styles
{
    public struct ButtonStyle
    {
        public required RgbaColor TextColor { get; set; }

        public required RgbaColor BackgroundColor { get; set; }

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