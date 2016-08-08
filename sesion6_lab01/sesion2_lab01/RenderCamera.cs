using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using Sesion2_Lab01.com.isil.data;

namespace Sesion2_Lab01 {
    public class RenderCamera {
        private const uint Up = 0x00000001;
        private const uint Down = 0x00000010;
        private const uint Right = 0x00000100;
        private const uint Left = 0x00001000;
        private const uint Forward = 0x00010000;
        private const uint Backward = 0x00100000;

        public const int PERSPECTIVE = 0;
        public const int ORTHOGRAPHIC = 1;

        private int mCameraType;

        private float mLeftrightRot;
        private float mUpdownRot;

        // for key control
        private bool mTouching;
        private uint mDirectionsSwitches;

        private Vector2 mOldPosition;
        private float mRotationSpeed = 0.005f;

        private Matrix mViewNMatrix;
        private Matrix mProjectionNMatrix;
        private Matrix mWorld;
        private Matrix mTransformed;

        private Vector3 mCameraPosition;

        public float UpDownRotation {
            get { return mUpdownRot; }
            set { mUpdownRot = value; }
        }

        public float LeftRightRotation {
            get { return mLeftrightRot; }
            set { mLeftrightRot = value; }
        }

        public virtual Vector3 Position {
            get { return mCameraPosition; }
            set { mCameraPosition = value; }
        }

        public Matrix world         { get { return mWorld; } }
        public Matrix projection    { get { return mProjectionNMatrix; } }
        public Matrix view          { get { return mViewNMatrix; } }
        public Matrix transformed   { get { return mTransformed; } }

        public int CameraType       { get { return mCameraType; } }

        public RenderCamera(int width, int height) {
            mCameraType = RenderCamera.ORTHOGRAPHIC;

            mTouching = false;
            mLeftrightRot = 0f;
            mUpdownRot = 0f;

            mWorld = SimpleMatrix.Identity;
            mProjectionNMatrix = SimpleMatrix.CreateOrthographicOffCenter(0, -width, height, 0, 1.0f, 100.0f);
            mViewNMatrix = SimpleMatrix.CreateLookAt(new Vector3(0, 0, -1.0f), Vector3.Zero, Vector3.UnitY);
        }

        public RenderCamera(NViewport viewport, float z) {
            mCameraType = RenderCamera.PERSPECTIVE;

            mTouching = false;
            mLeftrightRot = 0f;
            mUpdownRot = 0f;


            mWorld = mViewNMatrix = mProjectionNMatrix = mTransformed = SimpleMatrix.Identity;

            mCameraPosition = Vector3.Zero;
            mCameraPosition.Z = -z;

            float viewAngle = (float)Math.PI / 4.0f;
            float nearPlane = 0.1f;
            float farPlane = 4000000.0f;

            mProjectionNMatrix = SimpleMatrix.CreatePerspectiveFieldOfView(viewAngle,
                viewport.AspectRatio, nearPlane, farPlane);

            UpdateViewMatrix();
        }

        public void OnKeyDown(int keyCode) {
            switch (keyCode) {
            case EnumNKeyCode.W: mDirectionsSwitches |= RenderCamera.Up; break;
            case EnumNKeyCode.S: mDirectionsSwitches |= RenderCamera.Down; break;
            case EnumNKeyCode.D: mDirectionsSwitches |= RenderCamera.Right; break;
            case EnumNKeyCode.A: mDirectionsSwitches |= RenderCamera.Left; break;
            case EnumNKeyCode.Q: mDirectionsSwitches |= RenderCamera.Forward; break;
            case EnumNKeyCode.Z: mDirectionsSwitches |= RenderCamera.Backward; break;
            }
        }

        public void OnKeyUp(int keyCode) {
            switch (keyCode) {
            case EnumNKeyCode.W: mDirectionsSwitches ^= RenderCamera.Up; break;
            case EnumNKeyCode.S: mDirectionsSwitches ^= RenderCamera.Down; break;
            case EnumNKeyCode.D: mDirectionsSwitches ^= RenderCamera.Right; break;
            case EnumNKeyCode.A: mDirectionsSwitches ^= RenderCamera.Left; break;
            case EnumNKeyCode.Q: mDirectionsSwitches ^= RenderCamera.Forward; break;
            case EnumNKeyCode.Z: mDirectionsSwitches ^= RenderCamera.Backward; break;
            }
        }

        public virtual void UpdateViewMatrix() {
            Matrix cameraRotation = Matrix.RotationX(mUpdownRot) * Matrix.RotationY(mLeftrightRot);

            Vector3 cameraOriginalTarget = Vector3.Zero;
            cameraOriginalTarget.Z = -1;

            Vector3 cameraOriginalUpVector = Vector3.UnitY;

            Vector4 tres_1 = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraRotatedTarget = new Vector3(tres_1.X, tres_1.Y, tres_1.Z);
            Vector3 cameraFinalTarget = mCameraPosition + cameraRotatedTarget;

            Vector4 tres_2 = Vector3.Transform(cameraOriginalUpVector, cameraRotation);
            Vector3 cameraRotatedUpVector = new Vector3(tres_2.X, tres_2.Y, tres_2.Z);
            Vector3 cameraFinalUpVector = mCameraPosition + cameraRotatedUpVector;

            mViewNMatrix = SimpleMatrix.CreateLookAt(mCameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        public void AddToCameraPosition(Vector3 vectorToAdd) {
            float moveSpeed = 0.5f;

            Matrix cameraRotation = Matrix.RotationX(mUpdownRot) * Matrix.RotationY(mLeftrightRot);
            Vector4 tres_1 = Vector3.Transform(vectorToAdd, cameraRotation);
            Vector3 rotatedVector = new Vector3(tres_1.X, tres_1.Y, tres_1.Z);

            this.Position += moveSpeed * rotatedVector;
        }

        public void Update() {
            if ((mDirectionsSwitches & RenderCamera.Up) == RenderCamera.Up) { this.AddToCameraPosition(Vector3.UnitY); }
            if ((mDirectionsSwitches & RenderCamera.Down) == RenderCamera.Down) { this.AddToCameraPosition(-Vector3.UnitY); }
            if ((mDirectionsSwitches & RenderCamera.Left) == RenderCamera.Left) { this.AddToCameraPosition(-Vector3.UnitX); }
            if ((mDirectionsSwitches & RenderCamera.Right) == RenderCamera.Right) { this.AddToCameraPosition(Vector3.UnitX); }
            if ((mDirectionsSwitches & RenderCamera.Forward) == RenderCamera.Forward) { this.AddToCameraPosition(-Vector3.UnitZ); }
            if ((mDirectionsSwitches & RenderCamera.Backward) == RenderCamera.Backward) { this.AddToCameraPosition(Vector3.UnitZ); }

            // ahora con los eventos del mouse para rotar la camara
            List<NTouchState> tc = NativeApplication.instance.MouseHandler.TouchCollection;

            if (tc.Count > 0) {
                for (int i = 0; i < tc.Count; i++) {
                    NTouchState tl = tc[i];

                    if (!mTouching) {
                        mTouching = true;
                        mOldPosition = tl.Position;
                    }
                    else {
                        float xDifference = tl.Position.X - mOldPosition.X;
                        float yDifference = tl.Position.Y - mOldPosition.Y;

                        mLeftrightRot -= mRotationSpeed * xDifference;
                        mUpdownRot -= mRotationSpeed * yDifference;

                        mOldPosition = tl.Position;
                    }
                }
            }
            else { mTouching = false; }

            // ahora actualizamos nuestra matrix con los nuevos parametros de la camara
            switch (mCameraType) {
            case RenderCamera.PERSPECTIVE:
                UpdateViewMatrix();
                break;
            }

            Matrix temp = Matrix.Identity;

            Matrix.Multiply(ref mWorld, ref mViewNMatrix, out temp);
            Matrix.Multiply(ref temp, ref mProjectionNMatrix, out mTransformed);
        }
    }
}
