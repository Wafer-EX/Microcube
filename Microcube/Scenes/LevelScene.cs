using Microcube.Game;
using Microcube.Graphics;
using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Renderers;
using Microcube.Graphics.ScreenEffects;
using Microcube.Input;
using Microcube.Parsing;
using Microcube.UI.Components;
using Microcube.UI.Components.Containers;
using Microcube.UI.Components.Enums;
using Microcube.UI.Components.Layouts;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Microcube.Scenes
{
    public class LevelScene : Scene
    {
        private bool isPaused = false;

        private readonly Level level;
        private readonly Camera3D camera3D;
        private readonly LevelRenderer levelRenderer;
        private readonly RenderTarget levelRenderTarget;
        private readonly ChromaticAberrationScreenEffect levelScreenEffect;

        public LevelScene(GL gl, uint width, uint height, LevelInfo levelInfo) : base(gl, width, height)
        {
            ArgumentNullException.ThrowIfNull(levelInfo, nameof(levelInfo));
            LevelContent levelContent = LevelParser.Parse(levelInfo.Path);

            level = new Level(levelContent.Blocks, levelContent.MoveQueues, levelContent.StartPosition);
            camera3D = new Camera3D(Vector3D<float>.Zero, Vector3D<float>.Zero, 0.75f, (float)width / height, 5.0f);
            levelRenderer = new LevelRenderer(gl);
            levelScreenEffect = new ChromaticAberrationScreenEffect(gl);
            levelRenderTarget = new RenderTarget(gl, width, height, levelScreenEffect);

            TextComponent? prismCountTextComponent = null;
            CardLayout? cardLayout = null;

            UIContext.Child = cardLayout = new CardLayout()
            {
                IsFocused = true,
                Childs = new List<Component?>()
                {
                    new InputInterceptorContainer()
                    {
                        OnInterception = actionBatch =>
                        {
                            if (actionBatch.IsIncludeClick(GameAction.Escape))
                            {
                                if (cardLayout != null)
                                {
                                    cardLayout.SelectedIndex = 1;
                                    isPaused = true;
                                }
                                return true;
                            }
                            return false;
                        },
                        Child = new PaddingContainer()
                        {
                            PaddingLeft = 8,
                            PaddingRight = 8,
                            PaddingTop = 8,
                            PaddingBottom = 8,
                            Child = prismCountTextComponent = new TextComponent()
                            {
                                Text = $"Prisms: {level.CollectedPrisms} of {level.PrismCount}",
                                Font = DefaultFont,
                                Color = RgbaColor.White,
                                TextModifier = null,
                            },
                        },
                    },
                    new SizedContainer()
                    {
                        Size = new Vector2D<float>(256, 64),
                        AutomaticallyFocusChild = true,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Middle,
                        Child = new StackLayout()
                        {
                            Childs = new List<Component?>()
                            {
                                new ButtonComponent()
                                {
                                    Text = "Resume",
                                    Font = DefaultFont,
                                    OnClick = () =>
                                    {
                                        if (cardLayout != null)
                                        {
                                            cardLayout.SelectedIndex = 0;
                                            isPaused = false;
                                        }
                                    }
                                },
                                new ButtonComponent()
                                {
                                    Text = "Main menu",
                                    Font = DefaultFont,
                                    OnClick= () => SceneManager?.SetScene(new MainMenuScene(gl, width, height))
                                },
                            },
                        },
                    },
                    new SizedContainer()
                    {
                        Size = new Vector2D<float>(360, 256),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Middle,
                        BackgroundColor = new RgbaColor(0.0f, 0.0f, 0.0f, 0.5f),
                        Child = new WeightedStackLayout()
                        {
                            Weights = new float[]
                            {
                                0.80f,
                                0.20f
                            },
                            Childs = new List<Component?>()
                            {
                                new TextComponent()
                                {
                                    Text = "The level was completed!",
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Middle,

                                    Font = DefaultFont,
                                    Color = RgbaColor.White,
                                    TextModifier = null
                                },
                                new StackLayout()
                                {
                                    Orientation = StackLayoutOrientation.Horizontal,
                                    Childs = new List<Component?>()
                                    {
                                        levelInfo.NextLevel != null ? new ButtonComponent()
                                        {
                                            Text = "Next level",
                                            Font = DefaultFont,
                                            OnClick = () => SceneManager?.SetScene(new LevelScene(gl, width, height, levelInfo.NextLevel))
                                        } : null,
                                        new ButtonComponent()
                                        {
                                            Text = "Main menu",
                                            Font = DefaultFont,
                                            OnClick = () => SceneManager?.SetScene(new MainMenuScene(gl, width, height)),
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            level.PrismCollected += () =>
            {
                levelScreenEffect.Strength = 8.0f;
                prismCountTextComponent.Text = $"Prisms: {level.CollectedPrisms} of {level.PrismCount}";
            };
            level.Finished += () => cardLayout.SelectedIndex = 2;
        }

        public override void Update(GameActionBatch actionBatch, float deltaTime)
        {
            if (!isPaused)
            {
                foreach (GameActionInfo gameAction in actionBatch.GameActions)
                {
                    if (gameAction.Action == GameAction.Up || gameAction.Action == GameAction.Down || gameAction.Action == GameAction.Left || gameAction.Action == GameAction.Right)
                    {
                        if (gameAction.IsPressed)
                        {
                            bool isReversed = gameAction.Action == GameAction.Down || gameAction.Action == GameAction.Right;
                            bool changeAxis = gameAction.Action == GameAction.Left || gameAction.Action == GameAction.Right;
                            level.Player.Move(isReversed, changeAxis);
                        }
                    }
                }

                level.Update(deltaTime);
                camera3D.Target = level.Player.OffsettedPosition;
                camera3D.Position = level.Player.OffsettedPosition + new Vector3D<float>(-5, 5, -5);
                camera3D.Update(deltaTime);

                if (levelScreenEffect.Strength > 0.0f)
                    levelScreenEffect.Strength = MathF.Max(levelScreenEffect.Strength - 16.0f * deltaTime, 0.0f);
            }

            levelRenderer.SetData(level);
            SpriteRenderer.SetData(UIContext.GetSprites());

            base.Update(actionBatch, deltaTime);
        }

        public override void Render(float deltaTime)
        {
            levelRenderer.Render(camera3D, levelRenderTarget);
            levelRenderTarget.Render(FinalRenderTarget.Framebuffer, 0, 0, FinalRenderTarget.Width, FinalRenderTarget.Height);
            SpriteRenderer.Render(SpriteCamera, FinalRenderTarget);
        }

        public override void Dispose()
        {
            base.Dispose();

            levelRenderer.Dispose();

            levelRenderTarget.Dispose();
            levelScreenEffect.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}