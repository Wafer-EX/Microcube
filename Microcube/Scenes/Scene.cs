using Microcube.Graphics;
using Microcube.Graphics.Raster;
using Microcube.Graphics.Renderers;
using Microcube.Input;
using Microcube.UI;
using Silk.NET.OpenGL;

namespace Microcube.Scenes
{
    /// <summary>
    /// Represents a scene class that renders all things inside.
    /// </summary>
    public abstract class Scene : IDisposable
    {
        /// <summary>
        /// OpenGL context.
        /// </summary>
        protected GL GL { get; set; }

        /// <summary>
        /// Default font of the scene.
        /// </summary>
        protected BitmapFont DefaultFont { get; set; }

        /// <summary>
        /// Default 2D camera of the scene that matches with the scene scale and made to render sprites.
        /// </summary>
        protected Camera2D SpriteCamera { get; set; }

        /// <summary>
        /// Default sprite renderer of the scene.
        /// </summary>
        protected SpriteRenderer SpriteRenderer { get; set; }

        /// <summary>
        /// Width of the scene.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// height of the scene.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Scene manager to switch scenes (it should be implemented by child).
        /// </summary>
        public SceneManager? SceneManager { get; set; }

        /// <summary>
        /// UI context that will be displayed. Render of this should be implemented by child.
        /// </summary>
        public UIContext UIContext { get; set; }

        /// <summary>
        /// Final render target of the scene.
        /// </summary>
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

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="actionBatch">A set of game actions.</param>
        /// <param name="deltaTime">Time of the frame.</param>
        public virtual void Update(GameActionBatch actionBatch, float deltaTime)
        {
            UIContext.Update(deltaTime);
            UIContext.Input(actionBatch);
        }

        /// <summary>
        /// Renders this scene to final render target.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
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