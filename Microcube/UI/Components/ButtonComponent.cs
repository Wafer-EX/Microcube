using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components.Styles;
using Silk.NET.Maths;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represend a button that can be focused and reacted when the Enter is clicked.
    /// </summary>
    public class ButtonComponent : Component, IFocusable
    {
        /// <summary>
        /// Action that will be performed after click.
        /// </summary>
        public Action? OnClick { get; set; }

        public bool IsFocused { get; set; }

        /// <summary>
        /// Text that will be displayed inside this component.
        /// </summary>
        public required string Text { get; set; }

        /// <summary>
        /// Font that will be used to display text. It's unrecommended to use global fonts because
        /// this component switches some parameters of the font.
        /// </summary>
        public required BitmapFont Font { get; set; }

        /// <summary>
        /// Button style when it's focused.
        /// </summary>
        public ButtonStyle FocusedStyle { get; set; }

        /// <summary>
        /// Button style when it's unfocused.
        /// </summary>
        public ButtonStyle UnfocusedStyle { get; set; }

        public bool IsLastFocused => true;

        public ButtonComponent() : base()
        {
            Text = string.Empty;
            FocusedStyle = ButtonStyle.DefaultFocusedStyle;
            UnfocusedStyle = ButtonStyle.DefaultUnfocusedStyle;
        }

        public override void Update(float deltaTime)
        {
            if (IsFocused)
                FocusedStyle.TextModifier?.Update(deltaTime);
            else
                UnfocusedStyle.TextModifier?.Update(deltaTime);
        }

        public override IEnumerable<Sprite> GetSprites(Rectangle<float> displayedArea)
        {
            yield return new Sprite(displayedArea, IsFocused ? FocusedStyle.BackgroundColor : UnfocusedStyle.BackgroundColor);

            Font.Color = IsFocused ? FocusedStyle.TextColor : UnfocusedStyle.TextColor;
            Font.TextModifier = IsFocused ? FocusedStyle.TextModifier : UnfocusedStyle.TextModifier;

            IEnumerable<Sprite> sprites = Font.GetSprites(Text, displayedArea, HorizontalAlignment.Center, VerticalAlignment.Middle);
            foreach (Sprite sprite in sprites)
                yield return sprite;
        }

        public void Input(GameActionBatch actionBatch)
        {
            foreach (GameActionInfo gameAction in actionBatch.GameActions)
            {
                if (gameAction.IsClicked && gameAction.Action == GameAction.Enter)
                    OnClick?.Invoke();
            }
        }
    }
}