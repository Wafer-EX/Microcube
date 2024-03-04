namespace Microcube.Parsing
{
    /// <summary>
    /// Just a level info, name, path, next level and that's all.
    /// </summary>
    public class LevelInfo(string name, string path, LevelInfo? nextLevel = null)
    {
        /// <summary>
        /// Name of the level.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Path to the level xml file.
        /// </summary>
        public string Path { get; set; } = path;

        /// <summary>
        /// Info about next level.
        /// </summary>
        public LevelInfo? NextLevel { get; set; } = nextLevel;
    }
}