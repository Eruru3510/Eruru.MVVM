using System.Collections.Generic;
using UnityEngine;

namespace Eruru.MVVM {

	public class MVVMStyle : MVVMElement {

		List<MVVMSetter> Setters = new List<MVVMSetter> ();

		public MVVMStyle () {
			OnParentChanged += MVVMStyle_OnParentChanged;
			OnDataContextChanged += MVVMStyle_OnDataContextChanged;
		}

		private void MVVMStyle_OnParentChanged () {
			for (int i = 0; i < Setters.Count; i++) {
				Setters[i].Parent = Parent;
				Setters[i].Set ();
			}
		}

		private void MVVMStyle_OnDataContextChanged (object dataContext) {
			for (int i = 0; i < Setters.Count; i++) {
				Setters[i].Set ();
			}
		}

		public MVVMStyle Add (MVVMSetter setter) {
			Elements.Add (setter);
			setter.Parent = Parent;
			Setters.Add (setter);
			setter.DataContext.Rebinding (DataContext.GetTargetValue ());
			setter.Set ();
			return this;
		}
		public MVVMStyle Add (params MVVMSetter[] setters) {
			for (int i = 0; i < setters.Length; i++) {
				Add (setters[i]);
			}
			return this;
		}

	}

}