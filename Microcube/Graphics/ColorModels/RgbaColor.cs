namespace Microcube.Graphics.ColorModels
{
    public struct RgbaColor
    {
        public float Red { get; set; }

        public float Green { get; set; }

        public float Blue { get; set; }

        public float Alpha { get; set; }

        public static RgbaColor Black => new(0.0f, 0.0f, 0.0f, 1.0f);

        public static RgbaColor White => new(1.0f, 1.0f, 1.0f, 1.0f);

        public static RgbaColor Transparent => new(0.0f, 0.0f, 0.0f, 0.0f);

        public RgbaColor(float r, float g, float b, float a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        public readonly RgbaColor OffsetHue(float hue)
        {
            var hsvaColor = (HsvaColor)this;
            return (RgbaColor)hsvaColor.OffsetHue(hue);
        }

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