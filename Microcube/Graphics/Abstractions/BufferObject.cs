using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    /// <summary>
    /// Represents an OpenGL buffer object abstraction to more easily use in the project.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    public class BufferObject<T> : IDisposable where T : unmanaged
    {
        private readonly GL gl;
        private readonly BufferTargetARB target;

        /// <summary>
        /// Identifier of the buffer object.
        /// </summary>
        public uint Identifier { get; init; }

        /// <summary>
        /// Count of elements that was setted to the buffer.
        /// </summary>
        public uint Count { get; private set; }

        /// <summary>
        /// Size of the data, for example, 2 floats weights 8 bytes because float is 4 bytes.
        /// </summary>
        public uint Size { get; private set; }

        public BufferObject(GL gl, BufferTargetARB target, ReadOnlySpan<T> data)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;
            this.target = target;

            Identifier = gl.GenBuffer();
            SetBufferData(data);
        }

        /// <summary>
        /// Sets data to the buffer.
        /// </summary>
        /// <param name="data">Data that will be set to the buffer.</param>
        public unsafe void SetBufferData(ReadOnlySpan<T> data)
        {
            Count = (uint)data.Length;
            Size = (uint)(data.Length * sizeof(T));

            gl.BindBuffer(target, Identifier);
            gl.BufferData(target, Size, data, BufferUsageARB.DynamicDraw);
        }

        /// <summary>
        /// Bind the buffer (for example to use it with vertex array object and etc.). It's like global flag.
        /// </summary>
        public void Bind() => gl.BindBuffer(target, Identifier);

        public void Dispose()
        {
            gl.DeleteBuffer(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}