namespace Microcube.Graphics
{
    public class Mesh
    {
        public float[] Vertices { get; private set; }

        public uint VerticesCount { get; private set; }

        protected Mesh() => Vertices = Array.Empty<float>();

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