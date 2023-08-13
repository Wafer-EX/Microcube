using Silk.NET.Maths;
using System.Numerics;

namespace Microcube.Extensions
{
    public static class VectorExtensions
    {
        public static Vector4 ToSystem(this Vector4D<float> vector)
        {
            return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static Vector4D<float> ToGeneric(this Vector4 vector)
        {
            return new Vector4D<float>(vector.X, vector.Y, vector.Z, vector.W);
        }
    }
}