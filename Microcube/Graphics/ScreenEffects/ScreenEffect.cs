using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects
{
    // TODO: maybe I can set some attributes to new poperties in child classes
    // and analyze and set it here in the setup method
    public abstract class ScreenEffect : IDisposable
    {
        protected ShaderProgram ShaderProgram { get; private set; }

        public ScreenEffect(GL gl, string vertexShaderPath, string fragmentShaderPath)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            ShaderProgram = new ShaderProgram(gl, vertexShaderPath, fragmentShaderPath);
        }

        public abstract void Setup(TextureObject texture);

        public void Dispose()
        {
            ShaderProgram.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}