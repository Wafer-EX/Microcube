using Microcube.Graphics.ColorModels;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represents a component that has a colored background.
    /// </summary>
    public interface IBackgrounded
    {
        /// <summary>
        /// Color to fill background of the component.
        /// </summary>
        public RgbaColor BackgroundColor { get; set; }
    }
}