using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMTextBox : MVVMControl {

		public new TextBox Control { get; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, binding => Text = binding);
			}

			set {
				SetBinding (
					ref _Text, value,
					() => Control.Text, targetValue => Control.Text = MVVMAPI.To<string> (targetValue),
					null, MVVMBindingMode.TwoWay, MVVMUpdateSourceTrigger.LostFocus
				);
			}

		}

		MVVMBinding _Text;

		public MVVMTextBox (TextBox control) : base (control) {
			Control = control;
			Control.TextChanged += Control_TextChanged;
			control.LostFocus += Control_LostFocus;
		}

		private void Control_TextChanged (object sender, EventArgs e) {
			OnChanged (_Text);
		}

		private void Control_LostFocus (object sender, EventArgs e) {
			OnChanged (_Text, MVVMOnChangedType.LostFocus);
		}

	}

}