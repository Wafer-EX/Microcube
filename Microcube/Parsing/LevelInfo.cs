namespace Microcube.Parsing
{
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