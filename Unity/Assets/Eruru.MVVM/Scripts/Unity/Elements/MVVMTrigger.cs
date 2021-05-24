using UnityEngine;

namespace Eruru.MVVM {

	public partial class MVVMTrigger {

		MVVMEventTrigger MVVMEventTrigger;

		internal void BindEvent () {
			if (MVVMEventTrigger != null) {
				MVVMEventTrigger.OnMouseEnter -= OnMouseEnter;
				MVVMEventTrigger.OnMouseLeave -= OnMouseLeave;
			}
			MVVMEventTrigger = Control.Control.GetComponent<MVVMEventTrigger> ();
			if (MVVMEventTrigger == null) {
				MVVMEventTrigger = Control.Control.gameObject.AddComponent<MVVMEventTrigger> ();
			}
			MVVMEventTrigger.OnMouseEnter += OnMouseEnter;
			MVVMEventTrigger.OnMouseLeave += OnMouseLeave;
		}

	}

}