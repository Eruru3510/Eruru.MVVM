using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMButton : MVVMControl {

		public new Button Control { get; private set; }
		public MVVMBinding OnClick {

			get {
				return GetBinding (ref _OnClick, value => OnClick = value);
			}

			set {
				SetBinding (ref _OnClick, value, null, null, () => Control.onClick.AddListener (Control_OnClick), () => Control.onClick.RemoveListener (Control_OnClick));
			}

		}
		public MVVMBinding OnClickParameter {

			get {
				return GetBinding (ref _OnClickParameter, value => OnClickParameter = value);
			}

			set {
				SetBinding (ref _OnClickParameter, value);
			}

		}

		MVVMBinding _OnClick;
		MVVMBinding _OnClickParameter;

		public MVVMButton (Button control) : base (control) {
			Control = control;
		}

		void Control_OnClick () {
			OnCommand (OnClick, OnClickParameter);
		}

	}

}