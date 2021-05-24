using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Eruru.MVVM {

	public partial class MVVMControl : MVVMElement {

		internal event MVVMAction OnMouseEnter;
		internal event MVVMAction OnMouseLeave;

		public MVVMControl (MonoBehaviour control) {
			Control = control;
		}

		public MVVMControl Add (MVVMElement element) {
			element.Parent = this;
			Elements.Add (element);
			element.DataContext.Rebinding (DataContext.GetTargetValue ());
			return this;
		}
		public MVVMControl Add (params MVVMElement[] elements) {
			for (int i = 0; i < elements.Length; i++) {
				Add (elements[i]);
			}
			return this;
		}

	}

}