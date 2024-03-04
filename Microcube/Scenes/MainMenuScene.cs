using Microcube.Graphics.ColorModels;
using Microcube.Graphics.Enums;
using Microcube.Graphics.Raster;
using Microcube.Graphics.Renderers;
using Microcube.Input;
using Microcube.Parsing;
using Microcube.UI.Components;
using Microcube.UI.Components.Containers;
using Microcube.UI.Components.Layouts;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Microcube.Scenes
{
    /// <summary>
    /// Represents a scene that renders main menu with level list.
    /// </summary>
    public class MainMenuScene : Scene
    {
        public MainMenuScene(GL gl, uint width, uint height) : base(gl, width, height)
        {
            var logoSprite = new Sprite(gl, "Resources/textures/logo.png")
            {
                Scale = 4.0f,
            };

            CardLayout? cardLayout = null;
            SpriteComponent spriteComponent = new()
            {
                IsFitToDisplayedArea = false,
                Sprite = logoSprite,
            };

            SpriteRenderer.ClearColor = RgbaColor.Black;
            SpriteRenderer.IsClearBackground = true;

            UIContext.Child = cardLayout = new CardLayout()
            {
                IsFocused = true,
                Children =
                [
                    new OverlayContainer()
                    {
                        Child = new WeightedStackLayout()
                        {
                            Weights =
                            [
                                0.4f,
                                0.6f,
                            ],
                            Children =
                            [
                                spriteComponent,
                                new SizedContainer()
                                {
                                    Size = new Vector2(256, 64),
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Middle,
                                    Child = new StackLayout()
                                    {
                                        Children =
                                        [
                                            new ButtonComponent()
                                            {
                                                Text = "Play",
                                                Font = DefaultFont,
                                                OnClick = () =>
                                                {
                                                    if (cardLayout != null)
                                                        cardLayout.SelectedIndex = 1;
                                                },
                                            },
                                            new ButtonComponent()
                                            {
                                                Text = "Exit",
                                                Font = DefaultFont,
                                                OnClick = () => Environment.Exit(0),
                                            },
                                        ],
                                    },
                                },
                            ]
                        },
                        OverlayComponent = new PaddingContainer()
                        {
                            PaddingLeft = 8,
                            PaddingRight = 8,
                            PaddingTop = 8,
                            PaddingBottom = 8,
                            Child = new TextComponent()
                            {
                                Text = "Wafer EX",
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Bottom,

                                Font = DefaultFont,
                                Color = RgbaColor.White,
                                TextModifier = null
                            },
                        },
                    },
                    new InputInterceptorContainer()
                    {
                        OnInterception = actionBatch =>
                        {
                            foreach (GameActionInfo gameAction in actionBatch.GameActions)
                            {
                                if (gameAction.IsClicked && gameAction.Action == GameAction.Escape)
                                {
                                    if (cardLayout != null)
                                        cardLayout.SelectedIndex = 0;

                                    return true;
                                }
                            }
                            return false;
                        },
                        Child = new SizedContainer()
                        {
                            Size = new Vector2(256, 128),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Middle,
                            Child = new LimitedStackLayout()
                            {
                                DisplayedCount = 4,
                                Children = new List<Component?>(GetLevelButtons()),
                            },
                        },
                    },
                ],
            };
        }

        public override void Update(GameActionBatch actionBatch, float deltaTime)
        {
            SpriteRenderer.SetData(UIContext.GetSprites());
            base.Update(actionBatch, deltaTime);
        }

        public override void Render(float deltaTime)
        {
            SpriteRenderer.Render(SpriteCamera, FinalRenderTarget);
        }

        private IEnumerable<ButtonComponent> GetLevelButtons()
        {
            LevelInfo[] levels = LevelParser.GetLevels().ToArray();

            foreach (LevelInfo levelInfo in levels)
            {
                yield return new ButtonComponent()
                {
                    Text = levelInfo.Name,
                    Font = DefaultFont,
                    OnClick = () => SceneManager?.SetScene(new LevelScene(GL, Width, Height, levelInfo)),
                };
            }
        }
    }
}