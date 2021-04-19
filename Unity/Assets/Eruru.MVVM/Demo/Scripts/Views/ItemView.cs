using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo {

	public class ItemView : MVVMView {

		[SerializeField]
		Text Text;

		void Awake () {
			new MVVMText (this, Text) { Text = new MVVMBindingSource () };
		}

	}

}