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
        private readonly IWindow _window;

        private GL _gl = null!;
        private IInputContext _inputContext = null!;

        private SceneManager _sceneManager = null!;
        private KeyboardManager _keyboardManager = null!;
        private Viewport _viewport = null!;

        public MicrocubeGame(int width, int height)
        {
            _window = Window.Create(WindowOptions.Default with
            {
                API = new GraphicsAPI(ContextAPI.OpenGL, new APIVersion(4, 6)),
                Size = new Vector2D<int>(width, height),
                Title = "Microcube",
            });

            _window.Load += () =>
            {
                _gl = _window.CreateOpenGL();
                _inputContext = _window.CreateInput();

                _keyboardManager = new KeyboardManager(_inputContext.Keyboards);
                _viewport = new Viewport(_gl, (uint)_window.Size.X, (uint)_window.Size.Y);
                _sceneManager = new SceneManager(_viewport, new MainMenuScene(_gl, 640, 360));
            };

            _window.Update += (deltaTime) =>
            {
                if (_sceneManager != null && _keyboardManager != null)
                {
                    var actionBatch = new GameActionBatch(_keyboardManager.GetActions());

                    _sceneManager.Update(actionBatch, (float)deltaTime);
                    _keyboardManager.Update();
                }
            };

            _window.Render += (deltaTime) => _sceneManager?.Render((float)deltaTime);

            _window.Resize += (Vector2D<int> size) =>
            {
                if (_viewport != null)
                {
                    _viewport.Width = (uint)size.X;
                    _viewport.Height = (uint)size.Y;
                }
            };
        }

        public void Run() => _window.Run();

        public void Dispose()
        {
            _window.Dispose();
            _inputContext?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}