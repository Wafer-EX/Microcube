using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Microcube.Input;
using Microcube.UI.Components.Styles;
using Silk.NET.Maths;

namespace Microcube.UI.Components
{
    public class ButtonComponent : Component, IFocusable
    {
        public Action? OnClick { get; set; }

        public bool IsFocused { get; set; }

        public required string Text { get; set; }

        public required BitmapFont Font { get; set; }

        public ButtonStyle FocusedStyle { get; set; }

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