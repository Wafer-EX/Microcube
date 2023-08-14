namespace Microcube.Input
{
    /// <summary>
    /// Represents an info about concrete game action.
    /// </summary>
    public struct GameActionInfo
    {
        /// <summary>
        /// Concrete game action.
        /// </summary>
        public GameAction Action { get; set; }

        /// <summary>
        /// Is the key was clicked.
        /// </summary>
        public bool IsClicked { get; set; }

        /// <summary>
        /// Is the key was pressed.
        /// </summary>
        public bool IsPressed { get; set; }

        /// <summary>
        /// Is the key was repeated.
        /// </summary>
        public bool IsRepeated { get; set; }

        public GameActionInfo(GameAction action, bool isClicked, bool isPressed, bool isRepeated)
        {
            Action = action;
            IsClicked = isClicked;
            IsPressed = isPressed;
            IsRepeated = isRepeated;
        }
    }
}