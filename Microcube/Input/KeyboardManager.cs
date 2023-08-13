using Silk.NET.Input;

namespace Microcube.Input
{
    public class KeyboardManager
    {
        private readonly IReadOnlyList<IKeyboard> keyboards;
        private readonly List<Key> pressedKeys;
        private readonly List<Key> repeatedKeys;

        public KeyboardManager(IReadOnlyList<IKeyboard> keyboards)
        {
            ArgumentNullException.ThrowIfNull(keyboards, nameof(keyboards));
            this.keyboards = keyboards;

            pressedKeys = new List<Key>();
            repeatedKeys = new List<Key>();

            foreach (IKeyboard keyboard in keyboards)
            {
                keyboard.KeyDown += (IKeyboard keyboard, Key key, int keycode) => pressedKeys.Add(key);
                keyboard.KeyUp += (IKeyboard keyboard, Key key, int keycode) =>
                {
                    _ = pressedKeys.RemoveAll((Key removedKey) => removedKey == key);
                    _ = repeatedKeys.RemoveAll((Key removedKey) => removedKey == key);
                };
            }
        }

        public bool IsKeyClicked(Key key) => pressedKeys.Contains(key) && !repeatedKeys.Contains(key);

        public bool IsKeyPressed(Key key) => pressedKeys.Contains(key);

        public bool IsKeyRepeated(Key key) => repeatedKeys.Contains(key);

        public IEnumerable<GameActionInfo> GetActions()
        {
            foreach (Key key in pressedKeys)
            {
                // TODO: key mapping in xml
                GameAction? gameAction = key switch
                {
                    Key.Up => GameAction.Up,
                    Key.Down => GameAction.Down,
                    Key.Left => GameAction.Left,
                    Key.Right => GameAction.Right,
                    Key.Enter => GameAction.Enter,
                    Key.Escape => GameAction.Escape,
                    _ => null
                };

                if (gameAction.HasValue)
                {
                    yield return new GameActionInfo()
                    {
                        Action = gameAction.Value,
                        IsClicked = IsKeyClicked(key),
                        IsPressed = IsKeyPressed(key),
                        IsRepeated = IsKeyRepeated(key),
                    };
                }
            }
        }

        // Should be called after all
        public void Update()
        {
            foreach (Key key in pressedKeys)
            {
                foreach (IKeyboard keyboard in keyboards)
                {
                    if (keyboard.IsKeyPressed(key))
                    {
                        repeatedKeys.Add(key);
                    }
                }
            }
        }
    }
}