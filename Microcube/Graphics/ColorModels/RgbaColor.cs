namespace Microcube.Graphics.ColorModels
{
    /// <summary>
    /// Represents the RGBA color model.
    /// </summary>
    public struct RgbaColor(float r, float g, float b, float a)
    {
        /// <summary>
        /// Red channel. The range is 0..1.
        /// </summary>
        public float Red { get; set; } = r;

        /// <summary>
        /// Green channel. The range is 0..1.
        /// </summary>
        public float Green { get; set; } = g;

        /// <summary>
        /// Blue channel. The range is 0..1.
        /// </summary>
        public float Blue { get; set; } = b;

        /// <summary>
        /// Alpha channel. The range is 0..1.
        /// </summary>
        public float Alpha { get; set; } = a;

        public static RgbaColor Black => new(0.0f, 0.0f, 0.0f, 1.0f);

        public static RgbaColor White => new(1.0f, 1.0f, 1.0f, 1.0f);

        public static RgbaColor Transparent => new(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// Safely offsets hue of the color.
        /// </summary>
        /// <param name="offset">Hue offset.</param>
        /// <returns>RGBA color with offsetted hue.</returns>
        public readonly RgbaColor OffsetHue(float offset)
        {
            var hsvaColor = (HsvaColor)this;
            return (RgbaColor)hsvaColor.OffsetHue(offset);
        }

        /// <summary>
        /// Converts this color to HsvaColor.
        /// </summary>
        /// <param name="color">RGBA color.</param>
        public static explicit operator HsvaColor(RgbaColor color)
        {
            float r = color.Red;
            float g = color.Green;
            float b = color.Blue;

            float max = MathF.Max(r, MathF.Max(g, b));
            float min = MathF.Min(r, MathF.Min(g, b));

            float h = 0.0f;
            if (max == r && g >= b)
                h = 60.0f * (g - b) / (max - min);
            else if (max == r && g < b)
                h = 60.0f * (g - b) / (max - min) + 360.0f;
            else if (max == g)
                h = 60.0f * (b - r) / (max - min) + 120.0f;
            else if (max == b)
                h = 60.0f * (r - g) / (max - min) + 240.0f;

            float s = 0.0f;
            if (max != 0.0f)
                s = 1.0f - (min / max);

            float v = max;

            return new HsvaColor(h, s, v, color.Alpha);
        }

        public static bool operator ==(RgbaColor left, RgbaColor right)
        {
            bool isRedMatches = left.Red == right.Red;
            bool isGreenMatches = left.Green == right.Green;
            bool isBlueMatches = left.Blue == right.Blue;
            bool isAlphaMatches = left.Alpha == right.Alpha;
            return isRedMatches && isGreenMatches && isBlueMatches && isAlphaMatches;
        }

        public static bool operator !=(RgbaColor left, RgbaColor right)
        {
            bool isRedMatches = left.Red == right.Red;
            bool isGreenMatches = left.Green == right.Green;
            bool isBlueMatches = left.Blue == right.Blue;
            bool isAlphaMatches = left.Alpha == right.Alpha;
            return !isRedMatches || !isGreenMatches || !isBlueMatches || !isAlphaMatches;
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is RgbaColor rgbaColor)
                return rgbaColor == this;

            return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Red, Green, Blue, Alpha);
        }
    }
}