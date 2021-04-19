using System.Collections.Generic;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMScrollRect : MVVMItemsControlBase {

		new ScrollRect Control { get; set; }
		List<MVVMView> Items = new List<MVVMView> ();
		int Count;

		public MVVMScrollRect (ScrollRect control) : base (control) {
			Control = control;
			Initialize ();
		}
		public MVVMScrollRect (MVVMView view, ScrollRect control) : base (view, control) {
			Control = control;
			Initialize ();
		}

		void Initialize () {
			Items.Clear ();
			Items.AddRange (Control.content.GetComponentsInChildren<MVVMView> ());
			foreach (MVVMView item in Items) {
				item.gameObject.SetActive (false);
			}
		}

		protected override object ToItem (object value) {
			return value;
		}

		protected override void Add (object value) {
			MVVMView item;
			if (Count < Items.Count) {
				item = Items[Count];
			} else {
				item = DataTemplate == null ? null : DataTemplate (value) as MVVMView;
				Items.Add (item);
				item.transform.SetParent (Control.content);
			}
			item.DataContext = value;
			item.gameObject.SetActive (true);
			Count++;
		}

		protected override void RemoveAt (int index) {
			Items[index].gameObject.SetActive (false);
			Items[index].transform.SetSiblingIndex (Count - 1);
			MVVMView temp = Items[index];
			for (int i = index + 1; i < Count; i++) {
				Items[i - 1] = Items[i];
			}
			Items[Count - 1] = temp;
			Count--;
		}

		protected override void Replace (int index, object value) {
			Items[index].DataContext = value;
		}

		protected override void Move (int oldIndex, int newIndex) {
			object temp = Items[oldIndex].DataContext;
			Items[oldIndex].DataContext = Items[newIndex].DataContext;
			Items[newIndex].DataContext = temp;
		}

		protected override void Clear () {
			for (int i = 0; i < Count; i++) {
				Items[i].gameObject.SetActive (false);
			}
			Count = 0;
		}

	}

}