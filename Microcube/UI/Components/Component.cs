using Microcube.Graphics.Raster;
using System.Drawing;

namespace Microcube.UI.Components
{
    /// <summary>
    /// Represents an UI unit that can be displayed and can influence to other components.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Parent of the component.
        /// </summary>
        public Component? Parent { get; set; }

        /// <summary>
        /// Updates specific component features, like animation and etc.
        /// Shouldn't influence to other components (i.e. globally).
        /// </summary>
        /// <param name="deltaTime">Time of the frame update.</param>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// Returns all sprites and primitives of the component, can return that from child components (if it's a container or layout).
        /// </summary>
        /// <param name="displayedArea">Area where this component will be stretched.</param>
        /// <returns>All sprites that are ready to render.</returns>
        public abstract IEnumerable<Sprite> GetSprites(RectangleF displayedArea);
    }
}