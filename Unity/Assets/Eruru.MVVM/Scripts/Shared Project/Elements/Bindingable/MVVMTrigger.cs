using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public partial class MVVMTrigger : MVVMBindingable {

		MVVMControl Control;
		List<MVVMSetter> Setters = new List<MVVMSetter> ();
		MVVMTriggerRoutedEvent RoutedEvent;
		MVVMFunc<object, bool> Comparison;

		public MVVMTrigger (string path, object value) : base (path, value) {

		}
		public MVVMTrigger (string path, MVVMFunc<object, bool> comparison) : base (path, null) {
			Comparison = comparison;
		}
		public MVVMTrigger (MVVMBinding binding, object value) : base (binding, value) {

		}
		public MVVMTrigger (MVVMBinding binding, MVVMFunc<object, bool> comparison) : base (binding, null) {
			Comparison = comparison;
		}
		public MVVMTrigger (MVVMTriggerRoutedEvent routedEvent) {
			RoutedEvent = routedEvent;
		}

		protected internal override void OnInitialize () {
			OnParentChanged += MVVMTrigger_OnParentChanged;
		}

		private void MVVMTrigger_OnParentChanged () {
			Control = (MVVMControl)Parent;
			for (int i = 0; i < Setters.Count; i++) {
				Setters[i].Parent = Parent;
			}
			if (Path == null && Binding == null) {
				BindEvent ();
			}
		}

		protected internal override void OnSetTargetValue (object value) {
			if (Comparison == null) {
				if (value == null || Value == null) {
					if (value == Value) {
						ExecuteSetters ();
					}
					return;
				}
				IComparable a = value as IComparable;
				IComparable b = MVVMApi.ChangeType (value, value.GetType ()) as IComparable;
				if (a == null || b == null) {
					return;
				}
				ExecuteSetters ();
				return;
			}
			if (Comparison (value)) {
				ExecuteSetters ();
			}
		}

		public MVVMTrigger Add (MVVMSetter setter) {
			Elements.Add (setter);
			setter.Parent = Parent;
			Setters.Add (setter);
			setter.DataContext.Rebinding (DataContext.GetTargetValue ());
			return this;
		}
		public MVVMTrigger Add (params MVVMSetter[] setters) {
			for (int i = 0; i < setters.Length; i++) {
				Add (setters[i]);
			}
			return this;
		}

		void OnMouseEnter () {
			if (RoutedEvent == MVVMTriggerRoutedEvent.MouseEnter) {
				ExecuteSetters ();
			}
		}

		void OnMouseLeave () {
			if (RoutedEvent == MVVMTriggerRoutedEvent.MouseLeave) {
				ExecuteSetters ();
			}
		}

		void ExecuteSetters () {
			for (int i = 0; i < Setters.Count; i++) {
				Setters[i].Set ();
			}
		}

	}

}