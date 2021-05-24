using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMText : MVVMControl {

		public new Text Control { get; private set; }
		public MVVMBinding Text {

			get {
				return GetBinding (ref _Text, value => Text = value);
			}

			set {
				SetBinding (ref _Text, value, () => Control.text, targetValue => Control.text = MVVMApi.ToString (targetValue));
			}

		}
		public MVVMBinding Color {

			get {
				return GetBinding (ref _Color, value => Color = value);
			}

			set {
				SetBinding (ref _Color, value, () => Control.color, targetValue => Control.color = MVVMApi.ToColor (targetValue));
			}

		}

		MVVMBinding _Text;
		MVVMBinding _Color;

		public MVVMText (Text control) : base (control) {
			Control = control;
		}

	}

}