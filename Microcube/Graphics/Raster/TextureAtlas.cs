using Microcube.Graphics.Abstractions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Xml;

namespace Microcube.Graphics.Raster
{
    public class TextureAtlas<T> : IDisposable where T : notnull
    {
        protected Dictionary<T, Sprite> Sprites { get; init; }

        protected Dictionary<string, string> AdditionalInfo { get; init; }

        public TextureObject Texture { get; init; }

        public TextureAtlas(GL gl, string texturePath, string atlasPath)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));

            Sprites = new Dictionary<T, Sprite>();
            AdditionalInfo = new Dictionary<string, string>();

            Texture = new TextureObject(gl, texturePath);
            Texture.SetParameter(TextureParameterName.TextureWrapS, GLEnum.ClampToBorder);
            Texture.SetParameter(TextureParameterName.TextureWrapT, GLEnum.ClampToBorder);
            Texture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            Texture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            var document = new XmlDocument();
            document.Load(atlasPath);

            XmlElement? root = document.DocumentElement;
            if (root?.HasChildNodes is true)
            {
                foreach (XmlNode childNode in root.ChildNodes)
                {
                    if (childNode.Name == "sprites")
                    {
                        foreach (XmlNode spritesNode in childNode.ChildNodes)
                        {
                            XmlNode? identifierAttribute = spritesNode.Attributes?.GetNamedItem("identifier");
                            XmlNode? xAttribute = spritesNode.Attributes?.GetNamedItem("x");
                            XmlNode? yAttribute = spritesNode.Attributes?.GetNamedItem("y");
                            XmlNode? widthAttribute = spritesNode.Attributes?.GetNamedItem("width");
                            XmlNode? heightAttribute = spritesNode.Attributes?.GetNamedItem("height");

                            if (identifierAttribute?.Value != null && xAttribute?.Value != null && yAttribute?.Value != null && widthAttribute?.Value != null && heightAttribute?.Value != null)
                            {
                                T identifier = (T)Convert.ChangeType(identifierAttribute.Value, typeof(T));
                                float x = float.Parse(xAttribute.Value);
                                float y = float.Parse(yAttribute.Value);
                                float width = float.Parse(widthAttribute.Value);
                                float height = float.Parse(heightAttribute.Value);

                                var viewportArea = new Rectangle<float>();
                                var textureArea = new Rectangle<float>(x, y, width, height);
                                var sprite = new Sprite(Texture, viewportArea, textureArea);

                                Sprites.Add(identifier, sprite);
                            }
                        }
                    }
                    else if (childNode.Name == "additional-info")
                    {
                        foreach (XmlNode infoNode in childNode.ChildNodes)
                            AdditionalInfo.Add(infoNode.Name, infoNode.InnerText);
                    }
                }
            }
            else
            {
                Texture?.Dispose();
                throw new XmlException();
            }
        }

        public Sprite GetSprite(T identifier, Vector2D<float> position)
        {
            Sprite sprite = Sprites[identifier];
            sprite.ViewportArea = new Rectangle<float>(position, sprite.ViewportArea.Size);
            return sprite;
        }

        public void Dispose()
        {
            Sprites.Clear();
            AdditionalInfo.Clear();
            Texture.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}