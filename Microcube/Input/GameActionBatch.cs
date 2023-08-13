namespace Microcube.Input
{
    public struct GameActionBatch
    {
        public IEnumerable<GameActionInfo> GameActions { get; set; }

        public GameActionBatch(IEnumerable<GameActionInfo> buttonGameActions) => GameActions = buttonGameActions;

        public readonly bool IsIncludeClick(GameAction gameAction)
        {
            foreach (GameActionInfo gameActionInfo in GameActions)
            {
                if (gameActionInfo.IsClicked && gameActionInfo.Action == gameAction)
                    return true;
            }
            return false;
        }
    }
}