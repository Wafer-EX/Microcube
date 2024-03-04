using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Microcube
{
    public class Program
    {
        private static Game _game = null!;
        private static IWindow? _window;

        private static readonly int _windowWidth = 1280;
        private static readonly int _windowHeight = 720;

        private static void Main()
        {
            _window = Window.Create(WindowOptions.Default with
            {
                Size = new Vector2D<int>(_windowWidth, _windowHeight),
                Title = "Platformer"
            });

            _window.Load += () =>
            {
                IInputContext? inputContext = _window.CreateInput();
                _game = new MicrocubeGame(_window, inputContext.Keyboards, _windowWidth, _windowHeight);
            };

            _window.Update += deltaTime => _game.Update((float)deltaTime);
            _window.Render += deltaTime => _game.Render((float)deltaTime);
            _window.Resize += size => _game.Resize((uint)size.X, (uint)size.Y);
            _window.Closing += () => _game.Dispose();

            _window.Run();
            _window.Dispose();
        }
    }
}