using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMScrollRect : MVVMItemsControlBase {

		public new ScrollRect Control { get; private set; }

		public MVVMScrollRect (ScrollRect control) : base (control) {
			Control = control;
		}

		protected override object Convert (object value) {
			return value;
		}

		protected override void Add (object value) {
			IMVVMView view = ((GameObject)DataTemplate (value)).GetComponent<IMVVMView> ();
			view.OnLoaded = () => {
				view.Control.Parent = this;
				view.Control.DataContext.SetTargetValue (value);
				view.Control.Control.transform.SetParent (Control.content);
			};
		}

		protected override void Clear () {
			for (int i = 0; i < Control.content.childCount; i++) {
				RemoveAt (i);
			}
		}

		protected override void Move (int oldIndex, int newIndex) {
			Control.content.GetChild (newIndex).SetSiblingIndex (oldIndex);
			Control.content.GetChild (oldIndex + 1).SetSiblingIndex (newIndex);
		}

		protected override void RemoveAt (int index) {
			Object.Destroy (Control.content.GetChild (index).gameObject);
		}

		protected override void Replace (int index, object value) {
			Control.content.GetChild (index).GetComponent<IMVVMView> ().Control.DataContext.SetTargetValue (value);
		}

	}

}