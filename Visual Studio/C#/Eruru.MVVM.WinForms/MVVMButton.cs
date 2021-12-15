using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMButton : MVVMControl {

		public Button Button { get; }

		public MVVMButton (Button button) : base (button) {
			Button = button;
		}

	}

}