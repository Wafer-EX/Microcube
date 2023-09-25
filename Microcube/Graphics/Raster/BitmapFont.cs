using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster.TextModifiers;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Raster
{
    /// <summary>
    /// Abstraction on texture atlas, adds more features to render sprites as text, works only with a char identifiers.
    /// </summary>
    public class BitmapFont : TextureAtlas<char>
    {
        /// <summary>
        /// Global position of the font. Shoudn't be changed by UI.
        /// </summary>
        public Vector2D<float> Position { get; set; }

        /// <summary>
        /// Global color of the font. Shoudn't be changed by UI.
        /// </summary>
        public RgbaColor Color { get; set; }

        /// <summary>
        /// Global scale of the font.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Distance between characters.
        /// </summary>
        public float Tracking { get; set; }

        /// <summary>
        /// Distance betweed strokes.
        /// </summary>
        public float Leading { get; set; }

        /// <summary>
        /// Distance between words.
        /// </summary>
        public float WordSpacing { get; set; }

        /// <summary>
        /// Global text modifier. Shoudn't be changed by UI.
        /// </summary>
        public ITextModifier? TextModifier { get; set; }

        public BitmapFont(GL gl, string texturePath, string atlasPath) : base(gl, texturePath, atlasPath)
        {
            Position = Vector2D<float>.Zero;
            Color = RgbaColor.White;
            Scale = 1.0f;

            Tracking = float.Parse(AdditionalInfo["tracking"]);
            Leading = float.Parse(AdditionalInfo["leading"]);
            WordSpacing = float.Parse(AdditionalInfo["word-spacing"]);
        }

        public void Update(float deltaTime) => TextModifier?.Update(deltaTime);

        /// <summary>
        /// Get width of the most wide stroke in the text.
        /// </summary>
        /// <param name="text">The text which width should be calculated.</param>
        /// <returns>Width of the text.</returns>
        public float GetTextWidth(string text)
        {
            string[] splittedText = text.Split('\n');
            float width = 0.0f;

            foreach (string line in splittedText)
            {
                float lineWidth = 0.0f;
                foreach (char character in line)
                {
                    if (character == ' ')
                        lineWidth += WordSpacing;
                    else
                        lineWidth += Sprites[character].TextureArea.Size.X * Scale + Tracking;
                }

                width = MathF.Max(width, lineWidth);
            }

            return width;
        }

        /// <summary>
        /// Get height of the text.
        /// </summary>
        /// <param name="text">The text which height should be getted.</param>
        /// <returns>Height of the text.</returns>
        public float GetTextHeight(string text)
        {
            string[] splittedText = text.Split('\n');
            return splittedText.Length * Leading;
        }

        /// <summary>
        /// Get the text as a set of sprites to render it somewhere with specific position, color and text modifier.
        /// Is useful to change some parameters for this concrete call.
        /// </summary>
        /// <param name="text">Text that should be rendered.</param>
        /// <param name="specificPosition">Specific position of the text.</param>
        /// <param name="specificColor">Specific color for this call.</param>
        /// <param name="specificTextModifier">Specific text modifier for this call.</param>
        /// <returns>Sprites of the characters in the text.</returns>
        public IEnumerable<Sprite> GetSprites(string text, Vector2D<float> specificPosition, RgbaColor? specificColor = null, ITextModifier? specificTextModifier = null)
        {
            if (text.Length == 0)
                yield break;

            float offsetX = 0.0f;
            float offsetY = 0.0f;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                if (character == ' ')
                {
                    offsetX += WordSpacing;
                }
                else if (character != '\n')
                {
                    var sprite = Sprites[character];
                    sprite.Color = specificColor ?? Color;
                    sprite.Scale = Scale;
                    sprite.ViewportArea = new Rectangle<float>(specificPosition.X + offsetX, specificPosition.Y + offsetY, sprite.TextureArea.Size);

                    if (specificTextModifier != null)
                        sprite = specificTextModifier.ModifyCharacter(sprite, i);
                    else if (TextModifier != null)
                        sprite = TextModifier.ModifyCharacter(sprite, i);

                    yield return sprite;
                    offsetX += (sprite.TextureArea.Size.X + Tracking) * Scale;
                }
                else
                {
                    offsetX = 0.0f;
                    offsetY += Leading;
                }
            }
        }

        /// <summary>
        /// Get the text as a set of sprites to render it somewhere with global position.
        /// </summary>
        /// <param name="text">Text that should be rendered.</param>
        /// <returns>Sprites of the characters in the text.</returns>
        public IEnumerable<Sprite> GetSprites(string text) => GetSprites(text, Position);

        /// <summary>
        /// Get the text as a set of sprites to render it somewhere with aligned position inside specific area.
        /// Is useful to control sprites getting fully and ignore global parameters as color and text modifier.
        /// </summary>
        /// <param name="text">Text that should be rendered.</param>
        /// <param name="specificArea">Specific area where the text will be in.</param>
        /// <param name="specificColor">Specific color for this call.</param>
        /// <param name="specificTextModifier">Specific text modifier for this call.</param>
        /// <param name="horizontalAlignment">Horizontal alignment of the text.</param>
        /// <param name="verticalAlignment">Vertical alignment of the text.</param>
        /// <returns>Sprites of the characters in the text.</returns>
        public IEnumerable<Sprite> GetSprites(string text, Rectangle<float> specificArea,RgbaColor? specificColor = null, ITextModifier? specificTextModifier = null,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top)
        {
            // TODO: refactor this?

            if (text.Length == 0)
                yield break;

            float textHeight = GetTextHeight(text);
            string[] splittedText = text.Split('\n');

            for (int lineIndex = 0; lineIndex < splittedText.Length; lineIndex++)
            {
                float lineWidth = GetTextWidth(splittedText[lineIndex]);
                float lineHeight = GetTextHeight(splittedText[lineIndex]);
                float lineOffsetY = lineHeight * lineIndex;

                float offsetX = horizontalAlignment switch
                {
                    HorizontalAlignment.Left => 0.0f,
                    HorizontalAlignment.Center => MathF.Round(specificArea.Size.X / 2.0f - lineWidth / 2.0f),
                    HorizontalAlignment.Right => specificArea.Size.X - lineWidth,
                    _ => throw new NotImplementedException()
                };

                float offsetY = verticalAlignment switch
                {
                    VerticalAlignment.Top => 0.0f,
                    VerticalAlignment.Middle => MathF.Round(specificArea.Size.Y / 2.0f - textHeight / 2.0f + lineOffsetY),
                    VerticalAlignment.Bottom => specificArea.Size.Y - textHeight + lineOffsetY,
                    _ => throw new NotImplementedException()
                };

                var position = new Vector2D<float>
                {
                    X = specificArea.Origin.X + offsetX,
                    Y = specificArea.Origin.Y + offsetY,
                };

                foreach (Sprite sprite in GetSprites(splittedText[lineIndex], position, specificColor, specificTextModifier))
                    yield return sprite;
            }
        }
    }
}