using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public partial class MVVMItemsControl {

		protected Transform ItemParent;

		public MVVMItemsControl (UIBehaviour control) : base (control) {
			ItemParent = control.transform;
			OnUseDefaultDataTemplate = () => {
				Text text = new GameObject ().AddComponent<Text> ();
				text.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
				return new MVVMText (text) {
					Text = new MVVMBinding ()
				};
			};
			OnInsert = (index, currentControl) => {
				currentControl.Control.transform.SetParent (ItemParent);
				currentControl.Control.transform.SetSiblingIndex (index);
			};
			OnRemoveAt = index => Object.Destroy (ItemParent.GetChild (index).gameObject);
			OnMove = (oldIndex, newIndex) => ItemParent.GetChild (oldIndex).SetSiblingIndex (newIndex);
			OnReset = () => {
				while (ItemParent.childCount > 0) {
					OnRemoveAt (ItemParent.childCount - 1);
				}
			};
		}

	}

}