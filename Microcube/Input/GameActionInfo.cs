namespace Microcube.Input
{
    public struct GameActionInfo
    {
        public GameAction Action { get; set; }

        public bool IsClicked { get; set; }

        public bool IsPressed { get; set; }

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