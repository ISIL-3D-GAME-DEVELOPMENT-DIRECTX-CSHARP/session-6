using Sesion2_Lab01.com.isil.data;
using SharpDX;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sesion2_Lab01.com.isil.utils {
    public class NMouseHandler {

        private List<NTouchState> mTouchsCollection;

        // solo para el evento del OnTouchMoved
        private Dictionary<uint, NTouchLocationState> mPersistentTouchStates;

        public List<NTouchState> TouchCollection { get { return mTouchsCollection; } }

        public NMouseHandler(RenderForm renderForm) {
            mTouchsCollection = new List<NTouchState>();
            mPersistentTouchStates = new Dictionary<uint, NTouchLocationState>();

            renderForm.MouseDown += OnTouchPressed;
            renderForm.MouseMove += OnTouchMoved;
            renderForm.MouseUp += OnTouchReleased;
            renderForm.MouseLeave += OnTouchInvalid;
        }

        private void OnTouchPressed(object sender, MouseEventArgs e) {
            uint id = 0; // default cause is only one pointer on screen

            Vector2 tempPosition = Vector2.Zero;
            tempPosition.X = (float)e.Location.X;
            tempPosition.Y = (float)e.Location.Y;

            registerTouches((int)id, tempPosition, NTouchLocationState.Pressed);

            mPersistentTouchStates[id] = NTouchLocationState.Pressed;
        }

        private void OnTouchMoved(object sender, MouseEventArgs e) {
            uint id = 0; // default cause is only one pointer on screen

            if (mPersistentTouchStates.ContainsKey(id)) {
                switch (mPersistentTouchStates[id]) {
                case NTouchLocationState.Pressed:
                    Vector2 tempPosition = Vector2.Zero;
                    tempPosition.X = (float)e.Location.X;
                    tempPosition.Y = (float)e.Location.Y;

                    registerTouches((int)id, tempPosition, NTouchLocationState.Moved);
                    break;
                }
            }
        }

        private void OnTouchReleased(object sender, MouseEventArgs e) {
            uint id = 0; // default cause is only one pointer on screen

            Vector2 tempPosition = Vector2.Zero;
            tempPosition.X = (float)e.Location.X;
            tempPosition.Y = (float)e.Location.Y;

            registerTouches((int)id, tempPosition, NTouchLocationState.Released);

            mPersistentTouchStates[id] = NTouchLocationState.Released;
        }

        private void OnTouchInvalid(object sender, EventArgs e) {
            uint id = 0; // default cause is only one pointer on screen

            Vector2 tempPosition = Vector2.Zero;
            tempPosition.X = 0f;
            tempPosition.Y = 0f;

            registerTouches((int)id, tempPosition, NTouchLocationState.Invalid);

           mPersistentTouchStates[id] = NTouchLocationState.Invalid;
        }

        private void registerTouches(int id, Vector2 position, NTouchLocationState state) {
            NTouchState touchState = NTouchState.Empty;

            touchState.id = id;
            touchState.TouchState = state;
            touchState.Position = position;

            int foundIndex = -1;
            bool foundedTouchState = false;

            for (int i = 0; i < mTouchsCollection.Count; i++) {
                if (mTouchsCollection[i].id == id) {
                    foundedTouchState = true;
                    foundIndex = i;
                    break;
                }
            }

            if (!foundedTouchState) {
                mTouchsCollection.Add(touchState);
            }
            else {
                mTouchsCollection[foundIndex] = touchState;
            }
        }

        public void Update(int dt) {
            for (int i = 0; i < mTouchsCollection.Count; i++) {
                NTouchState touchState = mTouchsCollection[i];

                switch (touchState.TouchState) {
                case NTouchLocationState.Pressed:
                    touchState.TouchState = NTouchLocationState.Moved;
                    touchState.PreviousTouchState = NTouchLocationState.Pressed;

                    mTouchsCollection[i] = touchState;
                    break;
                case NTouchLocationState.Moved:
                    touchState.PreviousTouchState = NTouchLocationState.Moved;

                    mTouchsCollection[i] = touchState;
                    break;
                case NTouchLocationState.Released:
                case NTouchLocationState.Invalid:
                    mTouchsCollection.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
}
