using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects
{
    /// <summary>
    /// Represents a screen effect that will be influence to the final picture.
    /// </summary>
    public abstract class ScreenEffect : IDisposable
    {
        // TODO: maybe I can set some attributes to new poperties in child classes
        // and analyze and set it here in the setup method.

        /// <summary>
        /// Shader program that is used when rendering.
        /// </summary>
        protected ShaderProgram ShaderProgram { get; private set; }

        public ScreenEffect(GL gl, string vertexShaderPath, string fragmentShaderPath)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            ShaderProgram = new ShaderProgram(gl, vertexShaderPath, fragmentShaderPath);
        }

        /// <summary>
        /// Prepare this screen effect to render.
        /// </summary>
        /// <param name="texture">Texture that should be rendered. (like from render target and etc.)</param>
        public abstract void Setup(TextureObject texture);

        public void Dispose()
        {
            ShaderProgram.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}