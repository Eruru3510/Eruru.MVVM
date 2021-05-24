using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMInputField : MVVMControl {

		public new InputField Control { get; private set; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, value => Text = value);
			}

			set {
				SetBinding (
					ref _Text, value,
					() => Control.text, targetValue => Control.text = MVVMApi.ToString (targetValue),
					() => Control.onValueChange.AddListener (Control_OnValueChanged), () => Control.onValueChange.RemoveListener (Control_OnValueChanged),
					() => Control.onEndEdit.AddListener (Control_OnEndEdit), () => Control.onEndEdit.RemoveListener (Control_OnEndEdit)
				);
			}

		}

		MVVMBinding _Text;

		public MVVMInputField (InputField control) : base (control) {
			Control = control;
		}

		void Control_OnValueChanged (object value) {
			OnChanged (Text, value);
		}

		void Control_OnEndEdit (object value) {
			OnChanged (Text, value, MVVMBindingOnChangeType.LostFocus);
		}

	}

}