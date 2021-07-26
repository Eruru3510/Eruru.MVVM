using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMItemsControl : MVVMControl {

		public MVVMItemsControl (Control control) : base (control) {

		}

		MVVMControl Register (object value, int index) {
			MVVMControl control = (MVVMControl)Convert (value);
			base.Add (control, index);
			control.DataContext = new MVVMBinding (value);
			control.Build (Root);
			Control.Controls.Add (control.Control);
			Control.Controls.SetChildIndex (control.Control, index);
			return control;
		}

		protected virtual void Add (object value, int index) {
			Register (value, index);
		}

		protected virtual void Add (object value) {
			Add (value, Controls.Count);
		}

		protected virtual void Remove (int index) {
			RemoveAt (index);
			Control.Controls.RemoveAt (index);
		}

		protected virtual void Replace (int index, object value) {
			Remove (index);
			Add (value, index);
		}

		protected new virtual void Move (int oldIndex, int newIndex) {
			base.Move (oldIndex, newIndex);
			Control.Controls.SetChildIndex (Control.Controls[oldIndex], newIndex);
		}

		protected virtual void Reset () {
			Clear ();
			Control.Controls.Clear ();
		}

	}

}