using SharpDX;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01.com.isil.modules.obj_loader.data {
    [StructLayout(LayoutKind.Sequential)]
    public struct dtNObjVertex {
        public static dtNObjVertex Empty = new dtNObjVertex();

        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector3 Vertex;
    }
}
