using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Raster
{
    /// <summary>
    /// Represents an texture object abstraction that can display
    /// specific area of the texture, can be colored, scaled, rotated and etc...
    /// </summary>
    public struct Sprite
    {
        /// <summary>
        /// Original texture object that will be displayed.
        /// </summary>
        public TextureObject? Texture { get; set; }

        /// <summary>
        /// Specific area on a viewport to that the texture will be stretched.
        /// </summary>
        public Rectangle<float> ViewportArea { get; set; }

        /// <summary>
        /// Specific area of the texture object that will be displayed in the viewport area.
        /// </summary>
        public Rectangle<float> TextureArea { get; set; }

        /// <summary>
        /// Color of the texture. The texture pixel colors will be multiplied to the color.
        /// </summary>
        public RgbaColor Color { get; set; }

        /// <summary>
        /// Scale of the texture. Anchor point is center of the texture. Is 1.0f by default.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Rotation of the texture. Is 0.0f by default.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Creates an empty sprite that will be displayed just as color on the displayed area.
        /// </summary>
        /// <param name="viewportArea">Displayed area of the sprite.</param>
        /// <param name="color">Color that will be displayed in the displayed area.</param>
        public Sprite(Rectangle<float> viewportArea, RgbaColor color)
        {
            Texture = null;
            ViewportArea = viewportArea;
            TextureArea = new Rectangle<float>();

            Color = color;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

        /// <summary>
        /// Creates a sprite with ready to render texture.
        /// </summary>
        /// <param name="texture">Ready to render texture.</param>
        /// <param name="viewportArea">Viewport area of the sprite.</param>
        /// <param name="textureArea">Area of the texture that will be rendered in the viewport area.</param>
        public Sprite(TextureObject? texture, Rectangle<float> viewportArea, Rectangle<float> textureArea)
        {
            Texture = texture;
            ViewportArea = viewportArea;
            TextureArea = textureArea;

            Color = RgbaColor.White;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

        /// <summary>
        /// Creates a sprite from texture file.
        /// </summary>
        /// <param name="gl">OpenGL context.</param>
        /// <param name="texturePath">Texture path.</param>
        public Sprite(GL gl, string texturePath)
        {
            Texture = new TextureObject(gl, texturePath);
            Texture.SetParameter(TextureParameterName.TextureWrapS, GLEnum.ClampToBorder);
            Texture.SetParameter(TextureParameterName.TextureWrapT, GLEnum.ClampToBorder);
            Texture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            Texture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            ViewportArea = new Rectangle<float>(0, 0, Texture.Width, Texture.Height);
            TextureArea = new Rectangle<float>(0, 0, Texture.Width, Texture.Height);

            Color = RgbaColor.White;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

        /// <summary>
        /// Gets data that is ready to use in a shader that is made to render these sprites.
        /// </summary>
        /// <returns>Data to use in a shader</returns>
        public readonly float[] GetData()
        {
            float uvStartX = TextureArea.Origin.X / Texture?.Width ?? 1.0f;
            float uvStartY = TextureArea.Origin.Y / Texture?.Height ?? 1.0f;
            float uvEndX = uvStartX + TextureArea.Size.X / Texture?.Width ?? 1.0f;
            float evEndY = uvStartY + TextureArea.Size.Y / Texture?.Height ?? 1.0f;

            var pointA = new Vector4D<float>(ViewportArea.Origin.X, ViewportArea.Origin.Y, 0.0f, 1.0f);
            var pointB = new Vector4D<float>(ViewportArea.Origin.X + ViewportArea.Size.X, ViewportArea.Origin.Y, 0.0f, 1.0f);
            var pointC = new Vector4D<float>(ViewportArea.Origin.X + ViewportArea.Size.X, ViewportArea.Origin.Y + ViewportArea.Size.Y, 0.0f, 1.0f);
            var pointD = new Vector4D<float>(ViewportArea.Origin.X, ViewportArea.Origin.Y + ViewportArea.Size.Y, 0.0f, 1.0f);

            if (Scale != 1.0f || Rotation != 0.0f)
            {
                Matrix4X4<float> matrix = Matrix4X4.CreateTranslation(-ViewportArea.Center.X, -ViewportArea.Center.Y, 0.0f)
                    * Matrix4X4.CreateRotationZ(Rotation)
                    * Matrix4X4.CreateScale(Scale)
                    * Matrix4X4.CreateTranslation(ViewportArea.Center.X, ViewportArea.Center.Y, 0.0f);

                pointA *= matrix;
                pointB *= matrix;
                pointC *= matrix;
                pointD *= matrix;
            }

            float isIgnoreSprite = Texture is null ? 1.0f : 0.0f;
            return new float[]
            {
                pointA.X, pointA.Y, uvStartX, uvStartY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointB.X, pointB.Y, uvEndX, uvStartY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointC.X, pointC.Y, uvEndX, evEndY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointA.X, pointA.Y, uvStartX, uvStartY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointC.X, pointC.Y, uvEndX, evEndY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointD.X, pointD.Y, uvStartX, evEndY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
            };
        }
    }
}