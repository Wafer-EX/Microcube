using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects.Translations
{
    public class DefaultTranslation : Translation
    {
        private const string VertexShaderPath = "Resources/shaders/screen_effects/translations/default.vert";
        private const string FragmentShaderPath = "Resources/shaders/screen_effects/translations/default.frag";

        public DefaultTranslation(GL gl) : base(gl, VertexShaderPath, FragmentShaderPath) { }

        public override void Setup(TextureObject texture)
        {
            ArgumentNullException.ThrowIfNull(texture, nameof(texture));
            texture.Bind(TextureUnit.Texture0);

            ShaderProgram.Use();
            ShaderProgram.SetUniform("sprite", 0);
            ShaderProgram.SetUniform("elapsedTime", ElapsedTime);
        }
    }
}