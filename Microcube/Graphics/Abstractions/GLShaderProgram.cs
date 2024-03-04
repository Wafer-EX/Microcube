using Silk.NET.OpenGL;
using System.Numerics;

namespace Microcube.Graphics.Abstractions
{
    /// <summary>
    /// Abstraction of an OpenGL shader program to more easily use in the project.
    /// </summary>
    public class GLShaderProgram : IDisposable
    {
        private readonly GL _gl;
        
        /// <summary>
        /// Identifier of the shader program.
        /// </summary>
        public uint Identifier { get; init; }

        /// <summary>
        /// Creates shader from vertex and fragment shader files.
        /// </summary>
        /// <param name="gl">OpenGL context.</param>
        /// <param name="vertexShaderPath">Vertex shader path.</param>
        /// <param name="fragmentShaderPath">Fragment shader path.</param>
        public GLShaderProgram(GL gl, string vertexShaderPath, string fragmentShaderPath)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            _gl = gl;

            uint vertexShader = CompileShader(vertexShaderPath, ShaderType.VertexShader);
            uint fragmentShader = CompileShader(fragmentShaderPath, ShaderType.FragmentShader);

            Identifier = gl.CreateProgram();
            gl.AttachShader(Identifier, vertexShader);
            gl.AttachShader(Identifier, fragmentShader);
            gl.LinkProgram(Identifier);

            gl.GetProgram(Identifier, GLEnum.LinkStatus, out int status);
            if (status == 0)
                Console.WriteLine($"Error linking shader {gl.GetProgramInfoLog(Identifier)}");

            gl.DetachShader(Identifier, vertexShader);
            gl.DetachShader(Identifier, fragmentShader);
            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);
        }

        private uint CompileShader(string path, ShaderType shaderType)
        {
            string source = File.ReadAllText(path);
            uint shader = _gl.CreateShader(shaderType);
            _gl.ShaderSource(shader, source);
            _gl.CompileShader(shader);

            string infoLog = _gl.GetShaderInfoLog(shader);
            // TODO: why shader doesn't work with it?
            if (!string.IsNullOrWhiteSpace(infoLog))
                Console.WriteLine($"Error compiling {shaderType} shader {infoLog}");

            return shader;
        }

        /// <summary>
        /// Sets the int uniform of the shader.
        /// </summary>
        /// <param name="name">Uniform name.</param>
        /// <param name="value">Uniform value.</param>
        public void SetUniform(string name, int value)
        {
            int location = GetLocation(name);
            _gl.Uniform1(location, value);
        }

        /// <summary>
        /// Sets the float uniform of the shader.
        /// </summary>
        /// <param name="name">Uniform name.</param>
        /// <param name="value">Uniform value.</param>
        public void SetUniform(string name, float value)
        {
            int location = GetLocation(name);
            _gl.Uniform1(location, value);
        }

        /// <summary>
        /// Sets the vec3 uniform of the shader.
        /// </summary>
        /// <param name="name">Uniform name.</param>
        /// <param name="value">Uniform value.</param>
        public void SetUniform(string name, Vector3 value)
        {
            int location = GetLocation(name);
            _gl.Uniform3(location, value);
        }

        /// <summary>
        /// Sets the vec4 uniform of the shader.
        /// </summary>
        /// <param name="name">Uniform name.</param>
        /// <param name="value">Uniform value.</param>
        public void SetUniform(string name, Vector4 value)
        {
            int location = GetLocation(name);
            _gl.Uniform4(location, value);
        }

        /// <summary>
        /// Sets the mat4 uniform of the shader.
        /// </summary>
        /// <param name="name">Uniform name.</param>
        /// <param name="matrix">Uniform value.</param>
        public unsafe void SetUniform(string name, Matrix4x4 matrix)
        {
            int location = GetLocation(name);
            _gl.UniformMatrix4(location, 1, false, (float*)&matrix);
        }

        /// <summary>
        /// Use this shader program when render anything. It's like global flag.
        /// </summary>
        public void Use() => _gl.UseProgram(Identifier);

        public void Dispose()
        {
            _gl.DeleteShader(Identifier);
            GC.SuppressFinalize(this);
        }

        private int GetLocation(string name)
        {
            int location = _gl.GetUniformLocation(Identifier, name);
            if (location == -1)
                throw new InvalidOperationException($"The {name} uniform not found on shader.");

            return location;
        }
    }
}