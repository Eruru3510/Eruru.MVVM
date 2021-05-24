using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo {

	public class DemoView : MonoBehaviour, IMVVMView {

		[SerializeField]
		ScrollRect ScrollRect;
		[SerializeField]
		Button Button;
		[SerializeField]
		GameObject ItemPrefab;

		public MVVMControl Control { get; set; }
		public MVVMAction OnLoaded { get; set; }

		void Start () {
			Control = new MVVMControl (this) {
				DataContext = new MVVMBinding (new DemoViewModel ())
			}.Add (
				new MVVMScrollRect (ScrollRect) {
					DataTemplate = value => Instantiate (ItemPrefab),
					ItemsSource = new MVVMBinding ("Models")
				},
				new MVVMButton (Button) {
					OnClick = new MVVMBinding ("OnClick")
				}
			);
			if (OnLoaded != null) {
				OnLoaded ();
			}
		}

	}

}