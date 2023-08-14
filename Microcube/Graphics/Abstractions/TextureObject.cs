using Silk.NET.OpenGL;
using StbImageSharp;

namespace Microcube.Graphics.Abstractions
{
    /// <summary>
    /// Represents an OpenGL texture object abstraction.
    /// </summary>
    public class TextureObject : IDisposable
    {
        private readonly GL gl;

        /// <summary>
        /// Identifier of the texture.
        /// </summary>
        public uint Identifier { get; init; }

        /// <summary>
        /// Width of the texture.
        /// </summary>
        public uint Width { get; init; }

        /// <summary>
        /// Height of the texture.
        /// </summary>
        public uint Height { get; init; }

        /// <summary>
        /// Creates texture from the parameters.
        /// </summary>
        /// <param name="gl">OpenGL context.</param>
        /// <param name="width">Texture width.</param>
        /// <param name="height">Texture height.</param>
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

        /// <summary>
        /// Creates texture from the file.
        /// </summary>
        /// <param name="gl">OpenGL context.</param>
        /// <param name="path">Path to the texture.</param>
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

        /// <summary>
        /// Sets texture parameter.
        /// </summary>
        /// <param name="name">Texture parameter name.</param>
        /// <param name="value">Texture parameter value.</param>
        public void SetParameter(TextureParameterName name, GLEnum value)
        {
            gl.TextureParameter(Identifier, name, (int)value);
        }

        /// <summary>
        /// Binds texture to the unit. It means that if the unit is Texture0,
        /// we will get the texture from sampler in a shader when the sampler's
        /// value is 0 (it's can be setted as usual int uniform)
        /// </summary>
        /// <param name="unit">Unit to bind the texture.</param>
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