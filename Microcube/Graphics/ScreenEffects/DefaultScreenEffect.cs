using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects
{
    /// <summary>
    /// Default screen effect that just draws texture and doing nothing else.
    /// </summary>
    public class DefaultScreenEffect : ScreenEffect
    {
        private const string VertexShaderPath = "Resources/shaders/screen_effects/default.vert";
        private const string FragmentShaderPath = "Resources/shaders/screen_effects/default.frag";

        public DefaultScreenEffect(GL gl) : base(gl, VertexShaderPath, FragmentShaderPath) { }

        public override void Setup(TextureObject texture)
        {
            ArgumentNullException.ThrowIfNull(texture, nameof(texture));
            texture.Bind(TextureUnit.Texture0);

            ShaderProgram.Use();
            ShaderProgram.SetUniform("sprite", 0);
        }
    }
}