# Microcube
It's a simple game where you roll the cube and may collect prisms to finish the level. It's a clone of EDGE game by Mobigame. It's made by OpenGL (Silk.NET bindings) from scratch.

### What's the goal?
I always wanted to know how people make games on low-level and how the graphics works, and this my personal project is opened these mysteries to me. I wanted to make the graphics beautiful, but I got I can't done relatively big projects... The game is not pretend to be a real game like people really play and have some fun, it include basic implementation of common things and was planned as example for me, how I can make a game from scratch, and to get some experience.

### Implemented basic things:
- Levels
- Sprites
- Texture atlases (include fonts)
- UI system
- Translations
### Screenshots

| Main menu | Level selection |
| - | - |
| The play button just shows next screen (level selection) and the Exit button closes app. | Include level list that is parsed from `Resources/levels` folder with their names. When button is clicked, the level opens. |
| ![image](https://github.com/Wafer-EX/MicrocubeDemo/assets/76843479/e598f2ea-27bf-4874-8296-948c699f16fc) | ![image](https://github.com/Wafer-EX/MicrocubeDemo/assets/76843479/a9fad770-f6eb-4424-a1f4-15bd49e71426) |

| Game (with chromatic aberration) | Win screen |
| - | - |
| When you collect prisms, counter increases on 1 and chromatic aberration effect happens. If you step on the finish, you will win. | If the level is not last, the button "Next level" is displayed. |
| ![image](https://github.com/Wafer-EX/MicrocubeDemo/assets/76843479/884cd368-7a5e-44f1-811a-80bfe9fb66b0) | ![image](https://github.com/Wafer-EX/MicrocubeDemo/assets/76843479/89e2b6f1-e5e2-4b64-b902-23dd6f8f7352) |

## System requirements
- .NET 8.0
- OpenGL 3.3 (at least I strive for this)

## Third-party libraries
| Name | Link |
| - | - |
| Silk.NET | [GitHub](https://github.com/dotnet/Silk.NET) |
| StbImageSharp | [Github](https://github.com/StbSharp/StbImageSharp) |

## License
This game doesn't has any licence, i.e. all rights are reserved. But this code is trivial and was written by someone many times I guess, so you don't violate something when you write code like this, just don't copy and upload this game to resources like itch.io. I.e. formally... Copyright 2023-2024 Wafer EX. All rights reserved.
