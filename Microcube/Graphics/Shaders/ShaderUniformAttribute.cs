namespace Microcube.Graphics.Shaders
{
    /// <summary>
    /// It's an indicator that the property is uniform variable in a shader, so
    /// it's needed to set uniform with value in the property.
    /// </summary>
    /// <param name="uniform">Name of the uniform inside shader.</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class ShaderUniformAttribute(string uniform) : Attribute
    {
        public string Uniform { get; set; } = uniform;
    }
}