using Microcube.Graphics.ColorModels;
using Microcube.Input;

namespace Microcube.UI.Components.Layouts
{
    /// <summary>
    /// Represents an component that can display a lot of child components.
    /// </summary>
    public abstract class Layout : Component, IFocusable, IBackgrounded
    {
        private IReadOnlyList<Component> _children = [];

        public RgbaColor BackgroundColor { get; set; }

        public abstract bool IsFocused { get; set; }

        public bool IsLastFocused
        {
            get
            {
                foreach (IFocusable focusable in FocusableChildren)
                {
                    if (focusable.IsFocused)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// All childs of this component.
        /// </summary>
        public virtual IReadOnlyList<Component?> Children
        {
            get => _children;
            set
            {
                foreach (Component child in _children)
                {
                    if (child.Parent == this)
                        child.Parent = null;
                }
                _children = value.Where(obj => obj != null).ToArray()!;

                var focusableChildList = new List<IFocusable>();
                foreach (Component child in _children)
                {
                    child.Parent = this;
                    if (child is IFocusable focusable)
                        focusableChildList.Add(focusable);
                }
                FocusableChildren = focusableChildList;
            }
        }

        /// <summary>
        /// All childs of this component that can be focused.
        /// </summary>
        protected IReadOnlyList<IFocusable> FocusableChildren { get; private set; }

        public Layout() : base()
        {
            Children = [];
            FocusableChildren = [];
        }


        /// <summary>
        /// Returns all sprites that the layout has as childs.
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(float deltaTime)
        {
            foreach (Component? child in Children)
                child?.Update(deltaTime);
        }

        public abstract void Input(GameActionBatch actionBatch);
    }
}