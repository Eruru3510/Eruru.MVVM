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
					() => Control.onValueChange.AddListener (Control_OnValueChange), () => Control.onValueChange.RemoveListener (Control_OnValueChange)
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

	}

}