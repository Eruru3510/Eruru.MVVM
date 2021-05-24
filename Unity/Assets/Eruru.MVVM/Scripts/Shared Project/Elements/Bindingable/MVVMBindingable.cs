using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace Eruru.MVVM {

	public abstract class MVVMBindingable : MVVMElement {

		internal protected MVVMBinding Binding;
		internal protected string Path;
		internal protected object Value;

		public MVVMBindingable () {
			Initialize ();
		}
		public MVVMBindingable (string path, object value) {
			Path = path;
			Value = value;
			SetBinding (new MVVMBinding (Path));
			Initialize ();
		}
		public MVVMBindingable (MVVMBinding binding, object value) {
			Binding = binding;
			Value = value;
			SetBinding (binding);
			Initialize ();
		}

		void Initialize () {
			OnParentChanged += MVVMBindingable_OnParentChanged;
			OnDataContextChanged += MVVMBindingable_OnDataContextChanged;
			OnInitialize ();
		}

		private void MVVMBindingable_OnParentChanged () {
			if (Path != null) {
				Binding.Element = (MVVMControl)Parent;
				Binding.OnRebinding ();
				return;
			}
		}

		private void MVVMBindingable_OnDataContextChanged (object dataContext) {
			if (Binding != null) {
				Binding.OnRebinding ();
			}
		}

		internal protected virtual void OnInitialize () {

		}

		internal protected virtual void OnSetTargetValue (object value) {

		}

		void SetBinding (MVVMBinding value) {
			SetBinding (
				ref Binding, value,
				null, OnSetTargetValue,
				null, null,
				null, null,
				false, string.Empty
			);
		}

	}

}