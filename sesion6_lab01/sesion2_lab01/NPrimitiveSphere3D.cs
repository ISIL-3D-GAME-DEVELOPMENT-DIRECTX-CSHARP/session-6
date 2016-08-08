using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01 {
    public class NPrimitiveSphere3D {

        protected float[] mVertices;
        protected float[] mNormals;
        protected ushort[] mIndices;

        private int mIndicesCount;
        private int mNumSegments;
        private int mNumRings;

        public float[] Vertices { get { return mVertices; } }
        public float[] Normals  { get { return mNormals; } }
        public ushort[] Indices { get { return mIndices; } }

        public int IndicesCount { get { return mIndicesCount; } }

        public NPrimitiveSphere3D(int numRings, int numSegments, float radius) {
            mNumRings = numRings;
            mNumSegments = numSegments;

            mVertices = new float[((numRings + 1) * (numSegments + 1)) * 6]; // x, y, z || nx, ny, nz

            float deltaRingsAngle = (float)Math.PI / numRings;
            float deltaSegmentsAngle = (2.0f * (float)Math.PI) / numSegments;

            int indexV = 0;
            Vector3 vNormal = Vector3.Zero;

            for (int cRing = 0; cRing < mNumRings + 1; cRing++) {
                float phiAngle = cRing * deltaRingsAngle;

                float r0 = radius;
                float z0 = r0 * (float)Math.Cos(phiAngle);

                for (int cSegment = 0; cSegment < mNumSegments + 1; cSegment++) {
                    float thetaAngle = cSegment * deltaSegmentsAngle;

                    float x0 = r0 * (float)Math.Cos(thetaAngle) * (float)Math.Sin(phiAngle);
                    float y0 = r0 * (float)Math.Sin(thetaAngle) * (float)Math.Sin(phiAngle);

                    vNormal.X = x0;
                    vNormal.Y = y0;
                    vNormal.Z = z0;

                    Vector3.Normalize(ref vNormal, out vNormal);

                    mVertices[indexV++] = x0;
                    mVertices[indexV++] = y0;
                    mVertices[indexV++] = z0;
                    mVertices[indexV++] = vNormal.X;
                    mVertices[indexV++] = vNormal.Y;
                    mVertices[indexV++] = vNormal.Z;
                }
            }

            // cramos los indices
            CreateIndices();
        }

        private void CreateIndices() {
            int index_count = 0;
            ushort vertexIndex = 0;

            mIndicesCount = ((mNumRings + 1) * mNumSegments) * 2;
            mIndices = new ushort[mIndicesCount];

            for (int cRing = 0; cRing < mNumRings + 1; cRing++) {
                for (int cSegment = 0; cSegment < mNumSegments + 1; cSegment++) {
                    if (cRing != mNumRings) {
                        mIndices[index_count++] = vertexIndex;
                        mIndices[index_count++] = (ushort)(vertexIndex + (mNumSegments + 1));

                        vertexIndex++;
                    }
                }
            }
        }
    }
}
