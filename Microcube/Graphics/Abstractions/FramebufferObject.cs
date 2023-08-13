using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    public class FramebufferObject : IDisposable
    {
        private readonly GL gl;

        public uint Identifier { get; init; }

        public FramebufferObject(GL gl)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;

            Identifier = gl.GenFramebuffer();
            Bind();
        }

        public void AttachTexture(TextureObject texture, FramebufferAttachment attachment)
        {
            gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, texture.Identifier, 0);
        }

        public void Bind()
        {
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, Identifier);
        }

        public void Dispose()
        {
            gl.DeleteFramebuffer(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}