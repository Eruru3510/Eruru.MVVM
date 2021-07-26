using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMDropdown : MVVMItemsControlBase {

		public new Dropdown Control { get; private set; }
		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, value => Value = value);
			}

			set {
				SetBinding (
					ref _Value, value,
					() => Control.value, targetValue => Control.value = MVVMAPI.ToInt (targetValue),
					() => Control.onValueChanged.AddListener (Control_OnValueChanged), () => Control.onValueChanged.RemoveListener (Control_OnValueChanged)
				);
			}

		}

		MVVMBinding _Value;

		public MVVMDropdown (Dropdown control) : base (control) {
			Control = control;
		}

		protected override object Convert (object value) {
			if (value is Dropdown.OptionData) {
				return value;
			}
			return new Dropdown.OptionData (MVVMAPI.ToString (value));
		}

		protected override void Add (object value) {
			Control.options.Add ((Dropdown.OptionData)value);
		}

		protected override void Clear () {
			Control.options.Clear ();
		}

		protected override void Move (int oldIndex, int newIndex) {
			Dropdown.OptionData temp = Control.options[oldIndex];
			Control.options[oldIndex] = Control.options[newIndex];
			Control.options[newIndex] = temp;
		}

		protected override void RemoveAt (int index) {
			Control.options.RemoveAt (index);
		}

		protected override void Replace (int index, object value) {
			Control.options[index] = (Dropdown.OptionData)value;
		}

		void Control_OnValueChanged (int value) {
			OnChanged (Value, value);
		}

	}

}