using Sesion2_Lab01.com.isil.modules.obj_loader.data;

using SharpDX;
using SharpDX.IO;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01.com.isil.modules.obj_loader {
    public class NModelLoader_Obj {

        static char[] splitCharacters = new char[] { ' ' };
        static char[] faceParamaterSplitter = new char[] { '/' };

        private List<Vector3> vertices;
        private List<Vector3> normals;
        private List<Vector2> texCoords;
        private Dictionary<dtNObjVertex, int> objVerticesIndexDictionary;

        private List<dtNObjVertex> objVertices;
        private List<dtNObjTriangle> objTriangles;

        private StreamReader mStreamReader;

        // usable
        private float[] mVertices;
        private uint[] mIndicesTriangles;
        private uint[] mIndicesQuads;

        public float[] Vertices { get { return mVertices; } }
        public uint[] IndicesTriangles { get { return mIndicesTriangles; } }
        public uint[] IndicesQuads { get { return mIndicesQuads; } }

        public NModelLoader_Obj(string path) {
            byte[] rawData = NativeFile.ReadAllBytes(path);
            mStreamReader = new StreamReader(new MemoryStream(rawData));

            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            texCoords = new List<Vector2>();
            objVerticesIndexDictionary = new Dictionary<dtNObjVertex, int>();
            objVertices = new List<dtNObjVertex>();
            objTriangles = new List<dtNObjTriangle>();

            this.Load(mStreamReader);
        }

        private void Load(TextReader textReader) {
            string line;

            Vector3 temp_Vertex = Vector3.Zero;
            Vector3 temp_Normal = Vector3.Zero;
            Vector2 temp_TextCoorrd = Vector2.Zero;

            dtNObjQuad temp_objQuad = new dtNObjQuad();
            dtNObjTriangle temp_objTriangle = new dtNObjTriangle();

            List<dtNObjQuad> objQuads = new List<dtNObjQuad>();

            // culturalinfo for parser
            NumberStyles nsFloat = NumberStyles.Float;
            CultureInfo ci = new CultureInfo("en-US");

            while ((line = textReader.ReadLine()) != null) {
                line = line.Trim(splitCharacters);
                line = line.Replace("  ", " ");

                string[] parameters = line.Split(splitCharacters);

                switch (parameters[0]) {
                    case "p": // Point
                        break;
                    case "v": // Vertex
                        temp_Vertex.X = float.Parse(parameters[1], nsFloat, ci); // x
                        temp_Vertex.Y = float.Parse(parameters[2], nsFloat, ci); // y
                        temp_Vertex.Z = float.Parse(parameters[3], nsFloat, ci); // z

                        vertices.Add(temp_Vertex);
                        break;
                    case "vt": // TexCoord
                        temp_TextCoorrd.X = float.Parse(parameters[1], nsFloat, ci); // u
                        temp_TextCoorrd.Y = float.Parse(parameters[2], nsFloat, ci); // v

                        texCoords.Add(temp_TextCoorrd);
                        break;
                    case "vn": // Normal
                        temp_Normal.X = float.Parse(parameters[1], nsFloat, ci); // nx
                        temp_Normal.Y = float.Parse(parameters[2], nsFloat, ci); // ny
                        temp_Normal.Z = float.Parse(parameters[3], nsFloat, ci); // nz

                        normals.Add(temp_Normal);
                        break;
                    case "f":
                        switch (parameters.Length) {
                            case 4:
                                // flipping the index original: 1, 2, 3 -> new is: 3, 2, 1
                                temp_objTriangle.Index0 = ParseFaceParameter(parameters[3]);
                                temp_objTriangle.Index1 = ParseFaceParameter(parameters[2]);
                                temp_objTriangle.Index2 = ParseFaceParameter(parameters[1]);
                                objTriangles.Add(temp_objTriangle);
                                break;
                            case 5:
                                // flipping the index of the quad: 1, 2, 3, 4 -> new is: 1, 4, 2, 4, 3, 2
                                temp_objQuad.Index0 = ParseFaceParameter(parameters[1]);
                                temp_objQuad.Index1 = ParseFaceParameter(parameters[4]);
                                temp_objQuad.Index2 = ParseFaceParameter(parameters[2]);
                                temp_objQuad.Index3 = temp_objQuad.Index1;
                                temp_objQuad.Index4 = ParseFaceParameter(parameters[3]);
                                temp_objQuad.Index5 = temp_objQuad.Index2;

                                objQuads.Add(temp_objQuad);
                                break;
                        }
                        break;
                }
            }

            // create interleaved vertices, textcoords and normals
            mVertices = new float[objVertices.Count * 8];

            for (int i = 0, indexV = 0, indexLength = objVertices.Count; i < indexLength; i++) {
                dtNObjVertex vertex = objVertices[i];

                mVertices[indexV++] = vertex.Vertex.X;
                mVertices[indexV++] = vertex.Vertex.Y;
                mVertices[indexV++] = vertex.Vertex.Z;
                mVertices[indexV++] = vertex.Normal.X;
                mVertices[indexV++] = vertex.Normal.Y;
                mVertices[indexV++] = vertex.Normal.Z;
                mVertices[indexV++] = vertex.TexCoord.X;
                mVertices[indexV++] = vertex.TexCoord.Y;
            }

            // create index for triangles
            mIndicesTriangles = new uint[objTriangles.Count * 3];

            for (int i = 0, indexT = 0, indexLength = objTriangles.Count; i < indexLength; i++) {
                dtNObjTriangle triangle = objTriangles[i];

                mIndicesTriangles[indexT++] = (uint)triangle.Index0;
                mIndicesTriangles[indexT++] = (uint)triangle.Index1;
                mIndicesTriangles[indexT++] = (uint)triangle.Index2;
            }

            // create index for quads
            mIndicesQuads = new uint[objQuads.Count * 6];

            for (int i = 0, indexQ = 0, indexLength = objQuads.Count; i < indexLength; i++) {
                dtNObjQuad quad = objQuads[i];

                mIndicesQuads[indexQ++] = (uint)quad.Index0;
                mIndicesQuads[indexQ++] = (uint)quad.Index1;
                mIndicesQuads[indexQ++] = (uint)quad.Index2;
                mIndicesQuads[indexQ++] = (uint)quad.Index3;
                mIndicesQuads[indexQ++] = (uint)quad.Index4;
                mIndicesQuads[indexQ++] = (uint)quad.Index5;
            }

            objVerticesIndexDictionary = null;
            vertices = null;
            normals = null;
            texCoords = null;
            objVertices = null;
            objTriangles = null;
            objQuads = null;
        }

        private int ParseFaceParameter(string faceParameter) {
            Vector3 vertex = Vector3.Zero;
            Vector2 texCoord = Vector2.Zero;
            Vector3 normal = Vector3.Zero;

            string[] parameters = faceParameter.Split(faceParamaterSplitter);

            int vertexIndex = int.Parse(parameters[0]);
            if (vertexIndex < 0) vertexIndex = vertices.Count + vertexIndex;
            else vertexIndex = vertexIndex - 1;
            vertex = vertices[vertexIndex];

            if (parameters.Length > 1) {
                int texCoordIndex = int.Parse(parameters[1]);
                if (texCoordIndex < 0) texCoordIndex = texCoords.Count + texCoordIndex;
                else texCoordIndex = texCoordIndex - 1;
                texCoord = texCoords[texCoordIndex];
            }

            if (parameters.Length > 2) {
                int normalIndex = int.Parse(parameters[2]);
                if (normalIndex < 0) normalIndex = normals.Count + normalIndex;
                else normalIndex = normalIndex - 1;
                normal = normals[normalIndex];
            }

            return FindOrAddObjVertex(ref vertex, ref texCoord, ref normal);
        }

        private int FindOrAddObjVertex(ref Vector3 vertex, ref Vector2 texCoord, ref Vector3 normal) {
            dtNObjVertex newObjVertex = dtNObjVertex.Empty;
            newObjVertex.Vertex = vertex;
            newObjVertex.TexCoord = texCoord;
            newObjVertex.Normal = normal;

            int index;

            if (objVerticesIndexDictionary.TryGetValue(newObjVertex, out index)) {
                return index;
            }
            else {
                objVertices.Add(newObjVertex);
                objVerticesIndexDictionary[newObjVertex] = objVertices.Count - 1;
                return objVertices.Count - 1;
            }
        }
    }
}
