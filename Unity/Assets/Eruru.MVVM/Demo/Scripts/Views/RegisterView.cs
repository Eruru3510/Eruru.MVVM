using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo {

	public class RegisterView : MVVMView {

		[SerializeField]
		InputField AccountInputField;
		[SerializeField]
		InputField PasswordInputField;
		[SerializeField]
		Dropdown Dropdown;
		[SerializeField]
		ScrollRect ScrollRect;
		[SerializeField]
		Button ResetButton;
		[SerializeField]
		MVVMView ItemTemplate;
		[SerializeField]
		InputField TextInputField;
		[SerializeField]
		Sprite[] Sprites;

		RegisterViewModel RegisterViewModel = new RegisterViewModel ();
		int Operate;

		void Awake () {
			DataContext = RegisterViewModel;
			new MVVMControl (this, this) { DataContext = new MVVMBindingSource ("User") }.Add (
				new MVVMInputField (AccountInputField) { Text = new MVVMBindingSource ("Account") },
				new MVVMInputField (PasswordInputField) { Text = new MVVMBindingSource ("Password") }
			);
			new MVVMInputField (this, TextInputField) { Text = new MVVMBindingSource ("Text") };
			new MVVMButton (this, ResetButton) { OnClick = new MVVMBindingSource ("ResetCommand") };
			new MVVMScrollRect (this, ScrollRect) {
				DataTemplate = value => Instantiate (ItemTemplate),
				ItemsSource = new MVVMBindingSource ("Items")
			};
			new MVVMDropdown (this, Dropdown) { ItemsSource = new MVVMBindingSource ("Options") };
			StartCoroutine (A ());
		}

		IEnumerator A () {
			yield return new WaitForSeconds (1);
			RegisterViewModel.Items.Add ("D");//ABCD
			RegisterViewModel.Items.RemoveAt (1);//ACD
			RegisterViewModel.Items.Move (1, 2);//ADC
			RegisterViewModel.Items[1] = RegisterViewModel.Items[2];//ACC
		}

	}

}