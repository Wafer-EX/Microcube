using Silk.NET.OpenGL;
using StbImageSharp;

namespace Microcube.Graphics.Abstractions
{
    public class TextureObject : IDisposable
    {
        private readonly GL gl;

        public uint Identifier { get; init; }

        public uint Width { get; init; }

        public uint Height { get; init; }

        public TextureObject(GL gl, uint width, uint height, InternalFormat internalFormat, PixelFormat pixelFormat, PixelType pixelType)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;

            Width = width;
            Height = height;
            Identifier = gl.GenTexture();

            gl.BindTexture(TextureTarget.Texture2D, Identifier);
            gl.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, pixelFormat, pixelType, ReadOnlySpan<byte>.Empty);
        }

        public TextureObject(GL gl, string path)
        {
            ArgumentNullException.ThrowIfNull(nameof(gl));
            this.gl = gl;

            ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);

            Identifier = gl.GenTexture();
            Width = (uint)result.Width;
            Height = (uint)result.Height;

            gl.BindTexture(TextureTarget.Texture2D, Identifier);
            gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, new ReadOnlySpan<byte>(result.Data));
        }

        public void SetParameter(TextureParameterName name, GLEnum value)
        {
            gl.TextureParameter(Identifier, name, (int)value);
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            gl.ActiveTexture(unit);
            gl.BindTexture(TextureTarget.Texture2D, Identifier);
        }

        public void Dispose()
        {
            gl.DeleteTexture(Identifier);
            GC.SuppressFinalize(this);
        }
    }
}