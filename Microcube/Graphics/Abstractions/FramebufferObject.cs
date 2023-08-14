using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    /// <summary>
    /// Represents an OpenGL frame buffer abstraction to use it easily in the project.
    /// </summary>
    public class FramebufferObject : IDisposable
    {
        private readonly GL gl;

        /// <summary>
        /// Identifier of the frame buffer object.
        /// </summary>
        public uint Identifier { get; init; }

        public FramebufferObject(GL gl)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;

            Identifier = gl.GenFramebuffer();
            Bind();
        }

        /// <summary>
        /// Attaches texture with the frame buffer.
        /// </summary>
        /// <param name="texture">Texture that should be attached.</param>
        /// <param name="attachment">Texture attachement</param>
        public void AttachTexture(TextureObject texture, FramebufferAttachment attachment)
        {
            gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, texture.Identifier, 0);
        }

        /// <summary>
        /// Bind this frame buffer object to use it something. It's like global flag.
        /// </summary>
        public void Bind() => gl.BindFramebuffer(FramebufferTarget.Framebuffer, Identifier);

        public void Dispose()
        {
            gl.DeleteFramebuffer(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}