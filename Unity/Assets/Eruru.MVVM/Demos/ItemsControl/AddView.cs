using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Eruru.MVVM.Demo.ItemsControl {

	public class AddView : MonoBehaviour, IMVVMView {

		[SerializeField]
		InputField InputFieldName;
		[SerializeField]
		InputField InputFieldAge;
		[SerializeField]
		Dropdown DropdownSchool;
		[SerializeField]
		InputField InputFieldRemark;
		[SerializeField]
		Button ButtonConfirm;
		[SerializeField]
		Button ButtonCancel;

		public Item Item;
		public MVVMAction OnConfirm;

		void Start () {
			ButtonConfirm.onClick.AddListener (() => {
				if (OnConfirm != null) {
					OnConfirm ();
				}
				Destroy (gameObject);
			});
			ButtonCancel.onClick.AddListener (() => {
				Destroy (gameObject);
			});
		}

		public MVVMControl Build () {
			return new MVVMControl (this) {
				DataContext = new MVVMBinding (new AddViewModel () { Item = Item })
			}.Add (
				new MVVMControl (this) {
					DataContext = new MVVMBinding ("Item")
				}.Add (
					new MVVMInputField (InputFieldName) {
						Text = new MVVMBinding ("Name")
					},
					new MVVMInputField (InputFieldAge) {
						Text = new MVVMBinding ("Age")
					},
					new MVVMDropdown (DropdownSchool) {
						ItemsSource = new MVVMBinding (new MVVMBindingRelativeSource (3), "DataContext.Schools"),
						Text = new MVVMBinding ("School")
					},
					new MVVMInputField (InputFieldRemark) {
						Text = new MVVMBinding ("Remark")
					}
				)
			).Build ();
		}

	}

}