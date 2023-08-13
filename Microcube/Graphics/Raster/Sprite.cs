using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Raster
{
    // TODO: should I refactor this class?
    public struct Sprite
    {
        public TextureObject? Texture { get; set; }

        public Rectangle<float> ViewportArea { get; set; }

        public Rectangle<float> TextureArea { get; set; }

        public RgbaColor Color { get; set; }

        public float Scale { get; set; }

        public float Rotation { get; set; }

        // It's used in empty component as a test
        public Sprite(Rectangle<float> viewportArea, RgbaColor color)
        {
            Texture = null;
            ViewportArea = viewportArea;
            TextureArea = new Rectangle<float>();

            Color = color;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

        public Sprite(TextureObject? texture, Rectangle<float> viewportArea, Rectangle<float> textureArea)
        {
            Texture = texture;
            ViewportArea = viewportArea;
            TextureArea = textureArea;

            Color = RgbaColor.White;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

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