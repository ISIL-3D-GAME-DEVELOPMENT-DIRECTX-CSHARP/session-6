using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01 {
    public class NViewport {

        private int mWidth;
        private int mHeight;
        private float mAspectRatio;

        public int Width            { get { return mWidth; } }
        public int Height           { get { return mHeight; } }
        public float AspectRatio    { get { return mAspectRatio; } }

        public NViewport(int width, int height) {
            mWidth = width;
            mHeight = height;
            mAspectRatio = (float)mWidth / (float)mHeight;
        }
    }
}
