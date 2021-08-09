using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMScrollRect : MVVMItemsControl {

		public new ScrollRect Control { get; private set; }

		public MVVMScrollRect (ScrollRect control) : base (control) {
			Control = control;
			ItemParent = Control.content;
		}

	}

}