using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    public class VertexArrayObject : IDisposable
    {
        private readonly GL gl;

        public uint Identifier { get; init; }

        public VertexArrayObject(GL gl)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;

            Identifier = gl.GenVertexArray();
            Bind();
        }

        public unsafe void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, uint stride, int pointer)
        {
            gl.VertexAttribPointer(index, size, type, normalized, stride, (void*)pointer);
            gl.EnableVertexAttribArray(index);
        }

        public void Bind() => gl.BindVertexArray(Identifier);

        public void Dispose()
        {
            gl.DeleteVertexArray(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}