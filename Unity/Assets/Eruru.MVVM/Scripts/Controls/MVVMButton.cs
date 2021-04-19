using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMButton : MVVMControl {

		public MVVMBinding OnClick {

			get {
				return GetBinding (ref _OnClick, value => OnClick = value);
			}

			set {
				InitializeBinding (ref _OnClick, value, null, null,
					() => Control.onClick.AddListener (Control_OnClick), () => Control.onClick.RemoveListener (Control_OnClick)
				);
			}

		}
		public MVVMBinding OnClickParameter {

			get {
				return GetBinding (ref _OnClickParameter, value => OnClickParameter = value);
			}

			set {
				InitializeBinding (ref _OnClickParameter, value);
			}

		}

		new Button Control { get; set; }
		MVVMBinding _OnClick;
		MVVMBinding _OnClickParameter;

		public MVVMButton (Button control) : base (control) {
			Control = control;
		}
		public MVVMButton (MVVMView view, Button control) : base (view, control) {
			Control = control;
		}

		void Control_OnClick () {
			Command (OnClick, OnClickParameter);
		}

	}

}