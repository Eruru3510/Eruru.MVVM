using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMItemsControl : MVVMControl {

		public MVVMItemsControl (Control control) : base (control) {
			OnDefaultDataTemplate = () => new MVVMLabel (new Label () { AutoSize = true }) {
				Text = new MVVMBinding ()
			};
			OnItemsControlInsert = (index, currentControl) => {
				MVVMControl mvvmControl = (MVVMControl)currentControl;
				Control.Controls.Add (mvvmControl.Control);
				Control.Controls.SetChildIndex (mvvmControl.Control, index);
			};
			OnItemsControlRemoveAt = index => Control.Controls.RemoveAt (index);
			OnItemsControlMove = (oldIndex, newIndex) => Control.Controls.SetChildIndex (Control.Controls[oldIndex], newIndex);
			OnItemsControlReset = () => Control.Controls.Clear ();
		}

	}

}