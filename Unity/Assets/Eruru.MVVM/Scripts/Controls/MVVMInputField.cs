using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMInputField : MVVMControl {

		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, value => Text = value);
			}

			set {
				InitializeBinding (ref _Text, value,
					targetValue => Control.text = MVVMApi.ToString (targetValue), () => Control.text,
					() => Control.onValueChange.AddListener (Control_OnValueChange), () => Control.onValueChange.RemoveListener (Control_OnValueChange),
					() => Control.onEndEdit.AddListener (Control_OnEndEdit), () => Control.onEndEdit.RemoveListener (Control_OnEndEdit)
				);
			}

		}

		new InputField Control { get; set; }
		MVVMBinding _Text;

		public MVVMInputField (InputField control) : base (control) {
			Control = control;
		}
		public MVVMInputField (MVVMView view, InputField control) : base (view, control) {
			Control = control;
		}

		void Control_OnValueChange (string value) {
			Changed (Text, value);
		}

		void Control_OnEndEdit (string value) {
			Changed (Text, value, MVVMBindingChangedType.LostFocus);
		}

	}

}