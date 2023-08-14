using Microcube.Graphics;
using Microcube.Graphics.ScreenEffects.Translations;
using Microcube.Input;

namespace Microcube.Scenes
{
    /// <summary>
    /// Represents a scene manager that allows you to switch scenes with translations.
    /// </summary>
    public class SceneManager : IDisposable
    {
        private readonly Viewport viewport;
        private Scene currentScene;
        private Scene? expectedScene;

        /// <summary>
        /// Translation that will be started between scenes.
        /// </summary>
        public Translation Translation { get; set; }

        public SceneManager(Viewport viewport, Scene initialScene)
        {
            ArgumentNullException.ThrowIfNull(viewport, nameof(viewport));
            ArgumentNullException.ThrowIfNull(initialScene, nameof(initialScene));

            Translation = new DefaultTranslation(viewport.GLContext);

            this.viewport = viewport;
            currentScene = initialScene;
            currentScene.SceneManager = this;
        }

        /// <summary>
        /// Sets scene to the specific and starts translation.
        /// </summary>
        /// <param name="nextScene">Next scene that should be showed.</param>
        public void SetScene(Scene nextScene)
        {
            ArgumentNullException.ThrowIfNull(nextScene, nameof(nextScene));

            if (nextScene != currentScene)
            {
                expectedScene = nextScene;
                expectedScene.SceneManager = this;
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
            if (Translation.IsIntersectedCenter && expectedScene != null)
            {
                currentScene.Dispose();
                currentScene = expectedScene;
            }

            currentScene.Update(actionBatch, deltaTime);
        }

        /// <summary>
        /// Renders current scene to final viewport, i.e. window.
        /// </summary>
        /// <param name="deltaTime">Time of the frame.</param>
        public void Render(float deltaTime)
        {
            currentScene.Render(deltaTime);

            var displayedArea = viewport.FitToCenter(currentScene.Width, currentScene.Height);
            currentScene.FinalRenderTarget.ScreenEffect = Translation;
            currentScene.FinalRenderTarget.Render(0, displayedArea);
        }

        public void Dispose()
        {
            currentScene.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}