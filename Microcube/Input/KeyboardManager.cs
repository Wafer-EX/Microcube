using Silk.NET.Input;

namespace Microcube.Input
{
    /// <summary>
    /// Represents a Silk.NET keyboards abstractions with extra methods.
    /// </summary>
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

        /// <summary>
        /// Shows is the key was clicked at the first time in the frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Is this key was clicked.</returns>
        public bool IsKeyClicked(Key key) => pressedKeys.Contains(key) && !repeatedKeys.Contains(key);

        /// <summary>
        /// Shows is the key pressed in the frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Is this key was pressed (holded).</returns>
        public bool IsKeyPressed(Key key) => pressedKeys.Contains(key);

        /// <summary>
        /// Shows is the key was pressed not in the first time in the frame..
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Is this key was repeated.</returns>
        public bool IsKeyRepeated(Key key) => repeatedKeys.Contains(key);

        /// <summary>
        /// Converts all key presses into game actions.
        /// </summary>
        /// <returns>Game actions.</returns>
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

        /// <summary>
        /// Updated info of pressed keys. Should be called at end.
        /// </summary>
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