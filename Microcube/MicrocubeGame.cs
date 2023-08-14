using Microcube.Graphics;
using Microcube.Input;
using Microcube.Scenes;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Microcube
{
    /// <summary>
    /// Main class of the game.
    /// </summary>
    public class MicrocubeGame : IDisposable
    {
        private readonly IWindow window;

        private IInputContext? inputContext;
        private GL? gl;

        private SceneManager? sceneManager;
        private KeyboardManager? keyboardManager;
        private Viewport? viewport;

        public MicrocubeGame(int width, int height)
        {
            window = Window.Create(WindowOptions.Default with
            {
                API = new GraphicsAPI(ContextAPI.OpenGL, new APIVersion(4, 6)),
                Size = new Vector2D<int>(width, height),
                Title = "Microcube",
            });

            window.Load += () =>
            {
                gl = window.CreateOpenGL() ?? throw new NotSupportedException();
                inputContext = window.CreateInput() ?? throw new NotSupportedException();

                keyboardManager = new KeyboardManager(inputContext.Keyboards);
                viewport = new Viewport(gl, (uint)window.Size.X, (uint)window.Size.Y);
                sceneManager = new SceneManager(viewport, new MainMenuScene(gl, 640, 360));
            };

            window.Update += (deltaTime) =>
            {
                if (sceneManager != null && keyboardManager != null)
                {
                    var actionBatch = new GameActionBatch(keyboardManager.GetActions());

                    sceneManager.Update(actionBatch, (float)deltaTime);
                    keyboardManager.Update();
                }
            };

            window.Render += (deltaTime) =>
            {
                sceneManager?.Render((float)deltaTime);
            };

            window.Resize += (Vector2D<int> size) =>
            {
                if (viewport != null)
                {
                    viewport.Width = (uint)size.X;
                    viewport.Height = (uint)size.Y;
                }
            };
        }

        public void Run() => window.Run();

        public void Dispose()
        {
            window.Dispose();
            inputContext?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}