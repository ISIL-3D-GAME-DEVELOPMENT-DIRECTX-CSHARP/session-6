using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using SharpDX.Windows;

namespace Sesion2_Lab01.com.isil.utils {
    public class NKeyboardHandler {
        private Action<int> mOnKeyDownAction;
        private Action<int> mOnKeyUpAction;

        public NKeyboardHandler(RenderForm renderForm, Action<int> OnKeyDownAction, Action<int> OnKeyUpAction) {
            mOnKeyDownAction = OnKeyDownAction;
            mOnKeyUpAction = OnKeyUpAction;

            renderForm.KeyDown += OnKeyDownHandler;
            renderForm.KeyUp += OnKeyUpHandler;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e) {
            if (mOnKeyDownAction != null) {
                mOnKeyDownAction(e.KeyValue);
            }
        }

        private void OnKeyUpHandler(object sender, KeyEventArgs e) {
            if (mOnKeyUpAction != null) {
                mOnKeyUpAction(e.KeyValue);
            }
        }
    }
}
