using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMToggle : MVVMControl {

		public MVVMBinding IsOn {

			get {
				return GetBinding (ref _IsOn, value => IsOn = value);
			}

			set {
				InitializeBinding (ref _IsOn, value,
					targetValue => Control.isOn = MVVMApi.ToBool (targetValue), () => Control.isOn,
					() => Control.onValueChanged.AddListener (Control_OnValueChanged), () => Control.onValueChanged.RemoveListener (Control_OnValueChanged)
				);
			}

		}

		new Toggle Control { get; set; }
		MVVMBinding _IsOn;

		public MVVMToggle (Toggle control) : base (control) {
			Control = control;
		}
		public MVVMToggle (MVVMView view, Toggle control) : base (view, control) {
			Control = control;
		}

		void Control_OnValueChanged (bool value) {
			Changed (IsOn, value);
		}

	}

}