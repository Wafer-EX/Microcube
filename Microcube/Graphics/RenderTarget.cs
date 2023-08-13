using Microcube.Graphics.Abstractions;
using Microcube.Graphics.ScreenEffects;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Graphics
{
    public class RenderTarget : IDisposable
    {
        private readonly GL gl;

        private readonly VertexArrayObject screenQuadVao;
        private readonly BufferObject<float> screenQuadVbo;
        private readonly FramebufferObject framebuffer;

        public TextureObject ColorTexture { get; init; }

        public TextureObject DepthStencilTexture { get; init; }

        public ScreenEffect ScreenEffect { get; set; }

        public uint Width { get; init; }

        public uint Height { get; init; }

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

        public void Use()
        {
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer.Identifier);
            gl.Viewport(0, 0, Width, Height);
        }

        //public void Render(uint framebuffer, uint width, uint height, int x = 0, int y = 0)
        public void Render(uint framebuffer, int x, int y, uint width, uint height)
        {
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            gl.Viewport(x, y, width, height);

            screenQuadVao.Bind();
            ScreenEffect.Setup(ColorTexture);
            gl.DrawArrays(PrimitiveType.Triangles, 0, screenQuadVbo.Count);
        }

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