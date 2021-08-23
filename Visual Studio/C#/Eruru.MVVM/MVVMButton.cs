using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMButton : MVVMControl {

		public Button Button { get; }
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

		MVVMBinding _Click;
		MVVMBinding _ClickParameter;

		public MVVMButton (Button button) : base (button) {
			Button = button;
			Button.Click += (sender, e) => OnCommand (_Click, _ClickParameter);
		}

	}

}