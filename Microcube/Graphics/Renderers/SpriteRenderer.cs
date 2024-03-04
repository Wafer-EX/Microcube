using Microcube.Extensions;
using Microcube.Graphics.ColorModels;
using Microcube.Graphics.OpenGL;
using Microcube.Graphics.Raster;
using Microcube.Graphics.Shaders;
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
        private readonly GLVertexArray _glVertexArray;
        private readonly GLBuffer<float> _glBuffer;
        private readonly SpriteShader _shader;

        private readonly List<SpriteBatch> _spriteBatches;

        public SpriteRenderer(GL gl) : base(gl)
        {
            ClearColor = RgbaColor.Transparent;
            IsClearBackground = false;

            _shader = new SpriteShader(gl, null);
            _glVertexArray = new GLVertexArray(gl);

            _glBuffer = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer, null);
            _glVertexArray.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, 0);
            _glVertexArray.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 2);
            _glVertexArray.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 4);
            _glVertexArray.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, sizeof(float) * 9, sizeof(float) * 8);

            _spriteBatches = [];
        }

        public override void SetData(IEnumerable<Sprite> sprites)
        {
            ArgumentNullException.ThrowIfNull(sprites, nameof(sprites));
            _spriteBatches.Clear();

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
                        _spriteBatches.Add(new SpriteBatch(spritesInBatch.Last().Texture, [.. spritesInBatch]));
                        spritesInBatch.Clear();
                    }

                    spritesInBatch.Add(sprite);
                    previousTexture = sprite.Texture;
                    isSkipComparison = sprite.Texture == null && !isFirstElement;
                    isFirstElement = false;
                }

                _spriteBatches.Add(new SpriteBatch(spritesInBatch.First().Texture, [.. spritesInBatch]));
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

            foreach (SpriteBatch spriteBatch in _spriteBatches)
            {
                _glBuffer.SetBufferData(spriteBatch.Sprites.ToSpriteData());

                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                _glVertexArray.Bind();
                _shader.ProjectionMatrix = camera.GetProjectionMatrix();
                _shader.Texture = spriteBatch.Texture;
                _shader.Prepare();

                GL.DrawArrays(PrimitiveType.Triangles, 0, _glBuffer.Count);
                GL.Disable(EnableCap.Blend);
            }
        }

        public override void Dispose()
        {
            _glVertexArray.Dispose();
            _glBuffer.Dispose();
            _shader.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}