using Silk.NET.Maths;
using System.Numerics;

namespace Microcube.Extensions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Adds missing implememtation that should be in the Silk.Math library but by some reasons it doesn't has it.
        /// </summary>
        /// <param name="vector">Silk.Math vector</param>
        /// <returns>System vector</returns>
        public static Vector4 ToSystem(this Vector4D<float> vector)
        {
            return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        /// <summary>
        /// Adds missing implememtation that should be in the Silk.Math library but by some reasons it doesn't has it.
        /// </summary>
        /// <param name="vector">System vector</param>
        /// <returns>Silk.Math vector</returns>
        public static Vector4D<float> ToGeneric(this Vector4 vector)
        {
            return new Vector4D<float>(vector.X, vector.Y, vector.Z, vector.W);
        }
    }
}