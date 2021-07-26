using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMControl {

		public Control Control { get; }

		public MVVMControl (Control control) {
			Control = control;
		}

	}

}