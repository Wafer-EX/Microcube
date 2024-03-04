using Silk.NET.OpenGL;
using System.Numerics;

namespace Microcube.Graphics.Shaders
{
    /// <summary>
    /// Shader that used for level blocks rendering. 
    /// </summary>
    /// <param name="gl">OpenGL instance</param>
    public class BlockShader(GL gl) : Shader(gl, "Resources/shaders/block.vert", "Resources/shaders/block.frag")
    {
        [ShaderUniform("lightDirection")]
        public Vector3 LightDirection { get; set; }

        [ShaderUniform("projectionMatrix")]
        public Matrix4x4 ProjectionMatrix { get; set; }

        [ShaderUniform("viewMatrix")]
        public Matrix4x4 ViewMatrix { get; set; }
    }
}