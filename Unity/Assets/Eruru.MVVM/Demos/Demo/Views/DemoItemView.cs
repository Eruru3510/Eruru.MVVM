using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo {

	public class DemoItemView : MonoBehaviour, IMVVMView {

		[SerializeField]
		Text Text;
		[SerializeField]
		InputField InputField;
		[SerializeField]
		Dropdown Dropdown;
		[SerializeField]
		Toggle Toggle;
		[SerializeField]
		Slider Slider;
		[SerializeField]
		Scrollbar Scrollbar;
		[SerializeField]
		Button Button;
		[SerializeField]
		Button DeleteButton;

		public MVVMControl Control { get; set; }
		public MVVMAction OnLoaded { get; set; }

		void Start () {
			MVVMInputField mvvmInputField;
			MVVMSlider mvvmSlider;
			Control = new MVVMControl (this).Add (
				mvvmInputField = new MVVMInputField (InputField) {
					Text = new MVVMBinding ("Text", MVVMBindingUpdateSourceTrigger.PropertyChanged)
				},
				new MVVMText (Text) {
					Text = new MVVMBinding (mvvmInputField, "Text")
				},
				mvvmSlider = new MVVMSlider (Slider) {
					MaxValue = new MVVMBinding ("MaxValue"),
					MinValue = new MVVMBinding ("MinValue"),
					Value = new MVVMBinding ("Value")
				},
				new MVVMDropdown (Dropdown) {
					ItemsSource = new MVVMBinding ("Options"),
					Value = new MVVMBinding (mvvmSlider, "Value", MVVMBindingMode.OneWay)
				},
				new MVVMToggle (Toggle) {
					IsOn = new MVVMBinding ("IsOn")
				},
				new MVVMScrollBar (Scrollbar) {
					Value = new MVVMBinding (mvvmSlider, "Value", MVVMBindingMode.OneWay)
				},
				new MVVMButton (Button) {
					OnClick = new MVVMBinding (new MVVMBindingRelativeSource (typeof (MVVMScrollRect)), "DataContext.OnEdit"),
					OnClickParameter = new MVVMBinding (new MVVMBindingRelativeSource (typeof (MVVMControl), 2))
				},
				new MVVMButton (DeleteButton) {
					OnClick = new MVVMBinding (new MVVMBindingRelativeSource (typeof (MVVMScrollRect)), "DataContext.OnDelete"),
					OnClickParameter = new MVVMBinding ()
				}
			);
			if (OnLoaded != null) {
				OnLoaded ();
			}
		}

	}

}