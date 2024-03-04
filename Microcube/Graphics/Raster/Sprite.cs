using Microcube.Graphics.ColorModels;
using Microcube.Graphics.OpenGL;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;

namespace Microcube.Graphics.Raster
{
    /// <summary>
    /// Represents an texture object abstraction that can display specific area of the texture,
    /// can be colored, scaled, rotated and etc...
    /// </summary>
    public struct Sprite
    {
        /// <summary>
        /// Original texture object that will be displayed.
        /// </summary>
        public GLTexture? Texture { get; set; }

        /// <summary>
        /// Specific area on a viewport to that the texture will be stretched.
        /// </summary>
        //public Rectangle<float> ViewportArea { get; set; }
        public RectangleF ViewportArea { get; set; }

        /// <summary>
        /// Specific area of the texture object that will be displayed in the viewport area.
        /// </summary>
        public RectangleF TextureArea { get; set; }

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
        public Sprite(RectangleF viewportArea, RgbaColor color)
        {
            Texture = null;
            ViewportArea = viewportArea;
            TextureArea = RectangleF.Empty;

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
        public Sprite(GLTexture? texture, RectangleF viewportArea, RectangleF textureArea)
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
            Texture = new GLTexture(gl, texturePath);
            Texture.SetParameter(TextureParameterName.TextureWrapS, GLEnum.ClampToBorder);
            Texture.SetParameter(TextureParameterName.TextureWrapT, GLEnum.ClampToBorder);
            Texture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            Texture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            ViewportArea = new RectangleF(0, 0, Texture.Width, Texture.Height);
            TextureArea = new RectangleF(0, 0, Texture.Width, Texture.Height);

            Color = RgbaColor.White;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

        /// <summary>
        /// Gets data that is ready to use in a shader that is made to render these sprites.
        /// </summary>
        /// <returns>Data to use in a shader.</returns>
        public readonly float[] GetData()
        {
            float uvStartX = TextureArea.X / Texture?.Width ?? 1.0f;
            float uvStartY = TextureArea.Y / Texture?.Height ?? 1.0f;
            float uvEndX = uvStartX + TextureArea.Width / Texture?.Width ?? 1.0f;
            float uvEndY = uvStartY + TextureArea.Height / Texture?.Height ?? 1.0f;

            var pointA = new Vector4(ViewportArea.X, ViewportArea.Y, 0.0f, 1.0f);
            var pointB = new Vector4(ViewportArea.X + ViewportArea.Width, ViewportArea.Y, 0.0f, 1.0f);
            var pointC = new Vector4(ViewportArea.X + ViewportArea.Width, ViewportArea.Y + ViewportArea.Height, 0.0f, 1.0f);
            var pointD = new Vector4(ViewportArea.X, ViewportArea.Y + ViewportArea.Height, 0.0f, 1.0f);

            if (Scale != 1.0f || Rotation != 0.0f)
            {
                Matrix4x4 matrix = Matrix4x4.CreateTranslation(-(ViewportArea.X + ViewportArea.Width / 2.0f), -(ViewportArea.Y + ViewportArea.Height / 2.0f), 0.0f)
                    * Matrix4x4.CreateRotationZ(Rotation)
                    * Matrix4x4.CreateScale(Scale)
                    * Matrix4x4.CreateTranslation(ViewportArea.X + ViewportArea.Width / 2.0f, ViewportArea.Y + ViewportArea.Height / 2.0f, 0.0f);

                pointA = Vector4.Transform(pointA, matrix);
                pointB = Vector4.Transform(pointB, matrix);
                pointC = Vector4.Transform(pointC, matrix);
                pointD = Vector4.Transform(pointD, matrix);
            }

            float isIgnoreSprite = Texture is null ? 1.0f : 0.0f;

            return
            [
                pointA.X, pointA.Y, uvStartX, uvStartY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointB.X, pointB.Y, uvEndX, uvStartY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointC.X, pointC.Y, uvEndX, uvEndY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointA.X, pointA.Y, uvStartX, uvStartY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointC.X, pointC.Y, uvEndX, uvEndY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
                pointD.X, pointD.Y, uvStartX, uvEndY, Color.Red, Color.Green, Color.Blue, Color.Alpha, isIgnoreSprite,
            ];
        }
    }
}