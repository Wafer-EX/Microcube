﻿using Microcube.Graphics.ColorModels;
using Microcube.Graphics.OpenGL;
using Microcube.Graphics.Shaders;
using Microcube.Playable;
using Microcube.Playable.Blocks;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Microcube.Graphics.Renderers
{
    /// <summary>
    /// Renders a level.
    /// </summary>
    public class LevelRenderer : Renderer<Level, Camera3D>, IDisposable
    {
        private readonly GLVertexArray _glVertexArray;
        private readonly GLBuffer<float> _glBufferVertices;
        private readonly GLBuffer<float> _glBufferInstances;

        private readonly BlockShader _shader;

        public LevelRenderer(GL gl) : base(gl)
        {
            ClearColor = new RgbaColor(0.1f, 0.1f, 0.1f, 1.0f);
            IsClearBackground = true;

            _shader = new BlockShader(gl)
            {
                LightDirection = new Vector3(-0.65f, 1.0f, -0.75f)
            };

            _glVertexArray = new GLVertexArray(gl);

            _glBufferVertices = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer, Block.Mesh.Vertices);
            _glVertexArray.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
            _glVertexArray.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 3 * sizeof(float));
            _glVertexArray.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 8, 6 * sizeof(float));

            _glBufferInstances = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer, null);
            _glVertexArray.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(float) * 26, 0);
            _glVertexArray.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, sizeof(float) * 26, 3 * sizeof(float));
            _glVertexArray.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, sizeof(float) * 26, 6 * sizeof(float));
            _glVertexArray.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 9 * sizeof(float));
            _glVertexArray.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 13 * sizeof(float));
            _glVertexArray.VertexAttribPointer(8, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 17 * sizeof(float));
            _glVertexArray.VertexAttribPointer(9, 4, VertexAttribPointerType.Float, false, sizeof(float) * 26, 21 * sizeof(float));
            _glVertexArray.VertexAttribPointer(10, 1, VertexAttribPointerType.Float, false, sizeof(float) * 26, 25 * sizeof(float));

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
            _glBufferInstances.SetBufferData(blockInstancesList.ToArray());
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

            _glVertexArray.Bind();

            _shader.ProjectionMatrix = camera.GetProjectionMatrix();
            _shader.ViewMatrix = camera.GetViewMatrix();
            _shader.Prepare();

            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, Block.Mesh.VerticesCount, _glBufferInstances.Count);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
        }

        public override void Dispose()
        {
            _glVertexArray.Dispose();
            _glBufferVertices.Dispose();
            _glBufferInstances.Dispose();

            _shader.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}