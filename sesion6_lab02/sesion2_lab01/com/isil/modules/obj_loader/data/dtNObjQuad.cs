using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01.com.isil.modules.obj_loader.data {
    [StructLayout(LayoutKind.Sequential)]
    public struct dtNObjQuad {
        public int Index0;
        public int Index1;
        public int Index2;
        public int Index3;
        public int Index4;
        public int Index5;
    }
}
