using Microcube.Extensions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics.Abstractions
{
    public class ShaderProgram : IDisposable
    {
        private readonly GL gl;

        public uint Identifier { get; init; }

        public ShaderProgram(GL gl, string vertexShaderPath, string fragmentShaderPath)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;

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
            uint shader = gl.CreateShader(shaderType);
            gl.ShaderSource(shader, source);
            gl.CompileShader(shader);

            string infoLog = gl.GetShaderInfoLog(shader);
            // TODO: why shader doesn't work with it?
            if (!string.IsNullOrWhiteSpace(infoLog))
                Console.WriteLine($"Error compiling {shaderType} shader {infoLog}");

            return shader;
        }

        public void SetUniform(string name, int value)
        {
            int location = GetLocation(name);
            gl.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = GetLocation(name);
            gl.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector3D<float> value)
        {
            int location = GetLocation(name);
            gl.Uniform3(location, value.ToSystem());
        }

        public void SetUniform(string name, Vector4D<float> value)
        {
            int location = GetLocation(name);
            gl.Uniform4(location, value.ToSystem());
        }

        public unsafe void SetUniform(string name, Matrix4X4<float> matrix)
        {
            int location = GetLocation(name);
            gl.UniformMatrix4(location, 1, false, (float*)&matrix);
        }

        public void Use() => gl.UseProgram(Identifier);

        public void Dispose()
        {
            gl.DeleteShader(Identifier);
            GC.SuppressFinalize(this);
        }

        private int GetLocation(string name)
        {
            int location = gl.GetUniformLocation(Identifier, name);
            if (location == -1)
                throw new InvalidOperationException($"The {name} uniform not found on shader.");

            return location;
        }
    }
}