using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMSlider : MVVMControl {

		public new Slider Control { get; private set; }
		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, value => Value = value);
			}

			set {
				SetBinding (
					ref _Value, value,
					() => Control.value, targetValue => Control.value = MVVMAPI.ToFloat (targetValue),
					() => Control.onValueChanged.AddListener (Control_OnValueChanged), () => Control.onValueChanged.RemoveListener (Control_OnValueChanged)
				);
			}

		}
		public MVVMBinding MaxValue {

			get {
				return GetBinding (ref _MaxValue, value => MaxValue = value);
			}

			set {
				SetBinding (ref _MaxValue, value, () => Control.maxValue, targetValue => Control.maxValue = MVVMAPI.ToFloat (targetValue));
			}

		}
		public MVVMBinding MinValue {

			get {
				return GetBinding (ref _MinValue, value => MinValue = value);
			}

			set {
				SetBinding (ref _MinValue, value, () => Control.minValue, targetValue => Control.minValue = MVVMAPI.ToFloat (targetValue));
			}

		}
		public MVVMBinding HandleRectColor {

			get {
				return GetBinding (ref _HandleRectColor, value => HandleRectColor = value);
			}

			set {
				SetBinding (ref _HandleRectColor, value, () => HandleRectImage.color, targetValue => HandleRectImage.color = MVVMAPI.ToColor (targetValue));
			}

		}

		Image HandleRectImage;
		MVVMBinding _Value;
		MVVMBinding _MaxValue;
		MVVMBinding _MinValue;
		MVVMBinding _HandleRectColor;

		public MVVMSlider (Slider control) : base (control) {
			Control = control;
			HandleRectImage = Control.handleRect.GetComponent<Image> ();
		}

		void Control_OnValueChanged (float value) {
			OnChanged (Value, value);
		}

	}

}