namespace Microcube.Graphics
{
    /// <summary>
    /// Represent a mesh that include vertices data.
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// Vertices of the mesh.
        /// </summary>
        public float[] Vertices { get; private set; }

        /// <summary>
        /// Count of the vertices of the mesh.
        /// </summary>
        public uint VerticesCount { get; private set; }

        protected Mesh() => Vertices = Array.Empty<float>();

        /// <summary>
        /// Creates textured cube with positions, normals and texture coordinates (uv).
        /// </summary>
        /// <param name="scale">Scale of the cube.</param>
        /// <returns>Mesh of the cube.</returns>
        public static Mesh CreateTexturedCube(float scale)
        {
            float halfScale = scale / 2;
            return new Mesh()
            {
                VerticesCount = 36,
                Vertices = new float[]
                {
                    // --- TOP ------------------------------------------
                    // 3x POSITIONS | 3x NORMALS | 2x TEXTURE COORDINATES
                    -halfScale,  halfScale, -halfScale,  0,  1,  0, 0, 0,
                    -halfScale,  halfScale,  halfScale,  0,  1,  0, 0, 1,
                     halfScale,  halfScale,  halfScale,  0,  1,  0, 1, 1,
                    -halfScale,  halfScale, -halfScale,  0,  1,  0, 0, 0,
                     halfScale,  halfScale,  halfScale,  0,  1,  0, 1, 1,
                     halfScale,  halfScale, -halfScale,  0,  1,  0, 1, 0,

                     // --- BOTTOM ---------------------------------------
                     // 3x POSITIONS | 3x NORMALS | 2x TEXTURE COORDINATES
                    -halfScale, -halfScale, -halfScale,  0, -1,  0, 0, 0,
                     halfScale, -halfScale,  halfScale,  0, -1,  0, 1, 1,
                    -halfScale, -halfScale,  halfScale,  0, -1,  0, 0, 1,
                    -halfScale, -halfScale, -halfScale,  0, -1,  0, 0, 0,
                     halfScale, -halfScale, -halfScale,  0, -1,  0, 1, 0,
                     halfScale, -halfScale,  halfScale,  0, -1,  0, 1, 1,

                     // --- RIGHT ----------------------------------------
                     // 3x POSITIONS | 3x NORMALS | 2x TEXTURE COORDINATES
                     halfScale, -halfScale, -halfScale,  1,  0,  0, 1, 1,
                     halfScale,  halfScale,  halfScale,  1,  0,  0, 0, 0,
                     halfScale, -halfScale,  halfScale,  1,  0,  0, 1, 0,
                     halfScale, -halfScale, -halfScale,  1,  0,  0, 1, 1,
                     halfScale,  halfScale, -halfScale,  1,  0,  0, 1, 0,
                     halfScale,  halfScale,  halfScale,  1,  0,  0, 0, 0,

                     // --- LEFT -----------------------------------------
                     // 3x POSITIONS | 3x NORMALS | 2x TEXTURE COORDINATES
                    -halfScale, -halfScale, -halfScale, -1,  0,  0, 1, 1,
                    -halfScale, -halfScale,  halfScale, -1,  0,  0, 1, 0,
                    -halfScale,  halfScale,  halfScale, -1,  0,  0, 0, 0,
                    -halfScale, -halfScale, -halfScale, -1,  0,  0, 1, 1,
                    -halfScale,  halfScale,  halfScale, -1,  0,  0, 0, 0,
                    -halfScale,  halfScale, -halfScale, -1,  0,  0, 1, 0,

                     // --- BACK -----------------------------------------
                     // 3x POSITIONS | 3x NORMALS | 2x TEXTURE COORDINATES
                    -halfScale, -halfScale,  halfScale,  0,  0,  1, 1, 1,
                     halfScale, -halfScale,  halfScale,  0,  0,  1, 1, 0,
                     halfScale,  halfScale,  halfScale,  0,  0,  1, 0, 0,
                    -halfScale, -halfScale,  halfScale,  0,  0,  1, 1, 1,
                     halfScale,  halfScale,  halfScale,  0,  0,  1, 0, 0,
                    -halfScale,  halfScale,  halfScale,  0,  0,  1, 0, 1,

                    // --- FRONT ----------------------------------------
                    // 3x POSITIONS | 3x NORMALS | 2x TEXTURE COORDINATES
                    -halfScale, -halfScale, -halfScale,  0,  0, -1, 1, 1,
                     halfScale,  halfScale, -halfScale,  0,  0, -1, 0, 0,
                     halfScale, -halfScale, -halfScale,  0,  0, -1, 1, 0,
                    -halfScale, -halfScale, -halfScale,  0,  0, -1, 1, 1,
                    -halfScale,  halfScale, -halfScale,  0,  0, -1, 0, 1,
                     halfScale,  halfScale, -halfScale,  0,  0, -1, 0, 0,
                },
            };
        }
    }
}