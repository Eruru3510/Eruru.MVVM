using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo.ItemsControl {

	public class ItemView : MonoBehaviour, IMVVMView {

		[SerializeField]
		Text TextName;
		[SerializeField]
		Text TextAge;
		[SerializeField]
		Text TextSchool;
		[SerializeField]
		Text TextRemark;
		[SerializeField]
		Button ButtonEdit;
		[SerializeField]
		Button ButtonDelete;

		public MVVMControl Build () {
			return new MVVMControl (this) {

			}.Add (
				new MVVMText (TextName) {
					Text = new MVVMBinding ("Name")
				},
				new MVVMText (TextAge) {
					Text = new MVVMBinding ("Age")
				},
				new MVVMText (TextSchool) {
					Text = new MVVMBinding ("School")
				},
				new MVVMText (TextRemark) {
					Text = new MVVMBinding ("Remark")
				},
				new MVVMButton (ButtonEdit) {
					OnClick = new MVVMBinding (new MVVMRelativeSource (typeof (MVVMItemsControl)), "DataContext.OnEdit"),
					OnClickParameter = new MVVMBinding ()
				},
				new MVVMButton (ButtonDelete) {
					OnClick = new MVVMBinding (new MVVMRelativeSource (typeof (MVVMItemsControl)), "DataContext.OnDelete"),
					OnClickParameter = new MVVMBinding ()
				}
			).Build ();
		}

	}

}