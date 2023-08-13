using Microcube.Extensions;
using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Renderers
{
    public record SpriteBatch(TextureObject? Texture, IEnumerable<Sprite> Sprites);

    public class SpriteRenderer : Renderer<IEnumerable<Sprite>, Camera2D>, IDisposable
    {
        private readonly VertexArrayObject spriteVao;
        private readonly BufferObject<float> spriteVbo;
        private readonly ShaderProgram shaderProgram;

        private readonly List<SpriteBatch> spriteBatches;

        public SpriteRenderer(GL gl) : base(gl)
        {
            ClearColor = RgbaColor.Transparent;
            IsClearBackground = false;

            shaderProgram = new ShaderProgram(gl, "Resources/shaders/sprite.vert", "Resources/shaders/sprite.frag");
            spriteVao = new VertexArrayObject(gl);

            spriteVbo = new BufferObject<float>(gl, BufferTargetARB.ArrayBuffer, null);
            spriteVao.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, 0);
            spriteVao.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 2);
            spriteVao.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 4);
            spriteVao.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 8);

            spriteBatches = new List<SpriteBatch>();
        }

        public override void SetData(IEnumerable<Sprite> sprites)
        {
            ArgumentNullException.ThrowIfNull(sprites, nameof(sprites));
            spriteBatches.Clear();

            if (sprites.Any())
            {
                List<Sprite> spritesInBatch = new();
                TextureObject? previousTexture = null;
                bool isFirstElement = true;
                bool isSkipComparison = true;

                foreach (Sprite sprite in sprites)
                {
                    if (!isSkipComparison && sprite.Texture != previousTexture)
                    {
                        spriteBatches.Add(new SpriteBatch(spritesInBatch.Last().Texture, spritesInBatch.ToArray()));
                        spritesInBatch.Clear();
                    }

                    spritesInBatch.Add(sprite);
                    previousTexture = sprite.Texture;
                    isSkipComparison = sprite.Texture == null && !isFirstElement;
                    isFirstElement = false;
                }

                spriteBatches.Add(new SpriteBatch(spritesInBatch.First().Texture, spritesInBatch.ToArray()));
            }
        }

        public override void Render(Camera2D camera, RenderTarget? renderTarget = null)
        {
            ArgumentNullException.ThrowIfNull(camera, nameof(camera));
            renderTarget?.Use();

            if (IsClearBackground)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.ClearColor(ClearColor.Red, ClearColor.Green, ClearColor.Blue, ClearColor.Alpha);
            }

            foreach (var spriteBatch in spriteBatches)
            {
                spriteVbo.SetBufferData(spriteBatch.Sprites.ToSpriteData());
                spriteBatch.Texture?.Bind(TextureUnit.Texture0);

                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                spriteVao.Bind();
                shaderProgram.Use();
                shaderProgram.SetUniform("projectionMatrix", camera.GetProjectionMatrix());
                shaderProgram.SetUniform("sprite", 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, spriteVbo.Count);
                GL.Disable(EnableCap.Blend);
            }
        }

        public override void Dispose()
        {
            spriteVao.Dispose();
            spriteVbo.Dispose();
            shaderProgram.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}