namespace Microcube.Input
{
    /// <summary>
    /// Represents an info about concrete game action.
    /// </summary>
    public struct GameActionInfo(GameAction action, bool isClicked, bool isPressed, bool isRepeated)
    {
        /// <summary>
        /// Concrete game action.
        /// </summary>
        public GameAction Action { get; set; } = action;

        /// <summary>
        /// Is the key was clicked.
        /// </summary>
        public bool IsClicked { get; set; } = isClicked;

        /// <summary>
        /// Is the key was pressed.
        /// </summary>
        public bool IsPressed { get; set; } = isPressed;

        /// <summary>
        /// Is the key was repeated.
        /// </summary>
        public bool IsRepeated { get; set; } = isRepeated;
    }
}