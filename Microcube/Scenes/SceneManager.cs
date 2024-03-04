using Microcube.Graphics;
using Microcube.Graphics.ScreenEffects.Translations;
using Microcube.Input;
using System.Drawing;

namespace Microcube.Scenes
{
    /// <summary>
    /// Represents a scene manager that allows you to switch scenes with translations.
    /// </summary>
    public class SceneManager : IDisposable
    {
        private readonly Viewport _viewport;
        private Scene? _currentScene;
        private Scene? _expectedScene;

        /// <summary>
        /// Translation that will be started between scenes.
        /// </summary>
        public Translation Translation { get; set; }

        public SceneManager(Viewport viewport, Scene? initialScene = null)
        {
            ArgumentNullException.ThrowIfNull(viewport, nameof(viewport));

            Translation = new DefaultTranslation(viewport.GLContext);

            _viewport = viewport;
            _currentScene = initialScene;

            if (_currentScene != null)
                _currentScene.SceneManager = this;
        }

        /// <summary>
        /// Sets scene to the specific and starts translation.
        /// </summary>
        /// <param name="nextScene">Next scene that should be showed.</param>
        public void SetScene(Scene nextScene)
        {
            ArgumentNullException.ThrowIfNull(nextScene, nameof(nextScene));

            if (nextScene != _currentScene)
            {
                _expectedScene = nextScene;
                _expectedScene.SceneManager = this;
                Translation.IsEnabled = true;
            }
        }

        /// <summary>
        /// Updates the translation and current scene.
        /// </summary>
        /// <param name="actionBatch">A set of game actions.</param>
        /// <param name="deltaTime">Time of the frame.</param>
        public void Update(GameActionBatch actionBatch, float deltaTime)
        {
            Translation.Update(deltaTime);

            if (Translation.IsIntersectedCenter && _expectedScene != null)
            {
                _currentScene?.Dispose();
                _currentScene = _expectedScene;
            }

            _currentScene?.Update(actionBatch, deltaTime);
        }

        /// <summary>
        /// Renders current scene to final viewport, i.e. window.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
        public void Render(float deltaTime)
        {
            if (_currentScene != null)
            {
                _currentScene.Render(deltaTime);

                RectangleF displayedArea = _viewport.FitToCenter(_currentScene.Width, _currentScene.Height);
                _currentScene.FinalRenderTarget.ScreenEffect = Translation;
                _currentScene.FinalRenderTarget.Render(0, displayedArea);
            }
        }

        public void Dispose()
        {
            _currentScene?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}