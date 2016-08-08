using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace Sesion2_Lab01 {
    public class NPrimitiveCube3D {

        protected float[] mVertices;
        protected float[] mNormals;
        protected ushort[] mIndices;

        private int mIndicesCount;

        public float[] Vertices { get { return mVertices; } }
        public float[] Normals  { get { return mNormals; } }
        public ushort[] Indices { get { return mIndices; } }

        public int IndicesCount { get { return mIndicesCount; } }

        public NPrimitiveCube3D(float x, float y, float z, float size) {
            // A cube has six faces, each one pointing in a different direction.
            mNormals = new float[6 * 3]; // 6 faces, 3 components

            mNormals[0] = 0f; mNormals[1] = 0f; mNormals[2] = 1f;
            mNormals[3] = 0f; mNormals[4] = 0f; mNormals[5] = -1f;
            mNormals[6] = 1f; mNormals[7] = 0f; mNormals[8] = 0f;
            mNormals[9] = -1f; mNormals[10] = 0f; mNormals[11] = 0f;
            mNormals[12] = 0f; mNormals[13] = 1f; mNormals[14] = 0f;
            mNormals[15] = 0f; mNormals[16] = -1f; mNormals[17] = 0f;

            mVertices = new float[(6 * 4) * 6]; // 6 faces, 4 vertex, 3 position and 3 normals

            int vertexOffset = 0;

            // Create each face in turn.
            for (int i = 0; i < mNormals.Length; i += 3) {
                Vector3 normal = Vector3.Zero;
                normal.X = mNormals[i + 0];
                normal.Y = mNormals[i + 1];
                normal.Z = mNormals[i + 2];

                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = Vector3.Zero;
                side1.X = normal.Y;
                side1.Y = normal.Z;
                side1.Z = normal.X;

                Vector3 side2 = Vector3.Zero;
                Vector3.Cross(ref normal, ref side1, out side2);

                // Four vertices per face.
                Vector3 vertex_1 = (normal - side1 - side2) * size / 2;
                Vector3 vertex_2 = (normal - side1 + side2) * size / 2;
                Vector3 vertex_3 = (normal + side1 + side2) * size / 2;
                Vector3 vertex_4 = (normal + side1 - side2) * size / 2;

                mVertices[vertexOffset + 0] = vertex_1.X;
                mVertices[vertexOffset + 1] = vertex_1.Y;
                mVertices[vertexOffset + 2] = vertex_1.Z;
                mVertices[vertexOffset + 3] = normal.X;
                mVertices[vertexOffset + 4] = normal.Y;
                mVertices[vertexOffset + 5] = normal.Z;

                mVertices[vertexOffset + 6] = vertex_2.X;
                mVertices[vertexOffset + 7] = vertex_2.Y;
                mVertices[vertexOffset + 8] = vertex_2.Z;
                mVertices[vertexOffset + 9] = normal.X;
                mVertices[vertexOffset + 10] = normal.Y;
                mVertices[vertexOffset + 11] = normal.Z;

                mVertices[vertexOffset + 12] = vertex_3.X;
                mVertices[vertexOffset + 13] = vertex_3.Y;
                mVertices[vertexOffset + 14] = vertex_3.Z;
                mVertices[vertexOffset + 15] = normal.X;
                mVertices[vertexOffset + 16] = normal.Y;
                mVertices[vertexOffset + 17] = normal.Z;

                mVertices[vertexOffset + 18] = vertex_4.X;
                mVertices[vertexOffset + 19] = vertex_4.Y;
                mVertices[vertexOffset + 20] = vertex_4.Z;
                mVertices[vertexOffset + 21] = normal.X;
                mVertices[vertexOffset + 22] = normal.Y;
                mVertices[vertexOffset + 23] = normal.Z;

                vertexOffset += 24;
            }

            //creamos los indices para el cubo
            CreateIndices();
        }

        private void CreateIndices() {
            int indexOffset = 0;

            mIndicesCount = 6 * 6; // 6 faces, 6 quads
            mIndices = new ushort[mIndicesCount];

            for (int i = 0; i < mIndicesCount; i += 6) {
                mIndices[i] = (ushort)indexOffset;
                mIndices[i + 1] = (ushort)(indexOffset + 1);
                mIndices[i + 2] = (ushort)(indexOffset + 2);
                mIndices[i + 3] = (ushort)indexOffset;
                mIndices[i + 4] = (ushort)(indexOffset + 2);
                mIndices[i + 5] = (ushort)(indexOffset + 3);

                indexOffset += 4;
            }
        }
    }
}
