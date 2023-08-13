using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    public class BufferObject<T> : IDisposable where T : unmanaged
    {
        private readonly GL gl;
        private readonly BufferTargetARB target;

        public uint Identifier { get; init; }

        public uint Count { get; private set; }

        public uint Size { get; private set; }

        public BufferObject(GL gl, BufferTargetARB target, ReadOnlySpan<T> data)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;
            this.target = target;

            Identifier = gl.GenBuffer();
            SetBufferData(data);
        }

        public unsafe void SetBufferData(ReadOnlySpan<T> data)
        {
            Count = (uint)data.Length;
            Size = (uint)(data.Length * sizeof(T));

            gl.BindBuffer(target, Identifier);
            gl.BufferData(target, Size, data, BufferUsageARB.DynamicDraw);
        }

        public void Bind() => gl.BindBuffer(target, Identifier);

        public void Dispose()
        {
            gl.DeleteBuffer(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}