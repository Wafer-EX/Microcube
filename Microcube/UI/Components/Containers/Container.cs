using Microcube.Graphics.ColorModels;
using Microcube.Input;

namespace Microcube.UI.Components.Containers
{
    /// <summary>
    /// Represents a container that can change displayed area of child, intercept input and etc.
    /// It shouldn't store list of components, it's not layout, but it can store a few extra components.
    /// </summary>
    public abstract class Container : Component, IFocusable, IBackgrounded
    {
        private bool _isFocused;
        private Component? _child;

        public RgbaColor BackgroundColor { get; set; }

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                _isFocused = value;
                if (AutomaticallyFocusChild && _child is IFocusable focusable)
                    focusable.IsFocused = value;
            }
        }

        /// <summary>
        /// Focus child when this component is focused. Is useful to change displayed area but don't change
        /// behaviour of the chain.
        /// </summary>
        public bool AutomaticallyFocusChild { get; set; }

        public bool IsLastFocused => Child is IFocusable focusable && !focusable.IsFocused;

        /// <summary>
        /// Main child of the container.
        /// </summary>
        public Component? Child
        {
            get => _child;
            set
            {
                if (_child != null)
                    _child.Parent = null;

                if (value != null)
                    value.Parent = this;

                _child = value;
            }
        }

        public Container() : base()
        {
            BackgroundColor = RgbaColor.Transparent;
            AutomaticallyFocusChild = true;
        }

        public override void Update(float deltaTime) => Child?.Update(deltaTime);

        public virtual void Input(GameActionBatch actionBatch)
        {
            if (Child is IFocusable focusable)
            {
                if (actionBatch.IsIncludeClick(GameAction.Escape) && focusable.IsLastFocused)
                    focusable.IsFocused = false;
                else
                {
                    if (actionBatch.IsIncludeClick(GameAction.Enter) && !focusable.IsFocused)
                        focusable.IsFocused = true;
                    else
                        focusable.Input(actionBatch);
                }
            }
        }
    }
}