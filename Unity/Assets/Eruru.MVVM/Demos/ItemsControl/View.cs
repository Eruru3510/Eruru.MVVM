using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo.ItemsControl {

	public class View : MonoBehaviour {

		[SerializeField]
		ScrollRect ScrollRect;
		[SerializeField]
		ItemView ItemPrefab;
		[SerializeField]
		Button ButtonAdd;

		void Start () {
			new MVVMControl (this) {
				DataContext = new MVVMBinding (new ViewModel ())
			}.Add (
				new MVVMScrollRect (ScrollRect) {
					ItemsSource = new MVVMBinding ("Items"),
					ItemTemplate = new MVVMDataTemplate (() => Instantiate (ItemPrefab).Build ())
				},
				new MVVMButton (ButtonAdd) {
					OnClick = new MVVMBinding ("OnAdd")
				}
			).Build ();
		}

	}

}