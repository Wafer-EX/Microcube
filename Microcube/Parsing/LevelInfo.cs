namespace Microcube.Parsing
{
    /// <summary>
    /// Just a level info, name, path, next level and that's all.
    /// </summary>
    public class LevelInfo
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public LevelInfo? NextLevel { get; set; }

        public LevelInfo(string name, string path, LevelInfo? nextLevel = null)
        {
            Name = name;
            Path = path;
            NextLevel = nextLevel;
        }
    }
}