using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMToggle : MVVMControl {

		public new Toggle Control { get; private set; }
		public MVVMBinding IsOn {

			get {
				return GetBinding (ref _IsOn, value => IsOn = value);
			}

			set {
				SetBinding (
					ref _IsOn, value,
					() => Control.isOn, targetValue => Control.isOn = MVVMApi.ToBool (targetValue),
					() => Control.onValueChanged.AddListener (Control_OnValueChanged), () => Control.onValueChanged.RemoveListener (Control_OnValueChanged)
				);
			}

		}

		MVVMBinding _IsOn;

		public MVVMToggle (Toggle control) : base (control) {
			Control = control;
		}

		void Control_OnValueChanged (bool isOn) {
			OnChanged (IsOn, isOn);
		}

	}

}