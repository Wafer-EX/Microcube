using Microcube.Game;
using Microcube.Game.Blocks;
using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ColorModels;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Microcube.Graphics.Renderers
{
    /// <summary>
    /// Renders a level.
    /// </summary>
    public class LevelRenderer : Renderer<Level, Camera3D>, IDisposable
    {
        private readonly GLVertexArray blockVao;
        private readonly GLBuffer<float> blockVerticesVbo;
        private readonly GLBuffer<float> blockInstancesVbo;
        private readonly GLShaderProgram shaderProgram;

        public LevelRenderer(GL gl) : base(gl)
        {
            ClearColor = new RgbaColor(0.1f, 0.1f, 0.1f, 1.0f);
            IsClearBackground = true;

            shaderProgram = new GLShaderProgram(gl, "Resources/shaders/block.vert", "Resources/shaders/block.frag");
            blockVao = new GLVertexArray(gl);

            blockVerticesVbo = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer, Block.Mesh.Vertices);
            blockVao.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
            blockVao.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 3 * sizeof(float));
            blockVao.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 8, 6 * sizeof(float));

            blockInstancesVbo = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer, null);
            blockVao.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(float) * 26, 0);
            blockVao.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, sizeof(float) * 26, 3 * sizeof(float));
            blockVao.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, sizeof(float) * 26, 6 * sizeof(float));
            blockVao.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 9 * sizeof(float));
            blockVao.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 13 * sizeof(float));
            blockVao.VertexAttribPointer(8, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 17 * sizeof(float));
            blockVao.VertexAttribPointer(9, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 21 * sizeof(float));
            blockVao.VertexAttribPointer(10, 1, VertexAttribPointerType.Float, false, sizeof(float) * 26, 25 * sizeof(float));

            gl.VertexAttribDivisor(3, 1);
            gl.VertexAttribDivisor(4, 1);
            gl.VertexAttribDivisor(5, 1);
            gl.VertexAttribDivisor(6, 1);
            gl.VertexAttribDivisor(7, 1);
            gl.VertexAttribDivisor(8, 1);
            gl.VertexAttribDivisor(9, 1);
            gl.VertexAttribDivisor(10, 1);
        }

        public override void SetData(Level level)
        {
            ArgumentNullException.ThrowIfNull(level, nameof(level));

            var blockInstancesList = new List<float>();
            if (level.Player.IsRender)
                blockInstancesList.AddRange(level.Player.GetInstanceData());

            foreach (var block in level.Blocks)
            {
                if (block.IsRender)
                    blockInstancesList.AddRange(block.GetInstanceData());
            }
            blockInstancesVbo.SetBufferData(blockInstancesList.ToArray());
        }

        public override void Render(Camera3D camera, RenderTarget? renderTarget = null)
        {
            ArgumentNullException.ThrowIfNull(camera, nameof(camera));
            renderTarget?.Use();

            if (IsClearBackground)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.ClearColor(ClearColor.Red, ClearColor.Green, ClearColor.Blue, ClearColor.Alpha);
            }
            else
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
            }


            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            blockVao.Bind();
            shaderProgram.Use();
            shaderProgram.SetUniform("lightDirection", new Vector3(-0.65f, 1.0f, -0.75f));
            shaderProgram.SetUniform("projectionMatrix", camera.GetProjectionMatrix());
            shaderProgram.SetUniform("viewMatrix", camera.GetViewMatrix());

            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, Block.Mesh.VerticesCount, blockInstancesVbo.Count);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
        }

        public override void Dispose()
        {
            blockVao.Dispose();
            blockVerticesVbo.Dispose();
            blockInstancesVbo.Dispose();
            shaderProgram.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}