using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMItemsControl : MVVMItemsControlBase {

		public MVVMItemsControl (Control control) : base (control) {

		}
		public MVVMItemsControl (MVVMView view, Control control) : base (view, control) {

		}

		protected override void Add (object value) {
			Control.Controls.Add ((MVVMControl)value);
		}

		protected override void RemoveAt (int index) {
			Control.Controls.RemoveAt (index);
		}

		protected override void Replace (int index, object value) {
			Control.Controls.RemoveAt (index);
			MVVMControl control = (MVVMControl)value;
			Control.Controls.Add (control);
			Control.Controls.SetChildIndex (control, index);
		}

		protected override void Move (int oldIndex, int newIndex) {
			Control.Controls.SetChildIndex (Control.Controls[oldIndex], newIndex);
			Control.Controls.SetChildIndex (Control.Controls[newIndex - 1], oldIndex);
		}

		protected override void Clear () {
			Control.Controls.Clear ();
		}

	}

}