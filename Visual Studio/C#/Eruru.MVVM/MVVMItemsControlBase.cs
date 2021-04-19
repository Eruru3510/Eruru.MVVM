using System.Windows.Forms;

namespace Eruru.MVVM {

	public abstract partial class MVVMItemsControlBase {

		public MVVMItemsControlBase (Control control) : base (control) {

		}
		public MVVMItemsControlBase (MVVMView view, Control control) : base (view, control) {

		}

	}

}