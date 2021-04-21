using Eruru.MVVM;
using Eruru.MVVM.Demo;
using UnityEngine;
using UnityEngine.UI;

public class View : MVVMView {

	[SerializeField]
	Text Text;
	[SerializeField]
	Image Image;
	[SerializeField]
	RawImage RawImage;
	[SerializeField]
	Button Button;
	[SerializeField]
	Toggle Toggle;
	[SerializeField]
	Slider Slider;
	[SerializeField]
	Scrollbar Scrollbar;
	[SerializeField]
	Dropdown Dropdown;
	[SerializeField]
	InputField InputField;
	[SerializeField]
	InputField InputField1;
	[SerializeField]
	ScrollRect ScrollRect;

	void Awake () {
		DataContext = new ViewModel ();
		MVVMInputField inputField = new MVVMInputField (this, InputField) {
			Text = new MVVMBinding ("User.Account")
		};
		MVVMInputField inputField1 = new MVVMInputField (this, InputField1) {
			Text = new MVVMBinding (inputField, "Text", MVVMBindingUpdateSourceTrigger.Explicit)
		};
		new MVVMButton (this, Button) {
			OnClick = new MVVMBinding ("OnClick")
		};
		Button.onClick.AddListener (() => {
			inputField1.Text.UpdateSource ();
		});
	}

}