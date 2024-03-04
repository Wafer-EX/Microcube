using Microcube.Graphics.Abstractions;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Reflection;

namespace Microcube.Graphics.Shaders
{
    /// <summary>
    /// An abstraction on OpenGL shaders that can set uniforms using properties with ShaderUniformAttribute.
    /// It's more flexible than use clean OpenGL object.
    /// </summary>
    /// <param name="gl">OpenGL instance</param>
    /// <param name="vertexShaderSourcePath">Source to the vertex shader</param>
    /// <param name="fragmentShaderSourcePath">Source to the fragment shader</param>
    public abstract class Shader(GL gl, string vertexShaderSourcePath, string fragmentShaderSourcePath) : IDisposable
    {
        private readonly GLShaderProgram _glShaderProgram = new(gl, vertexShaderSourcePath, fragmentShaderSourcePath);

        /// <summary>
        /// Bind shader to use it and automatically set all uniforms.
        /// </summary>
        public virtual void Prepare()
        {
            _glShaderProgram.Use();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                object? value = property.GetValue(this);
                foreach (var attribute in property.GetCustomAttributes())
                {
                    if (attribute is ShaderUniformAttribute shaderUniformAttribute)
                    {
                        switch (value)
                        {
                            case int:
                                _glShaderProgram.SetUniform(shaderUniformAttribute.Uniform, (int)value);
                                break;
                            case float:
                                _glShaderProgram.SetUniform(shaderUniformAttribute.Uniform, (float)value);
                                break;
                            case Vector3:
                                _glShaderProgram.SetUniform(shaderUniformAttribute.Uniform, (Vector3)value);
                                break;
                            case Vector4:
                                _glShaderProgram.SetUniform(shaderUniformAttribute.Uniform, (Vector4)value);
                                break;
                            case Matrix4x4:
                                _glShaderProgram.SetUniform(shaderUniformAttribute.Uniform, (Matrix4x4)value);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _glShaderProgram.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}