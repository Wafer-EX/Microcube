using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Microcube.Graphics.Shaders
{
    public class SpriteShader(GL gl, GLTexture? texture) : Shader(gl, "Resources/shaders/sprite.vert", "Resources/shaders/sprite.frag")
    {
        /// <summary>
        /// Sprite sheet or just an texture, it's not important because you set vertices in different place.
        /// </summary>
        public GLTexture? Texture { get; set; } = texture;

        /// <summary>
        /// Projection matrix from your camera, usually it's orthographic.
        /// </summary>
        [ShaderUniform("projectionMatrix")]
        public Matrix4x4 ProjectionMatrix { get; set; }

        /// <summary>
        /// Placeholder to set uniform in the shader.
        /// </summary>
        [ShaderUniform("sprite")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public int TextureBinding { get => 0; }

        public override void Prepare()
        {
            base.Prepare();
            Texture?.Bind(TextureUnit.Texture0);
        }
    }
}