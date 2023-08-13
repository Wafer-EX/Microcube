using Microcube.Graphics.ColorModels;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Renderers
{
    public abstract class Renderer<TData, TCamera> : IDisposable
    {
        protected GL GL { get; private set; }

        public RgbaColor ClearColor { get; set; }

        public bool IsClearBackground { get; set; }

        public Renderer(GL gl)
        {
            ArgumentNullException.ThrowIfNull(nameof(gl));
            GL = gl;
        }

        public abstract void SetData(TData dataSource);

        public abstract void Render(TCamera camera, RenderTarget? renderTarget = null);

        public abstract void Dispose();
    }
}