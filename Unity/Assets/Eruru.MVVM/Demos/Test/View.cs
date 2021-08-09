using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo.Test {

	public class View : MonoBehaviour {

		[SerializeField]
		Text Text;
		[SerializeField]
		InputField InputField;

		void Start () {
			new MVVMControl (this) {
				DataContext = new MVVMBinding (new ViewModel ())
			}.Add (
				new MVVMText (Text) {
					Text = new MVVMBinding ("Text")
				},
				new MVVMInputField (InputField) {
					Text = new MVVMBinding ("Text")
				}
			).Build ();
		}

	}

}