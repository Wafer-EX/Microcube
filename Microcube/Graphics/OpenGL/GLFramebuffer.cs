using Silk.NET.OpenGL;

namespace Microcube.Graphics.OpenGL
{
    /// <summary>
    /// Represents an OpenGL frame buffer abstraction to use it easily in the project.
    /// </summary>
    public class GLFramebuffer : IDisposable
    {
        private readonly GL _gl;

        /// <summary>
        /// Identifier of the frame buffer object.
        /// </summary>
        public uint Identifier { get; init; }

        public GLFramebuffer(GL gl)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            _gl = gl;

            Identifier = gl.GenFramebuffer();
            Bind();
        }

        /// <summary>
        /// Attaches texture with the frame buffer.
        /// </summary>
        /// <param name="texture">Texture that should be attached.</param>
        /// <param name="attachment">Texture attachement.</param>
        public void AttachTexture(GLTexture texture, FramebufferAttachment attachment)
        {
            _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, texture.Identifier, 0);
        }

        /// <summary>
        /// Bind this frame buffer object to use it something. It's like global flag.
        /// </summary>
        public void Bind() => _gl.BindFramebuffer(FramebufferTarget.Framebuffer, Identifier);

        public void Dispose()
        {
            _gl.DeleteFramebuffer(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}