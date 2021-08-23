using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMLabel : MVVMControl {

		public Label Label { get; }

		public MVVMLabel (Label label) : base (label) {
			Label = label;
		}

	}

}