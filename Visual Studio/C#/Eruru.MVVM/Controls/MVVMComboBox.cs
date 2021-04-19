using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMComboBox : MVVMItemsControl {

		new readonly ComboBox Control;

		public MVVMComboBox (ComboBox control) : base (control) {
			Control = control;
		}
		public MVVMComboBox (MVVMView view, ComboBox control) : base (view, control) {
			Control = control;
		}

		protected override void Add (object value) {
			Control.Items.Add (value);
		}

		protected override void RemoveAt (int index) {
			Control.Items.RemoveAt (index);
		}

		protected override void Replace (int index, object value) {
			Control.Items[index] = value;
		}

		protected override void Move (int oldIndex, int newIndex) {
			Control.Items.Swap (oldIndex, newIndex);
		}

		protected override void Clear () {
			Control.Items.Clear ();
		}

	}

}