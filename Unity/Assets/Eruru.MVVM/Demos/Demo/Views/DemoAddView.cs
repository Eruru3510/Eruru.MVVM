using System;
using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo {

	public class DemoAddView : MonoBehaviour, IMVVMView {

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
		Button CancelButton;

		public MVVMControl Control { get; set; }
		public MVVMAction OnLoaded { get; set; }

		public Action OnClosed;

		Animator TextAnimator;

		void Awake () {
			TextAnimator = Text.GetComponent<Animator> ();
		}

		void Start () {
			MVVMInputField mvvmInputField;
			MVVMSlider mvvmSlider = null;
			Control = new MVVMControl (this) {
				DataContext = new MVVMBinding (new DemoAddViewModel ())
			}.Add (
				new MVVMControl (this) {
					DataContext = new MVVMBinding ("Model")
				}.Add (
					mvvmInputField = new MVVMInputField (InputField) {
						Text = new MVVMBinding ("Text", MVVMBindingUpdateSourceTrigger.PropertyChanged)
					},
					new MVVMText (Text) {
						Text = new MVVMBinding (mvvmInputField, "Text")
					}.Add (
						new MVVMStyle ().Add (
							new MVVMSetter ("Color", Color.blue)
						),
						new MVVMTrigger (MVVMTriggerRoutedEvent.MouseEnter).Add (
							new MVVMSetter ("Color", Color.red),
							new MVVMSetter (() => TextAnimator.SetBool ("IsPlaying", true))
						),
						new MVVMTrigger (MVVMTriggerRoutedEvent.MouseLeave).Add (
							new MVVMSetter ("Color", Color.black),
							new MVVMSetter (() => TextAnimator.SetBool ("IsPlaying", false))
						)
					),
					mvvmSlider = (MVVMSlider)new MVVMSlider (Slider) {
						MaxValue = new MVVMBinding ("MaxValue"),
						MinValue = new MVVMBinding ("MinValue"),
						Value = new MVVMBinding ("Value", MVVMBindingMode.OneWayToSource)
					}.Add (
						new MVVMTrigger (new MVVMBinding ("Value"), value => MVVMAPI.ToFloat (value) <= 0.5).Add (
							new MVVMSetter ("HandleRectColor", Color.red)
						),
						new MVVMTrigger (new MVVMBinding (mvvmSlider, "Value"), value => {
							float floatValue = MVVMAPI.ToFloat (value);
							return floatValue > 0.5 && floatValue < 1.5;
						}).Add (
							new MVVMSetter ("HandleRectColor", Color.yellow)
						),
						new MVVMTrigger ("Value", value => MVVMAPI.ToFloat (value) >= 1.5).Add (
							new MVVMSetter ("HandleRectColor", Color.green)
						)
					),
					new MVVMDropdown (Dropdown) {
						ItemsSource = new MVVMBinding ("Options"),
						Value = new MVVMBinding (mvvmSlider, "Value", MVVMBindingMode.OneWay)
					},
					new MVVMToggle (Toggle) {
						IsOn = new MVVMBinding ("IsOn")
					},
					new MVVMScrollBar (Scrollbar) {
						Value = new MVVMBinding (mvvmSlider, "Value", MVVMBindingMode.OneWay)
					}
				),
				new MVVMButton (Button) {
					OnClick = new MVVMBinding ("OnClick"),
					OnClickParameter = new MVVMBinding ()
				}
			);
			Button.onClick.AddListener (() => {
				Close ();
			});
			CancelButton.onClick.AddListener (() => {
				Close ();
			});
			if (OnLoaded != null) {
				OnLoaded ();
			}
		}

		void Close () {
			Destroy (gameObject);
			if (OnClosed != null) {
				OnClosed ();
			}
		}

	}

}