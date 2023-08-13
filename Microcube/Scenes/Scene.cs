using Microcube.Graphics;
using Microcube.Graphics.Raster;
using Microcube.Graphics.Renderers;
using Microcube.Input;
using Microcube.UI;
using Silk.NET.OpenGL;

namespace Microcube.Scenes
{
    public abstract class Scene : IDisposable
    {
        protected GL GL { get; set; }

        protected BitmapFont DefaultFont { get; set; }

        protected Camera2D SpriteCamera { get; set; }

        protected SpriteRenderer SpriteRenderer { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public SceneManager? SceneManager { get; set; }

        public UIContext UIContext { get; set; }

        public RenderTarget FinalRenderTarget { get; set; }

        public Scene(GL gl, uint width, uint height)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));

            GL = gl;
            Width = width;
            Height = height;

            DefaultFont = new BitmapFont(gl, "Resources/textures/atlases/font.png", "Resources/textures/atlases/font.xml");
            SpriteCamera = new Camera2D(width, height);
            SpriteRenderer = new SpriteRenderer(gl);

            UIContext = new UIContext(width, height);
            FinalRenderTarget = new RenderTarget(gl, width, height);
        }

        public virtual void Update(GameActionBatch actionBatch, float deltaTime)
        {
            UIContext.Update(deltaTime);
            UIContext.Input(actionBatch);
        }

        public abstract void Render(float deltaTime);

        public virtual void Dispose()
        {
            DefaultFont.Dispose();
            UIContext.Dispose();
            FinalRenderTarget.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}