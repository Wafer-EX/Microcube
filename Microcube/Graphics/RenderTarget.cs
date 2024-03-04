using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ScreenEffects;
using Silk.NET.OpenGL;
using System.Drawing;

namespace Microcube.Graphics
{
    /// <summary>
    /// Abstraction on OpenGL frame buffer, can be used to render there something like in texture.
    /// </summary>
    public class RenderTarget : IDisposable
    {
        private readonly GL _gl;

        private readonly GLVertexArray _screenQuadVao;
        private readonly GLBuffer<float> _screenQuadVbo;
        private readonly GLFramebuffer _framebuffer;

        /// <summary>
        /// Color texture that represents colors of vertices.
        /// </summary>
        public GLTexture ColorTexture { get; init; }

        /// <summary>
        /// Combine depth and stencil textures in the same object.
        /// </summary>
        public GLTexture DepthStencilTexture { get; init; }

        /// <summary>
        /// Screen effect that can change final output.
        /// </summary>
        public ScreenEffect ScreenEffect { get; set; }

        /// <summary>
        /// Width of the render target.
        /// </summary>
        public uint Width { get; init; }

        /// <summary>
        /// Height of the render target.
        /// </summary>
        public uint Height { get; init; }

        /// <summary>
        /// Identifier of the frame buffer that was generated inside this render target.
        /// </summary>
        public uint Framebuffer => _framebuffer.Identifier;

        public RenderTarget(GL gl, uint width, uint height, ScreenEffect? screenEffect = null)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            _gl = gl;

            Width = width;
            Height = height;
            ScreenEffect = screenEffect ?? new DefaultScreenEffect(gl);

            ColorTexture = new GLTexture(gl, width, height, InternalFormat.Rgb, PixelFormat.Rgb, PixelType.UnsignedByte);
            ColorTexture.SetParameter(TextureParameterName.TextureWrapS, GLEnum.ClampToEdge);
            ColorTexture.SetParameter(TextureParameterName.TextureWrapT, GLEnum.ClampToEdge);
            ColorTexture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            ColorTexture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            DepthStencilTexture = new GLTexture(gl, width, height, InternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedInt248);
            DepthStencilTexture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            DepthStencilTexture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            _framebuffer = new GLFramebuffer(gl);
            _framebuffer.AttachTexture(ColorTexture, FramebufferAttachment.ColorAttachment0);
            _framebuffer.AttachTexture(DepthStencilTexture, FramebufferAttachment.DepthStencilAttachment);

            _screenQuadVao = new GLVertexArray(gl);
            _screenQuadVbo = new GLBuffer<float>(gl, BufferTargetARB.ArrayBuffer,
            [
                // ---- SCREEN QUAD ----
                // 2x POSITIONS | 2x UVs
                -1.0f, -1.0f, 0.0f, 0.0f,
                 1.0f, -1.0f, 1.0f, 0.0f,
                 1.0f,  1.0f, 1.0f, 1.0f,
                -1.0f, -1.0f, 0.0f, 0.0f,
                 1.0f,  1.0f, 1.0f, 1.0f,
                -1.0f,  1.0f, 0.0f, 1.0f,
            ]);

            _screenQuadVao.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            _screenQuadVao.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);
        }

        /// <summary>
        /// Use this render target to render there something.
        /// </summary>
        public void Use()
        {
            _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer.Identifier);
            _gl.Viewport(0, 0, Width, Height);
        }

        /// <summary>
        /// Render this render target to specific frame buffer. Lets to draw render targets to each other.
        /// </summary>
        /// <param name="framebuffer">Specific frame buffer.</param>
        /// <param name="x">X coordinate in the viewport.</param>
        /// <param name="y">X coordinate in the viewport.</param>
        /// <param name="width">Width in the viewport.</param>
        /// <param name="height">Height in the viewport.</param>
        public void Render(uint framebuffer, int x, int y, uint width, uint height)
        {
            _gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            _gl.Viewport(x, y, width, height);

            _screenQuadVao.Bind();
            ScreenEffect.Setup(ColorTexture);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, _screenQuadVbo.Count);
        }

        /// <summary>
        /// Render this render target to specific frame buffer. Lets to draw render targets to each other.
        /// </summary>
        /// <param name="frameBuffer">Specific frame buffer.</param>
        /// <param name="displayedArea">Displayed area in the viewport.</param>
        public void Render(uint frameBuffer, RectangleF displayedArea)
        {
            Render(frameBuffer, (int)displayedArea.X, (int)displayedArea.Y, (uint)displayedArea.Width, (uint)displayedArea.Height);
        }

        public void Dispose()
        {
            ScreenEffect.Dispose();
            ColorTexture.Dispose();

            _screenQuadVao.Dispose();
            _screenQuadVbo.Dispose();
            _framebuffer.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}