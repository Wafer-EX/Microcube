using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ScreenEffects;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics
{
    /// <summary>
    /// Abstraction on OpenGL frame buffer, can be used to render there something like in texture.
    /// </summary>
    public class RenderTarget : IDisposable
    {
        private readonly GL gl;

        private readonly VertexArrayObject screenQuadVao;
        private readonly BufferObject<float> screenQuadVbo;
        private readonly FramebufferObject framebuffer;

        /// <summary>
        /// Color texture that represents colors of vertices.
        /// </summary>
        public TextureObject ColorTexture { get; init; }

        /// <summary>
        /// Combine depth and stencil textures in the same object.
        /// </summary>
        public TextureObject DepthStencilTexture { get; init; }

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
        public uint Framebuffer => framebuffer.Identifier;

        public RenderTarget(GL gl, uint width, uint height, ScreenEffect? screenEffect = null)
        {
            ArgumentNullException.ThrowIfNull(gl, nameof(gl));
            this.gl = gl;

            Width = width;
            Height = height;
            ScreenEffect = screenEffect ?? new DefaultScreenEffect(gl);

            ColorTexture = new TextureObject(gl, width, height, InternalFormat.Rgb, PixelFormat.Rgb, PixelType.UnsignedByte);
            ColorTexture.SetParameter(TextureParameterName.TextureWrapS, GLEnum.ClampToEdge);
            ColorTexture.SetParameter(TextureParameterName.TextureWrapT, GLEnum.ClampToEdge);
            ColorTexture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            ColorTexture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            DepthStencilTexture = new TextureObject(gl, width, height, InternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedInt248);
            DepthStencilTexture.SetParameter(TextureParameterName.TextureMinFilter, GLEnum.Nearest);
            DepthStencilTexture.SetParameter(TextureParameterName.TextureMagFilter, GLEnum.Nearest);

            framebuffer = new FramebufferObject(gl);
            framebuffer.AttachTexture(ColorTexture, FramebufferAttachment.ColorAttachment0);
            framebuffer.AttachTexture(DepthStencilTexture, FramebufferAttachment.DepthStencilAttachment);

            screenQuadVao = new VertexArrayObject(gl);
            screenQuadVbo = new BufferObject<float>(gl, BufferTargetARB.ArrayBuffer, new float[]
            {
                // ---- SCREEN QUAD ----
                // 2x POSITIONS | 2x UVs
                -1.0f, -1.0f, 0.0f, 0.0f,
                 1.0f, -1.0f, 1.0f, 0.0f,
                 1.0f,  1.0f, 1.0f, 1.0f,
                -1.0f, -1.0f, 0.0f, 0.0f,
                 1.0f,  1.0f, 1.0f, 1.0f,
                -1.0f,  1.0f, 0.0f, 1.0f,
            });

            screenQuadVao.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            screenQuadVao.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);
        }

        /// <summary>
        /// Use this render target to render there something.
        /// </summary>
        public void Use()
        {
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer.Identifier);
            gl.Viewport(0, 0, Width, Height);
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
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            gl.Viewport(x, y, width, height);

            screenQuadVao.Bind();
            ScreenEffect.Setup(ColorTexture);
            gl.DrawArrays(PrimitiveType.Triangles, 0, screenQuadVbo.Count);
        }

        /// <summary>
        /// Render this render target to specific frame buffer. Lets to draw render targets to each other.
        /// </summary>
        /// <param name="frameBuffer">Specific frame buffer.</param>
        /// <param name="displayedArea">Displayed area in the viewport.</param>
        public void Render(uint frameBuffer, Rectangle<float> displayedArea)
        {
            Render(frameBuffer, (int)displayedArea.Origin.X, (int)displayedArea.Origin.Y, (uint)displayedArea.Size.X, (uint)displayedArea.Size.Y);
        }

        public void Dispose()
        {
            ScreenEffect.Dispose();
            ColorTexture.Dispose();

            screenQuadVao.Dispose();
            screenQuadVbo.Dispose();
            framebuffer.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}