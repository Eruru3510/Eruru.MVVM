using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMText : MVVMControl {

		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, value => Text = value);
			}

			set {
				InitializeBinding (ref _Text, value,
					targetValue => Control.text = MVVMApi.ToString (targetValue), () => Control.text
				);
			}

		}

		new Text Control { get; set; }
		MVVMBinding _Text;

		public MVVMText (Text control) : base (control) {
			Control = control;
		}
		public MVVMText (MVVMView view, Text control) : base (view, control) {
			Control = control;
		}

	}

}