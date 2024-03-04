using Microcube.Graphics;
using Microcube.Input;
using Microcube.Scenes;
using Silk.NET.Core.Contexts;
using Silk.NET.Input;

namespace Microcube
{
    public class MicrocubeGame : Game
    {
        public MicrocubeGame(IGLContextSource contextSource, IReadOnlyList<IKeyboard> keyboardList, int width, int height) : base(contextSource, keyboardList, width, height)
            => SceneManager.SetScene(new MainMenuScene(GL, 640, 360));

        public override void Update(float deltaTime)
        {
            SceneManager.Update(KeyboardManager.GetActionBatch(), deltaTime);
            KeyboardManager.Update();
        }

        public override void Render(float deltaTime) => SceneManager?.Render(deltaTime);

        public override void Resize(uint width, uint height)
        {
            Viewport.Width = width;
            Viewport.Height = height;
        }
    }
}