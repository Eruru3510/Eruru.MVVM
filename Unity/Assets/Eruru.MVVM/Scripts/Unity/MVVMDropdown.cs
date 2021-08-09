using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMDropdown : MVVMItemsControl {

		public new Dropdown Control { get; private set; }
		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, binding => Value = binding);
			}

			set {
				SetBinding (ref _Value, value, () => Control.value, targetValue => Control.value = MVVMAPI.To<int> (targetValue), defaultMode: MVVMBindingMode.TwoWay);
			}

		}
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, binding => Text = binding);
			}

			set {
				SetBinding (ref _Text, value, () => Control.captionText.text, targetValue => {
					string text = MVVMAPI.To<string> (targetValue);
					for (int i = 0; i < Control.options.Count; i++) {
						if (Control.options[i].text == text) {
							Control.value = i;
							break;
						}
					}
				}, defaultMode: MVVMBindingMode.TwoWay);
			}

		}

		MVVMBinding _Value;
		MVVMBinding _Text;

		public MVVMDropdown (Dropdown control) : base (control) {
			Control = control;
			Control.onValueChanged.AddListener (Control_OnValueChanged);
		}

		protected override object Convert (object value) {
			if (value is Dropdown.OptionData) {
				return value;
			}
			if (value is Sprite) {
				return new Dropdown.OptionData ((Sprite)value);
			}
			return new Dropdown.OptionData (value == null ? string.Empty : value.ToString ());
		}

		protected override void Insert (int index, object value) {
			Control.options.Insert (index, (Dropdown.OptionData)Convert (value));
		}

		protected override void Add (object value) {
			Control.options.Add ((Dropdown.OptionData)Convert (value));
		}

		protected override void RemoveAt (int index) {
			Control.options.RemoveAt (index);
		}

		protected override void Replace (int index, object value) {
			Control.options[index] = (Dropdown.OptionData)Convert (value);
		}

		protected override void Move (int oldIndex, int newIndex) {
			Dropdown.OptionData value = Control.options[oldIndex];
			Control.options.RemoveAt (oldIndex);
			Control.options.Insert (newIndex, value);
		}

		protected override void Reset () {
			Control.options.Clear ();
		}

		void Control_OnValueChanged (int value) {
			OnChanged (_Value);
			OnChanged (_Text);
		}

	}

}