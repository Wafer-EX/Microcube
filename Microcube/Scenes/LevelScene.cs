using Microcube.Graphics;
using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Renderers;
using Microcube.Graphics.ScreenEffects;
using Microcube.Input;
using Microcube.Parsing;
using Microcube.Playable;
using Microcube.UI.Components;
using Microcube.UI.Components.Containers;
using Microcube.UI.Components.Enums;
using Microcube.UI.Components.Layouts;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Microcube.Scenes
{
    /// <summary>
    /// Represents a scene that renders current level with all necessary UI.
    /// </summary>
    public class LevelScene : Scene
    {
        private bool isPaused = false;

        private readonly Level _level;
        private readonly Camera3D _camera3D;
        private readonly LevelRenderer _levelRenderer;
        private readonly RenderTarget _levelRenderTarget;
        private readonly ChromaticAberrationScreenEffect _levelScreenEffect;

        public LevelScene(GL gl, uint width, uint height, LevelInfo levelInfo) : base(gl, width, height)
        {
            ArgumentNullException.ThrowIfNull(levelInfo, nameof(levelInfo));
            LevelContent levelContent = LevelParser.Parse(levelInfo.Path);

            _level = new Level(levelContent.Blocks, levelContent.MoveQueues, levelContent.StartPosition);
            _camera3D = new Camera3D(Vector3.Zero, Vector3.Zero, 0.75f, (float)width / height, 5.0f);
            _levelRenderer = new LevelRenderer(gl);
            _levelScreenEffect = new ChromaticAberrationScreenEffect(gl);
            _levelRenderTarget = new RenderTarget(gl, width, height, _levelScreenEffect);

            TextComponent? prismCountTextComponent = null;
            CardLayout? cardLayout = null;

            UIContext.Child = cardLayout = new CardLayout()
            {
                IsFocused = true,
                Children =
                [
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
                                Text = $"Prisms: {_level.CollectedPrisms} of {_level.PrismCount}",
                                Font = DefaultFont,
                                Color = RgbaColor.White,
                                TextModifier = null,
                            },
                        },
                    },
                    new SizedContainer()
                    {
                        Size = new Vector2(256, 64),
                        AutomaticallyFocusChild = true,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Middle,
                        Child = new StackLayout()
                        {
                            Children =
                            [
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
                            ],
                        },
                    },
                    new SizedContainer()
                    {
                        Size = new Vector2(360, 256),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Middle,
                        BackgroundColor = new RgbaColor(0.0f, 0.0f, 0.0f, 0.5f),
                        Child = new WeightedStackLayout()
                        {
                            Weights =
                            [
                                0.80f,
                                0.20f
                            ],
                            Children =
                            [
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
                                    Children =
                                    [
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
                                    ],
                                },
                            ],
                        },
                    },
                ],
            };

            _level.PrismCollected += () =>
            {
                _levelScreenEffect.Strength = 8.0f;
                prismCountTextComponent.Text = $"Prisms: {_level.CollectedPrisms} of {_level.PrismCount}";
            };
            _level.Finished += () => cardLayout.SelectedIndex = 2;
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
                            _level.Player.Move(isReversed, changeAxis);
                        }
                    }
                }

                _level.Update(deltaTime);
                _camera3D.Target = _level.Player.OffsettedPosition;
                _camera3D.Position = _level.Player.OffsettedPosition + new Vector3(-5, 5, -5);
                _camera3D.Update(deltaTime);

                if (_levelScreenEffect.Strength > 0.0f)
                    _levelScreenEffect.Strength = MathF.Max(_levelScreenEffect.Strength - 16.0f * deltaTime, 0.0f);
            }

            _levelRenderer.SetData(_level);
            SpriteRenderer.SetData(UIContext.GetSprites());

            base.Update(actionBatch, deltaTime);
        }

        public override void Render(float deltaTime)
        {
            _levelRenderer.Render(_camera3D, _levelRenderTarget);
            _levelRenderTarget.Render(FinalRenderTarget.Framebuffer, 0, 0, FinalRenderTarget.Width, FinalRenderTarget.Height);
            SpriteRenderer.Render(SpriteCamera, FinalRenderTarget);
        }

        public override void Dispose()
        {
            base.Dispose();

            _levelRenderer.Dispose();

            _levelRenderTarget.Dispose();
            _levelScreenEffect.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}