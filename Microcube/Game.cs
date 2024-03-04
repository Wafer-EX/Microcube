using Microcube.Graphics;
using Microcube.Input;
using Microcube.Scenes;
using Silk.NET.Core.Contexts;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace Microcube
{
    public abstract class Game : IDisposable
    {
        protected GL GL { get; set; }

        public KeyboardManager KeyboardManager { get; private set; }

        public SceneManager SceneManager { get; private set; }

        public Viewport Viewport { get; private set; }

        public Game(IGLContextSource contextSource, IReadOnlyList<IKeyboard> keyboardList, int width, int height)
        {
            // TODO: fix drawing with this
            //StbImage.stbi_set_flip_vertically_on_load(1);

            GL = GL.GetApi(contextSource);
            KeyboardManager = new KeyboardManager(keyboardList);
            Viewport = new Viewport(GL, (uint)width, (uint)height);
            SceneManager = new SceneManager(Viewport, null!);
        }

        public abstract void Update(float deltaTime);

        public abstract void Render(float deltaTime);

        public abstract void Resize(uint width, uint height);

        public virtual void Dispose()
        {
            SceneManager.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}