using Microcube.Graphics.ColorModels;
using Microcube.Input;

namespace Microcube.UI.Components.Containers
{
    public abstract class Container : Component, IFocusable, IBackgrounded
    {
        private bool isFocused;
        private Component? child;

        public RgbaColor BackgroundColor { get; set; }

        public bool IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;
                if (AutomaticallyFocusChild && child is IFocusable focusable)
                    focusable.IsFocused = value;
            }
        }

        public bool AutomaticallyFocusChild { get; set; }

        public bool IsLastFocused => Child is IFocusable focusable && !focusable.IsFocused;

        public Component? Child
        {
            get => child;
            set
            {
                if (child != null)
                    child.Parent = null;

                if (value != null)
                    value.Parent = this;

                child = value;
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