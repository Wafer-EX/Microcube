using Silk.NET.Input;

namespace Microcube.Input
{
    /// <summary>
    /// Represents a Silk.NET keyboards abstractions with extra methods.
    /// </summary>
    public class KeyboardManager
    {
        private readonly IReadOnlyList<IKeyboard> _keyboards;
        private readonly List<Key> _pressedKeys;
        private readonly List<Key> _repeatedKeys;

        public KeyboardManager(IReadOnlyList<IKeyboard> keyboards)
        {
            ArgumentNullException.ThrowIfNull(keyboards, nameof(keyboards));
            this._keyboards = keyboards;

            _pressedKeys = [];
            _repeatedKeys = [];

            foreach (IKeyboard keyboard in keyboards)
            {
                keyboard.KeyDown += (IKeyboard keyboard, Key key, int keycode) => _pressedKeys.Add(key);
                keyboard.KeyUp += (IKeyboard keyboard, Key key, int keycode) =>
                {
                    _ = _pressedKeys.RemoveAll((Key removedKey) => removedKey == key);
                    _ = _repeatedKeys.RemoveAll((Key removedKey) => removedKey == key);
                };
            }
        }

        /// <summary>
        /// Shows is the key was clicked at the first time in the frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Is this key was clicked.</returns>
        public bool IsKeyClicked(Key key) => _pressedKeys.Contains(key) && !_repeatedKeys.Contains(key);

        /// <summary>
        /// Shows is the key pressed in the frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Is this key was pressed (holded).</returns>
        public bool IsKeyPressed(Key key) => _pressedKeys.Contains(key);

        /// <summary>
        /// Shows is the key was pressed not in the first time in the frame..
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Is this key was repeated.</returns>
        public bool IsKeyRepeated(Key key) => _repeatedKeys.Contains(key);

        /// <summary>
        /// Converts all key presses into game actions.
        /// </summary>
        /// <returns>Game actions.</returns>
        public IEnumerable<GameActionInfo> GetActions()
        {
            foreach (Key key in _pressedKeys)
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
            foreach (Key key in _pressedKeys)
            {
                foreach (IKeyboard keyboard in _keyboards)
                {
                    if (keyboard.IsKeyPressed(key))
                    {
                        _repeatedKeys.Add(key);
                    }
                }
            }
        }
    }
}