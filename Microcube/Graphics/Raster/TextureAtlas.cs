using Microcube.Graphics.Abstractions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Xml;

namespace Microcube.Graphics.Raster
{
    /// <summary>
    /// Represents a texture atlas object that should be loaded from a file.
    /// </summary>
    /// <typeparam name="T">Type of sprite identifier.</typeparam>
    public class TextureAtlas<T> : IDisposable where T : notnull
    {
        /// <summary>
        /// All sprites that was parsed from a xml file.
        /// </summary>
        protected Dictionary<T, Sprite> Sprites { get; init; }

        /// <summary>
        /// All additional info that was added to the section in xml file.
        /// </summary>
        protected Dictionary<string, string> AdditionalInfo { get; init; }

        /// <summary>
        /// Texture that is associated with the texture atlas.
        /// </summary>
        public GLTexture Texture { get; init; }

        /// <summary>
        /// Loads texture atlas from the file.
        /// </summary>
        /// <param name="gl">OpenGL context.</param>
        /// <param name="texturePath">Path to the texture that is associated to the texture atlas.</param>
        /// <param name="atlasPath">Path to the xml file with all info about this texture atlas.</param>
        public TextureAtlas(GL gl, string texturePath, string atlasPath)
        {
            // I'm not sure how I should throw and process exceptions in this constructor so I haven't made it.
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));

            Sprites = [];
            AdditionalInfo = [];

            Texture = new GLTexture(gl, texturePath);
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

        /// <summary>
        /// Get sprite from the texture atlas by the identifier. Sets specific position because they are
        /// don't defined in the xml file.
        /// </summary>
        /// <param name="identifier">Identifier of the sprite.</param>
        /// <param name="position">Position of the sprite where it will be displayed.</param>
        /// <returns>Sprite from the texture atlas.</returns>
        public Sprite GetSprite(T identifier, Vector2 position)
        {
            Sprite sprite = Sprites[identifier];
            sprite.ViewportArea = new Rectangle<float>(position.ToGeneric(), sprite.ViewportArea.Size);
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