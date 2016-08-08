using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace Sesion2_Lab01 {
    public struct Shader3DInputParameters {
        public static Shader3DInputParameters EMPTY = new Shader3DInputParameters();

        public float ambientIntensity;
	    public Vector3 lightDirection;
	    public Vector4 ambientColor;
        public Vector4 diffuseLighting;
        public Matrix transformation;
    }
}
