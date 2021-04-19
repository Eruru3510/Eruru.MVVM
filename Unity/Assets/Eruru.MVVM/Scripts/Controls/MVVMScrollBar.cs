using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMScrollBar : MVVMControl {

		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, value => Value = value);
			}

			set {
				InitializeBinding (ref _Value, value,
					targetValue => Control.value = MVVMApi.ToFloat (targetValue), () => Control.value,
					() => Control.onValueChanged.AddListener (Control_OnValueChanged), () => Control.onValueChanged.RemoveListener (Control_OnValueChanged)
				);
			}

		}
		public MVVMBinding Size {

			get {
				return GetBinding (ref _Size, value => Value = value);
			}

			set {
				InitializeBinding (ref _Size, value,
					targetValue => Control.size = MVVMApi.ToFloat (targetValue), () => Control.size
				);
			}

		}
		public MVVMBinding NumberOfSteps {

			get {
				return GetBinding (ref _NumberOfSteps, value => Value = value);
			}

			set {
				InitializeBinding (ref _NumberOfSteps, value,
					targetValue => Control.numberOfSteps = MVVMApi.ToInt (targetValue), () => Control.numberOfSteps
				);
			}

		}

		new Scrollbar Control { get; set; }
		MVVMBinding _Value;
		MVVMBinding _Size;
		MVVMBinding _NumberOfSteps;

		public MVVMScrollBar (Scrollbar control) : base (control) {
			Control = control;
		}
		public MVVMScrollBar (MVVMView view, Scrollbar control) : base (view, control) {
			Control = control;
		}

		void Control_OnValueChanged (float value) {
			Changed (Value, value);
		}

	}

}