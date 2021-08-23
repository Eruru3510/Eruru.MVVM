using System;
using System.Drawing;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMControl {

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

		protected MVVMBinding _Text;

		MVVMBinding _ForeColor;

		public MVVMControl (Control control) {
			Control = control;
			Control.TextChanged += (sender, e) => OnChanged (_Text);
			Control.LostFocus += OnLostFocus;
			Control.ForeColorChanged += (sender, e) => OnChanged (_ForeColor);
		}

		protected virtual void OnSetText (MVVMBinding text) {

		}

		protected virtual void OnLostFocus (object sender, EventArgs e) {
			OnChanged (_Text, MVVMOnChangedType.LostFocus);
		}

	}

}