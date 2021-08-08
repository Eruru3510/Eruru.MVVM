using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMButton : MVVMControl {

		public new Button Control { get; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, binding => Text = binding);
			}

			set {
				SetBinding (ref _Text, value, () => Control.Text, targetValue => Control.Text = MVVMAPI.To<string> (targetValue));
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
		MVVMBinding _Click;
		MVVMBinding _ClickParameter;

		public MVVMButton (Button control) : base (control) {
			Control = control;
			Control.Click += Control_Click;
		}

		private void Control_Click (object sender, EventArgs e) {
			OnCommand (_Click, _ClickParameter);
		}

	}

}