using Microcube.Input;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represents a component that can be focused (in lists and etc.) and intercept input.
    /// </summary>
    public interface IFocusable
    {
        /// <summary>
        /// Represents is the component focused, but only last component in the chain intercepts input.
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Represents is the last component in the chain that is focused or not.
        /// This property is useful to find out is the component should intercept input or not.
        /// </summary>
        public bool IsLastFocused { get; }

        /// <summary>
        /// The implementation can react to input actions, but it's not necessary.
        /// </summary>
        /// <param name="actionBatch">Input information</param>
        public void Input(GameActionBatch actionBatch);
    }
}