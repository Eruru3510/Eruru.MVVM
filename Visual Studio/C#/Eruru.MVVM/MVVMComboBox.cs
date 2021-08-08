using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMComboBox : MVVMItemsControl {

		public new ComboBox Control { get; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, binding => Text = binding);
			}

			set {
				SetBinding (ref _Text, value, () => Control.Text, targetValue => Control.Text = MVVMAPI.To<string> (targetValue), null, MVVMBindingMode.TwoWay);
			}

		}
		public MVVMBinding SelectedIndex {

			get {
				return GetBinding (ref _SelectedIndex, binding => SelectedIndex = binding);
			}

			set {
				SetBinding (
					ref _SelectedIndex, value,
					() => Control.SelectedIndex, targetValue => Control.SelectedIndex = MVVMAPI.To<int> (targetValue),
					null, MVVMBindingMode.TwoWay
				);
			}

		}
		public MVVMBinding SelectedItem {

			get {
				return GetBinding (ref _SelectedItem, binding => SelectedItem = binding);
			}

			set {
				SetBinding (ref _SelectedItem, value, () => Control.SelectedItem, targetValue => Control.SelectedItem = MVVMAPI.To<int> (targetValue));
			}

		}

		MVVMBinding _Text;
		MVVMBinding _SelectedIndex;
		MVVMBinding _SelectedItem;

		public MVVMComboBox (ComboBox control) : base (control) {
			Control = control;
			Control.TextChanged += Control_TextChanged;
			control.LostFocus += Control_LostFocus;
			Control.SelectedIndexChanged += Control_SelectedIndexChanged;
		}

		protected override object Convert (object value) {
			return value;
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
			object value = Control.Items[oldIndex];
			Control.Items.RemoveAt (oldIndex);
			Control.Items.Insert (newIndex, value);
		}

		protected override void Reset () {
			Control.Items.Clear ();
		}

		private void Control_TextChanged (object sender, EventArgs e) {
			OnChanged (_Text);
		}

		private void Control_LostFocus (object sender, EventArgs e) {
			OnChanged (_Text, MVVMOnChangedType.LostFocus);
		}

		private void Control_SelectedIndexChanged (object sender, EventArgs e) {
			OnChanged (_SelectedIndex);
		}

	}

}