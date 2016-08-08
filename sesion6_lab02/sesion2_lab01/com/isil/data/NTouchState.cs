using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01.com.isil.data {
    public struct NTouchState {
        public static NTouchState Empty = new NTouchState();

        public int id;
        public NTouchLocationState TouchState;
        public NTouchLocationState PreviousTouchState;
        public Vector2 Position;
        public Vector2 PreviousPosition;
    }
}
