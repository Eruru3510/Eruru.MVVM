using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMComboBox : MVVMItemsControl {

		public ComboBox ComboBox { get; }
		public MVVMBinding SelectedIndex {

			get {
				return GetBinding (ref _SelectedIndex, binding => SelectedIndex = binding);
			}

			set {
				SetBinding (ref _SelectedIndex, value, () => ComboBox.SelectedIndex, targetValue => ComboBox.SelectedIndex = MVVMAPI.To<int> (targetValue), true);
			}

		}
		public MVVMBinding SelectedItem {

			get {
				return GetBinding (ref _SelectedItem, binding => SelectedItem = binding);
			}

			set {
				SetBinding (ref _SelectedItem, value, () => ComboBox.SelectedItem, targetValue => ComboBox.SelectedItem = targetValue);
			}

		}

		MVVMBinding _SelectedIndex;
		MVVMBinding _SelectedItem;

		public MVVMComboBox (ComboBox comboBox) : base (comboBox) {
			ComboBox = comboBox;
			ComboBox.SelectedIndexChanged += (sender, e) => OnChanged (_SelectedIndex);
		}

		protected override void OnSetText (MVVMBinding text) {
			text.DefaultMode = MVVMBindingMode.TwoWay;
			text.DefaultUpdateSourceTrigger = MVVMUpdateSourceTrigger.PropertyChanged;
		}

		protected override object Convert (object value) {
			return value;
		}

		protected override void Insert (int index, object value) {
			ComboBox.Items.Insert (index, value);
		}

		protected override void Add (object value) {
			ComboBox.Items.Add (value);
		}

		protected override void RemoveAt (int index) {
			ComboBox.Items.RemoveAt (index);
		}

		protected override void Replace (int index, object value) {
			ComboBox.Items[index] = value;
		}

		protected override void Move (int oldIndex, int newIndex) {
			object value = ComboBox.Items[oldIndex];
			ComboBox.Items.RemoveAt (oldIndex);
			ComboBox.Items.Insert (newIndex, value);
		}

		protected override void Reset () {
			ComboBox.Items.Clear ();
		}

	}

}