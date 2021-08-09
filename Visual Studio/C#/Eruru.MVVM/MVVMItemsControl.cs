using System.Collections;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMItemsControl : MVVMControl {

		public MVVMItemsControl (Control control) : base (control) {
			OnUseDefaultDataTemplate = () => new MVVMLabel (new Label () { AutoSize = true }) {
				Text = new MVVMBinding ()
			};
			OnInsert = (index, currentControl) => {
				Control.Controls.Add (currentControl.Control);
				Control.Controls.SetChildIndex (currentControl.Control, index);
			};
			OnRemoveAt = index => Control.Controls.RemoveAt (index);
			OnMove = (oldIndex, newIndex) => Control.Controls.SetChildIndex (Control.Controls[oldIndex], newIndex);
			OnReset = () => Control.Controls.Clear ();
		}

	}

}