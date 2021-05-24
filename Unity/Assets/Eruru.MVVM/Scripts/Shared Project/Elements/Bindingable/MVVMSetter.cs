using System;
using System.Reflection;
using UnityEngine;

namespace Eruru.MVVM {

	public class MVVMSetter : MVVMBindingable {

		MVVMAction Action;

		public MVVMSetter (string path, object value) : base (path, value) {

		}
		public MVVMSetter (MVVMBinding binding, object value) : base (binding, value) {

		}
		public MVVMSetter (MVVMAction action) {
			Action = action;
		}

		internal void Set () {
			if (Action != null) {
				Action ();
				return;
			}
			Binding.SetValue (Value);
		}

	}

}