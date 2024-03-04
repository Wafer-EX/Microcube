using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    /// <summary>
    /// Represents an OpenGL buffer object abstraction to more easily use in the project.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    public class GLBuffer<T> : IDisposable where T : unmanaged
    {
        private readonly GL _gl;
        private readonly BufferTargetARB _target;

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

        public GLBuffer(GL gl, BufferTargetARB target, ReadOnlySpan<T> data)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            _gl = gl;
            _target = target;

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

            _gl.BindBuffer(_target, Identifier);
            _gl.BufferData(_target, Size, data, BufferUsageARB.DynamicDraw);
        }

        /// <summary>
        /// Bind the buffer (for example to use it with vertex array object and etc.). It's like global flag.
        /// </summary>
        public void Bind() => _gl.BindBuffer(_target, Identifier);

        public void Dispose()
        {
            _gl.DeleteBuffer(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}