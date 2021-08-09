using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMInputField : MVVMControl {

		public new InputField Control { get; private set; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, value => Text = value);
			}

			set {
				SetBinding (ref _Text, value, () => Control.text, targetValue => Control.text = MVVMAPI.To<string> (targetValue),
					defaultMode: MVVMBindingMode.TwoWay, defaultUpdateSourceTrigger: MVVMUpdateSourceTrigger.LostFocus
				);
			}

		}

		MVVMBinding _Text;

		public MVVMInputField (InputField control) : base (control) {
			Control = control;
			Control.onValueChange.AddListener (Control_OnValueChanged);
			Control.onEndEdit.AddListener (Control_OnEndEdit);
		}

		void Control_OnValueChanged (string value) {
			OnChanged (_Text);
		}

		void Control_OnEndEdit (string value) {
			OnChanged (_Text, MVVMOnChangedType.LostFocus);
		}

	}

}