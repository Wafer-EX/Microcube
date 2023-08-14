namespace Microcube.Graphics.ColorModels
{
    /// <summary>
    /// Represents the HSV color model with extra alpha channel.
    /// </summary>
    public struct HsvaColor
    {
        /// <summary>
        /// Hue of the color. The range is 0..360.
        /// </summary>
        public float Hue { get; set; }

        /// <summary>
        /// Saturation of the color. The range is 0..1.
        /// </summary>
        public float Saturation { get; set; }

        /// <summary>
        /// Value of the color. The range is 0..1.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Alpha of the color. The range is 0..1.
        /// </summary>
        public float Alpha { get; set; }

        public HsvaColor(float hue, float saturation, float value, float alpha)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
            Alpha = alpha;
        }

        /// <summary>
        /// Safely offsets hue of this color.
        /// </summary>
        /// <param name="offset">Hue offset.</param>
        /// <returns>HSVA color with offsetted hue.</returns>
        public HsvaColor OffsetHue(float offset)
        {
            Hue += offset;

            if (Hue >= 360.0f)
            {
                while (Hue >= 360.0f)
                    Hue -= 360.0f;
            }
            else if (Hue < 0.0f)
            {
                while (Hue < 360.0f)
                    Hue += 360.0f;
            }

            return this;
        }

        /// <summary>
        /// Converts this color to RgbaColor.
        /// </summary>
        /// <param name="color">This color.</param>
        public static explicit operator RgbaColor(HsvaColor color)
        {
            float h = color.Hue;
            float s = color.Saturation;
            float v = color.Value;

            float c = v * s;
            float x = c * (1.0f - MathF.Abs((h / 60.0f % 2.0f) - 1.0f));
            float m = v - c;

            return ((int)h / 60) switch
            {
                0 => new RgbaColor(c + m, x + m, m, color.Alpha),
                1 => new RgbaColor(x + m, c + m, m, color.Alpha),
                2 => new RgbaColor(m, c + m, x + m, color.Alpha),
                3 => new RgbaColor(m, x + m, c + m, color.Alpha),
                4 => new RgbaColor(x + m, m, c + m, color.Alpha),
                5 or _ => new RgbaColor(c + m, m, x + m, color.Alpha),
            };
        }
    }
}