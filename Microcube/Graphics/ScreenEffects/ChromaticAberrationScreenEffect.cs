using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects
{
    public class ChromaticAberrationScreenEffect : ScreenEffect
    {
        private const string VertexShaderPath = "Resources/shaders/screen_effects/chromatic_aberration.vert";
        private const string FragmentShaderPath = "Resources/shaders/screen_effects/chromatic_aberration.frag";

        public float Strength { get; set; }

        public ChromaticAberrationScreenEffect(GL gl) : base(gl, VertexShaderPath, FragmentShaderPath) { }

        public override void Setup(TextureObject texture)
        {
            ArgumentNullException.ThrowIfNull(texture, nameof(texture));
            texture.Bind(TextureUnit.Texture0);

            ShaderProgram.Use();
            ShaderProgram.SetUniform("sprite", 0);
            ShaderProgram.SetUniform("strength", Strength);
        }
    }
}