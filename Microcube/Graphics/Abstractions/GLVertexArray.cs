using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    /// <summary>
    /// Represents an OpenGL vertex array object abstraction to more easily use in the project.
    /// </summary>
    public class GLVertexArray : IDisposable
    {
        private readonly GL _gl;

        /// <summary>
        /// Identidier of the vertex array object.
        /// </summary>
        public uint Identifier { get; init; }

        public GLVertexArray(GL gl)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            _gl = gl;

            Identifier = gl.GenVertexArray();
            Bind();
        }

        /// <summary>
        /// Abstraction of the glVertexAttribPointer that also enables this attribute calls glEnableVertexAttribArray.
        /// </summary>
        /// <param name="index">Index of the attribute. (as location in a shader)</param>
        public unsafe void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, uint stride, int pointer)
        {
            _gl.VertexAttribPointer(index, size, type, normalized, stride, (void*)pointer);
            _gl.EnableVertexAttribArray(index);
        }

        /// <summary>
        /// Binds this vertex array object to use somewhere. It's like global flag.
        /// </summary>
        public void Bind() => _gl.BindVertexArray(Identifier);

        public void Dispose()
        {
            _gl.DeleteVertexArray(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}