using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMLabel : MVVMControl {

		public new Label Control { get; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, binding => Text = binding);
			}

			set {
				SetBinding (ref _Text, value, () => Control.Text, targetValue => Control.Text = MVVMAPI.To<string> (targetValue));
			}

		}

		MVVMBinding _Text;

		public MVVMLabel (Label control) : base (control) {
			Control = control;
		}

	}

}