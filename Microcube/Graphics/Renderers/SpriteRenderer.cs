using Microcube.Extensions;
using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Raster;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Renderers
{
    /// <summary>
    /// Represents a batch of sprites with the same texture.
    /// </summary>
    /// <param name="Texture">Texture of these sprites.</param>
    /// <param name="Sprites">Sprites in the batch.</param>
    public record SpriteBatch(GLTexture? Texture, IEnumerable<Sprite> Sprites);

    /// <summary>
    /// Renders a sprite set.
    /// </summary>
    public class SpriteRenderer : Renderer<IEnumerable<Sprite>, Camera2D>, IDisposable
    {
        private readonly GLVertexArray _spriteVao;
        private readonly GLBuffer<float> _spriteVbo;
        private readonly GLShaderProgram _shaderProgram;

        private readonly List<SpriteBatch> spriteBatches;

        public SpriteRenderer(GL gl) : base(gl)
        {
            ClearColor = RgbaColor.Transparent;
            IsClearBackground = false;

            _shaderProgram = new GLShaderProgram(gl, "Resources/shaders/sprite.vert", "Resources/shaders/sprite.frag");
            _spriteVao = new GLVertexArray(gl);

            _spriteVbo = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer, null);
            _spriteVao.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, 0);
            _spriteVao.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 2);
            _spriteVao.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 4);
            _spriteVao.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 8);

            spriteBatches = [];
        }

        public override void SetData(IEnumerable<Sprite> sprites)
        {
            ArgumentNullException.ThrowIfNull(sprites, nameof(sprites));
            spriteBatches.Clear();

            if (sprites.Any())
            {
                List<Sprite> spritesInBatch = [];
                GLTexture? previousTexture = null;
                bool isFirstElement = true;
                bool isSkipComparison = true;

                foreach (Sprite sprite in sprites)
                {
                    if (!isSkipComparison && sprite.Texture != previousTexture)
                    {
                        spriteBatches.Add(new SpriteBatch(spritesInBatch.Last().Texture, [.. spritesInBatch]));
                        spritesInBatch.Clear();
                    }

                    spritesInBatch.Add(sprite);
                    previousTexture = sprite.Texture;
                    isSkipComparison = sprite.Texture == null && !isFirstElement;
                    isFirstElement = false;
                }

                spriteBatches.Add(new SpriteBatch(spritesInBatch.First().Texture, [.. spritesInBatch]));
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

            foreach (SpriteBatch spriteBatch in spriteBatches)
            {
                _spriteVbo.SetBufferData(spriteBatch.Sprites.ToSpriteData());
                spriteBatch.Texture?.Bind(TextureUnit.Texture0);

                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                _spriteVao.Bind();
                _shaderProgram.Use();
                _shaderProgram.SetUniform("projectionMatrix", camera.GetProjectionMatrix());
                _shaderProgram.SetUniform("sprite", 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, _spriteVbo.Count);
                GL.Disable(EnableCap.Blend);
            }
        }

        public override void Dispose()
        {
            _spriteVao.Dispose();
            _spriteVbo.Dispose();
            _shaderProgram.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}