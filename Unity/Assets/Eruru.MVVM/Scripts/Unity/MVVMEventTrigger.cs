using UnityEngine;
using UnityEngine.EventSystems;

namespace Eruru.MVVM {

	public class MVVMEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		internal event MVVMAction OnMouseEnter, OnMouseLeave;

		public void OnPointerEnter (PointerEventData eventData) {
			if (OnMouseEnter == null) {
				return;
			}
			OnMouseEnter ();
		}

		public void OnPointerExit (PointerEventData eventData) {
			if (OnMouseLeave == null) {
				return;
			}
			OnMouseLeave ();
		}

	}

}