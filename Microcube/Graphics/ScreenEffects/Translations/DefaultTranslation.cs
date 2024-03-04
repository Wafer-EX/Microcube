using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.ScreenEffects.Translations
{
    /// <summary>
    /// Default translation that just makes the screen darker.
    /// </summary>
    public class DefaultTranslation(GL gl) : Translation(gl, VertexShaderPath, FragmentShaderPath)
    {
        private const string VertexShaderPath = "Resources/shaders/screen_effects/translations/default.vert";
        private const string FragmentShaderPath = "Resources/shaders/screen_effects/translations/default.frag";

        public override void Setup(GLTexture texture)
        {
            ArgumentNullException.ThrowIfNull(texture, nameof(texture));
            texture.Bind(TextureUnit.Texture0);

            ShaderProgram.Use();
            ShaderProgram.SetUniform("sprite", 0);
            ShaderProgram.SetUniform("elapsedTime", ElapsedTime);
        }
    }
}