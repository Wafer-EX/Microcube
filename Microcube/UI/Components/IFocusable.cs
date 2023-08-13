using Microcube.Input;

namespace Microcube.UI.Components
{
    public interface IFocusable
    {
        public bool IsFocused { get; set; }

        public bool IsLastFocused { get; }

        public void Input(GameActionBatch actionBatch);
    }
}