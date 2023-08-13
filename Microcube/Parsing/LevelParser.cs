using Microcube.Game.Blocks;
using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;
using System.Xml;

namespace Microcube.Parsing
{
    public record LevelContent(string Name, Block[] Blocks, MoveQueue[] MoveQueues, Vector3D<float> StartPosition);

    public static class LevelParser
    {
        // TODO: refactor this?
        public static IEnumerable<LevelInfo> GetLevels()
        {
            var levelList = new List<LevelInfo>();

            foreach (string levelPath in Directory.GetFiles("Resources/levels/"))
            {
                var document = new XmlDocument();
                document.Load(levelPath);

                XmlElement? root = document.DocumentElement;
                if (root?.HasChildNodes is true)
                {
                    XmlNodeList nameNodeList = root.GetElementsByTagName("name");
                    if (nameNodeList.Count > 0)
                    {
                        string name = "[Undefined]";
                        foreach (XmlNode nameNode in nameNodeList)
                        {
                            if (!string.IsNullOrEmpty(nameNode.InnerText))
                                name = nameNode.InnerText;
                        }

                        var levelInfo = new LevelInfo(name, levelPath);

                        if (levelList.Any())
                            levelList.Last().NextLevel = levelInfo;

                        levelList.Add(levelInfo);
                    }
                }
            }

            return levelList;
        }

        public static LevelContent Parse(string path)
        {
            var groundColor = new RgbaColor(1.0f, 1.0f, 1.0f, 1.0f);
            var triggerButtonColor = new RgbaColor(0.5f, 0.5f, 0.5f, 1.0f);
            var fallingPlateColor = new RgbaColor(0.5f, 0.5f, 0.5f, 1.0f);

            string levelName = "[Undefined]";
            var blocks = new List<Block>();
            var moveQueues = new Dictionary<string, MoveQueue>();
            var playerStartPosition = Vector3D<float>.Zero;

            var document = new XmlDocument();
            document.Load(path);

            XmlElement? root = document.DocumentElement;
            if (root?.HasChildNodes is true)
            {
                foreach (XmlNode childNode in root.ChildNodes)
                {
                    if (childNode.Name == "name")
                    {
                        levelName = childNode.InnerText;
                    }
                    else if (childNode.Name == "player")
                    {
                        XmlNode? startXAttribute = childNode.Attributes?.GetNamedItem("start-x");
                        XmlNode? startYAttribute = childNode.Attributes?.GetNamedItem("start-y");
                        XmlNode? startZAttribute = childNode.Attributes?.GetNamedItem("start-z");

                        if (startXAttribute?.Value != null && startYAttribute?.Value != null && startZAttribute?.Value != null)
                        {
                            float startX = float.Parse(startXAttribute.Value);
                            float startY = float.Parse(startYAttribute.Value);
                            float startZ = float.Parse(startZAttribute.Value);

                            playerStartPosition = new Vector3D<float>(startX, startY, startZ);
                        }
                    }
                    else if (childNode.Name == "move-queues")
                    {
                        foreach (XmlNode moveQueueNode in childNode.ChildNodes)
                        {
                            if (moveQueueNode.Name == "move-queue")
                            {
                                XmlNode? nameAttribute = moveQueueNode.Attributes?.GetNamedItem("name");
                                XmlNode? isRepeatableAttribute = moveQueueNode.Attributes?.GetNamedItem("is-repeatable");
                                XmlNode? isActiveAttribute = moveQueueNode.Attributes?.GetNamedItem("is-active");

                                if (nameAttribute?.Value != null && isRepeatableAttribute?.Value != null && isActiveAttribute?.Value != null)
                                {
                                    var movements = new List<Movement>();

                                    string name = nameAttribute.Value;
                                    bool isRepeatable = bool.Parse(isRepeatableAttribute.Value);
                                    bool isActive = bool.Parse(isActiveAttribute.Value);

                                    foreach (XmlNode movementNode in moveQueueNode.ChildNodes)
                                    {
                                        if (movementNode.Name == "movement")
                                        {
                                            XmlNode? xAttribute = movementNode.Attributes?.GetNamedItem("x");
                                            XmlNode? yAttribute = movementNode.Attributes?.GetNamedItem("y");
                                            XmlNode? zAttribute = movementNode.Attributes?.GetNamedItem("z");
                                            XmlNode? secondsAttribute = movementNode.Attributes?.GetNamedItem("seconds");

                                            if (xAttribute?.Value != null && yAttribute?.Value != null && zAttribute?.Value != null && secondsAttribute?.Value != null)
                                            {
                                                float x = float.Parse(xAttribute.Value);
                                                float y = float.Parse(yAttribute.Value);
                                                float z = float.Parse(zAttribute.Value);
                                                float seconds = float.Parse(secondsAttribute.Value);

                                                movements.Add(new Movement(x, y, z, seconds));
                                            }
                                        }
                                    }

                                    var moveQueue = new MoveQueue(movements.ToArray(), isRepeatable, isActive);
                                    moveQueues.Add(name, moveQueue);
                                }
                            }
                        }
                    }
                    else if (childNode.Name == "blocks")
                    {
                        foreach (XmlNode blockNode in childNode.ChildNodes)
                        {
                            XmlNode? xAttribute = blockNode.Attributes?.GetNamedItem("x");
                            XmlNode? yAttribute = blockNode.Attributes?.GetNamedItem("y");
                            XmlNode? zAttribute = blockNode.Attributes?.GetNamedItem("z");
                            XmlNode? moveQueueAttribute = blockNode.Attributes?.GetNamedItem("move-queue");

                            if (xAttribute?.Value != null && yAttribute?.Value != null && zAttribute?.Value != null)
                            {
                                float x = float.Parse(xAttribute.Value);
                                float y = float.Parse(yAttribute.Value);
                                float z = float.Parse(zAttribute.Value);

                                MoveQueue? moveQueue = null;
                                if (moveQueueAttribute?.Value != null)
                                    moveQueue = moveQueues[moveQueueAttribute.Value];

                                var position = new Vector3D<float>(x, y, z);
                                Block? block = blockNode.Name switch
                                {
                                    "ground" => new Ground(position, groundColor, moveQueue),
                                    "falling-plate" => new FallingPlate(position, fallingPlateColor),
                                    "prism" => new Prism(position),
                                    "trigger-button" => new TriggerButton(position, triggerButtonColor, moveQueue ?? throw new NullReferenceException()),
                                    _ => throw new InvalidOperationException()
                                };

                                blocks.Add(block);
                            }
                        }
                    }
                    else if (childNode.Name == "block-generators")
                    {
                        foreach (XmlNode generatorNode in childNode.ChildNodes)
                        {
                            if (generatorNode.Name == "generate-ground-plate")
                            {
                                XmlNode? startXAttribute = generatorNode.Attributes?.GetNamedItem("start-x");
                                XmlNode? startZAttribute = generatorNode.Attributes?.GetNamedItem("start-z");
                                XmlNode? endXAttribute = generatorNode.Attributes?.GetNamedItem("end-x");
                                XmlNode? endZAttribute = generatorNode.Attributes?.GetNamedItem("end-z");
                                XmlNode? heightAttribute = generatorNode.Attributes?.GetNamedItem("height");

                                if (startXAttribute?.Value != null && startZAttribute?.Value != null && endXAttribute?.Value != null && endZAttribute?.Value != null && heightAttribute?.Value != null)
                                {
                                    float startX = float.Parse(startXAttribute.Value);
                                    float startZ = float.Parse(startZAttribute.Value);
                                    float endX = float.Parse(endXAttribute.Value);
                                    float endZ = float.Parse(endZAttribute.Value);
                                    float height = float.Parse(heightAttribute.Value);

                                    var pointA = new Vector2D<float>(startX, startZ);
                                    var pointB = new Vector2D<float>(endX, endZ);

                                    blocks.AddRange(Ground.GeneratePlane(pointA, pointB, height, groundColor));
                                }
                            }
                            else if (generatorNode.Name == "generate-finish-plate")
                            {
                                XmlNode? positionXAttribute = generatorNode.Attributes?.GetNamedItem("x");
                                XmlNode? positionYAttribute = generatorNode.Attributes?.GetNamedItem("y");
                                XmlNode? positionZAttribute = generatorNode.Attributes?.GetNamedItem("z");

                                if (positionXAttribute?.Value != null && positionYAttribute?.Value != null && positionZAttribute?.Value != null)
                                {
                                    float positionX = float.Parse(positionXAttribute.Value);
                                    float positionY = float.Parse(positionYAttribute.Value);
                                    float positionZ = float.Parse(positionZAttribute.Value);

                                    var position = new Vector3D<float>(positionX, positionY, positionZ);

                                    blocks.AddRange(Finish.GenerateFinish(position, groundColor));
                                }
                            }
                        }
                    }
                }
            }

            return new LevelContent(levelName, blocks.ToArray(), moveQueues.Values.ToArray(), playerStartPosition);
        }
    }
}