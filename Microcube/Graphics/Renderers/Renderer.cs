using Microcube.Graphics.ColorModels;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Renderers
{
    /// <summary>
    /// Represents a... thing that will render something
    /// </summary>
    /// <typeparam name="TData">Type of a data source.</typeparam>
    /// <typeparam name="TCamera">Type of a camera.</typeparam>
    public abstract class Renderer<TData, TCamera> : IDisposable
    {
        /// <summary>
        /// OpenGL context.
        /// </summary>
        protected GL GL { get; private set; }

        /// <summary>
        /// Color to clear the viewport.
        /// </summary>
        public RgbaColor ClearColor { get; set; }

        /// <summary>
        /// Is clear viewport by the defined color.
        /// </summary>
        public bool IsClearBackground { get; set; }

        public Renderer(GL gl)
        {
            ArgumentNullException.ThrowIfNull(nameof(gl));
            GL = gl;

            ClearColor = RgbaColor.Black;
        }

        /// <summary>
        /// Sets data to render it.
        /// </summary>
        /// <param name="dataSource"></param>
        public abstract void SetData(TData dataSource);

        /// <summary>
        /// Render setted data to specific render target. If render target is null, it will be not used.
        /// </summary>
        /// <param name="camera">Camera of the renderer.</param>
        /// <param name="renderTarget">Render target that will be used to render.</param>
        public abstract void Render(TCamera camera, RenderTarget? renderTarget = null);

        public abstract void Dispose();
    }
}