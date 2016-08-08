using Sesion2_Lab01.com.isil.modules.obj_loader;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01.com.isil.core.graphics {
    public class NStaticModel3D {

        private NTexture2D mTexture;
        private NModelLoader_Obj mModelLoader;

        private Shader3DProgram mShaderProgram;
        private RenderCamera mCamera;

        private float[] mVertices;
        private uint[] mIndicesTriangles;
        private uint[] mIndicesQuads; 

        public NStaticModel3D(Device device, RenderCamera camera, string texturePath, string modelPath,
            Shader3DProgram shaderProgram) {

            mCamera = camera;
            mShaderProgram = shaderProgram;
            
            mTexture = new NTexture2D(device);
            mTexture.Load(texturePath);

            mModelLoader = new NModelLoader_Obj(modelPath);

            mVertices = mModelLoader.Vertices;
            mIndicesTriangles = mModelLoader.IndicesTriangles;
            mIndicesQuads = mModelLoader.IndicesQuads;
        }

        public void Update(int dt) {
            mShaderProgram.Update(mVertices, mIndicesQuads);
        }

        public void Draw(int dt) {
            mShaderProgram.Draw(mCamera.transformed, mTexture, PrimitiveTopology.TriangleList);
        }
    }
}
