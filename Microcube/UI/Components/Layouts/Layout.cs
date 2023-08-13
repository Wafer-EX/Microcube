using Microcube.Graphics.ColorModels;
using Microcube.Input;

namespace Microcube.UI.Components.Layouts
{
    public abstract class Layout : Component, IFocusable, IBackgrounded
    {
        private IReadOnlyList<Component> childs = new List<Component>();

        public RgbaColor BackgroundColor { get; set; }

        public abstract bool IsFocused { get; set; }

        public bool IsLastFocused
        {
            get
            {
                foreach (IFocusable focusable in FocusableChilds)
                {
                    if (focusable.IsFocused)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public virtual IReadOnlyList<Component?> Childs
        {
            get => childs;
            set
            {
                foreach (Component child in childs)
                {
                    if (child.Parent == this)
                        child.Parent = null;
                }
                childs = value.Where(obj => obj != null).ToArray()!;

                var focusableChildList = new List<IFocusable>();
                foreach (Component child in childs)
                {
                    child.Parent = this;
                    if (child is IFocusable focusable)
                        focusableChildList.Add(focusable);
                }
                FocusableChilds = focusableChildList;
            }
        }

        protected IReadOnlyList<IFocusable> FocusableChilds { get; private set; }

        public Layout() : base()
        {
            Childs = new List<Component>();
            FocusableChilds = new List<IFocusable>();
        }

        public override void Update(float deltaTime)
        {
            foreach (Component? child in Childs)
                child?.Update(deltaTime);
        }

        public abstract void Input(GameActionBatch actionBatch);
    }
}