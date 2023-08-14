namespace Microcube.Input
{
    /// <summary>
    /// Represents a batch of game actions.
    /// </summary>
    public struct GameActionBatch
    {
        /// <summary>
        /// Game action list with all info of these actions.
        /// </summary>
        public IEnumerable<GameActionInfo> GameActions { get; set; }

        public GameActionBatch(IEnumerable<GameActionInfo> buttonGameActions) => GameActions = buttonGameActions;

        /// <summary>
        /// Check is this batch include click of the action or not..
        /// </summary>
        /// <param name="gameAction">Specific game action that should be checked.</param>
        /// <returns>Is this action was clicked.</returns>
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