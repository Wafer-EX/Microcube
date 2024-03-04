using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects
{
    /// <summary>
    /// A screen effect that can apply chromatic aberration to the texture.
    /// </summary>
    public class ChromaticAberrationScreenEffect(GL gl) : ScreenEffect(gl, VertexShaderPath, FragmentShaderPath)
    {
        private const string VertexShaderPath = "Resources/shaders/screen_effects/chromatic_aberration.vert";
        private const string FragmentShaderPath = "Resources/shaders/screen_effects/chromatic_aberration.frag";

        public float Strength { get; set; }

        public override void Setup(GLTexture texture)
        {
            ArgumentNullException.ThrowIfNull(texture, nameof(texture));
            texture.Bind(TextureUnit.Texture0);

            ShaderProgram.Use();
            ShaderProgram.SetUniform("sprite", 0);
            ShaderProgram.SetUniform("strength", Strength);
        }
    }
}