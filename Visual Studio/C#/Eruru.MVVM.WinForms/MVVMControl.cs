using System;
using System.Drawing;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMControl : MVVMControlBase {

		public Control Control { get; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, binding => Text = binding);
			}

			set {
				SetBinding (ref _Text, value, () => Control.Text, targetValue => Control.Text = MVVMAPI.To<string> (targetValue), false, true);
				OnSetText (value);
			}

		}
		public MVVMBinding ForeColor {

			get {
				return GetBinding (ref _ForeColor, binding => ForeColor = binding);
			}

			set {
				SetBinding (ref _ForeColor, value, () => Control.ForeColor, targetValue => Control.ForeColor = MVVMAPI.To<Color> (targetValue));
			}

		}
		public MVVMBinding BackColor {

			get {
				return GetBinding (ref _BackColor, binding => BackColor = binding);
			}

			set {
				SetBinding (ref _BackColor, value, () => Control.BackColor, targetValue => Control.BackColor = MVVMAPI.To<Color> (targetValue));
			}

		}
		public MVVMBinding Click {

			get {
				return GetBinding (ref _Click, binding => Click = binding);
			}

			set {
				SetBinding (ref _Click, value);
			}

		}
		public MVVMBinding ClickParameter {

			get {
				return GetBinding (ref _ClickParameter, binding => ClickParameter = binding);
			}

			set {
				SetBinding (ref _ClickParameter, value);
			}

		}

		MVVMBinding _Text;
		MVVMBinding _ForeColor;
		MVVMBinding _BackColor;
		MVVMBinding _Click;
		MVVMBinding _ClickParameter;

		public MVVMControl (Control control) {
			Control = control;
			Control.TextChanged += (sender, e) => OnChanged (_Text);
			Control.LostFocus += OnLostFocus;
			Control.ForeColorChanged += (sender, e) => OnChanged (_ForeColor);
			Control.BackColorChanged += (sender, e) => OnChanged (_BackColor);
			Control.Click += (sender, e) => OnCommand (_Click, _ClickParameter);
		}

		protected virtual void OnSetText (MVVMBinding text) {

		}

		protected virtual void OnLostFocus (object sender, EventArgs e) {
			OnChanged (_Text, MVVMOnChangedType.LostFocus);
		}

	}

}