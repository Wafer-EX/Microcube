using Microcube.Graphics;
using Microcube.Graphics.ScreenEffects.Translations;
using Microcube.Input;

namespace Microcube.Scenes
{
    public class SceneManager : IDisposable
    {
        private readonly Viewport viewport;
        private Scene currentScene;
        private Scene? expectedScene;

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