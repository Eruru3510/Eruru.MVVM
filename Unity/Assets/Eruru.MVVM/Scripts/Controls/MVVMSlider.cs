using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMSlider : MVVMControl {

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
		public MVVMBinding MaxValue {

			get {
				return GetBinding (ref _MaxValue, value => Value = value);
			}

			set {
				InitializeBinding (ref _MaxValue, value,
					targetValue => Control.maxValue = MVVMApi.ToFloat (targetValue), () => Control.maxValue
				);
			}

		}
		public MVVMBinding MinValue {

			get {
				return GetBinding (ref _MinValue, value => Value = value);
			}

			set {
				InitializeBinding (ref _MinValue, value,
					targetValue => Control.minValue = MVVMApi.ToFloat (targetValue), () => Control.minValue
				);
			}

		}

		new Slider Control { get; set; }
		MVVMBinding _Value;
		MVVMBinding _MaxValue;
		MVVMBinding _MinValue;

		public MVVMSlider (Slider control) : base (control) {
			Control = control;
		}
		public MVVMSlider (MVVMView view, Slider control) : base (view, control) {
			Control = control;
		}

		void Control_OnValueChanged (float value) {
			Changed (Value, value);
		}

	}

}