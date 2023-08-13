using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster.TextModifiers;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Raster
{
    public class BitmapFont : TextureAtlas<char>
    {
        public Vector2D<float> Position { get; set; }

        public RgbaColor Color { get; set; }

        public float Scale { get; set; }

        public float Tracking { get; set; }

        public float Leading { get; set; }

        public float WordSpacing { get; set; }

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

        public float GetTextHeight(string text)
        {
            string[] splittedText = text.Split('\n');
            return splittedText.Length * Leading;
        }

        public IEnumerable<Sprite> GetSprites(string text, Vector2D<float> specificPosition)
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
                    sprite.Color = Color;
                    sprite.Scale = Scale;
                    sprite.ViewportArea = new Rectangle<float>(specificPosition.X + offsetX, specificPosition.Y + offsetY, sprite.TextureArea.Size);

                    if (TextModifier != null)
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

        public IEnumerable<Sprite> GetSprites(string text) => GetSprites(text, Position);

        public IEnumerable<Sprite> GetSprites(string text, Rectangle<float> specificArea, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top)
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

                foreach (Sprite sprite in GetSprites(splittedText[lineIndex], position))
                    yield return sprite;
            }
        }
    }
}