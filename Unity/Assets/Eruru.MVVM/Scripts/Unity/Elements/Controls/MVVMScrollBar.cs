using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMScrollBar : MVVMControl {

		public new Scrollbar Control { get; private set; }
		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, value => Value = value);
			}

			set {
				SetBinding (
					ref _Value, value,
					() => Control.value, targetValue => Control.value = MVVMApi.ToFloat (targetValue),
					() => Control.onValueChanged.AddListener (Control_OnValueChanged), () => Control.onValueChanged.RemoveListener (Control_OnValueChanged)
				);
			}

		}
		public MVVMBinding Size {

			get {
				return GetBinding (ref _Size, value => Size = value);
			}

			set {
				SetBinding (ref _Size, value, () => Control.size, targetValue => Control.size = MVVMApi.ToFloat (targetValue));
			}

		}
		public MVVMBinding NumberOfSteps {

			get {
				return GetBinding (ref _NumberOfSteps, value => NumberOfSteps = value);
			}

			set {
				SetBinding (ref _NumberOfSteps, value, () => Control.numberOfSteps, targetValue => Control.numberOfSteps = MVVMApi.ToInt (targetValue));
			}

		}

		MVVMBinding _Value;
		MVVMBinding _Size;
		MVVMBinding _NumberOfSteps;

		public MVVMScrollBar (Scrollbar control) : base (control) {
			Control = control;
		}

		void Control_OnValueChanged (float value) {
			OnChanged (Value, value);
		}

	}

}