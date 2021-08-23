using System;
using System.Collections.Generic;

namespace Eruru.MVVM {

	public class MVVMTrigger {

		public MVVMTriggerType Type { get; private set; }
		public MVVMControl Control {

			get {
				return Property.Control;
			}

			set {
				Property.Control = value;
				if (BindingValue != null) {
					BindingValue.Control = value;
				}
				foreach (MVVMSetter setter in Setters) {
					setter.Control = value;
				}
			}

		}

		readonly MVVMBinding Property;
		readonly MVVMBinding BindingValue;
		readonly Predicate<object> Predicate;
		readonly object Value;
		readonly List<MVVMSetter> Setters = new List<MVVMSetter> ();

		public MVVMTrigger (MVVMBinding property, object value) {
			Type = MVVMTriggerType.Value;
			Property = property;
			Value = value;
			Property.IsTrigger = true;
			Property.OnSetTargetValue = OnSetTargetValue;
		}
		public MVVMTrigger (MVVMBinding property, MVVMBinding bindingValue) {
			Type = MVVMTriggerType.Binding;
			Property = property;
			BindingValue = bindingValue;
			Property.IsTrigger = true;
			BindingValue.IsTrigger = true;
			Property.OnSetTargetValue = OnSetTargetValue;
		}
		public MVVMTrigger (MVVMBinding property, Predicate<object> predicate) {
			Type = MVVMTriggerType.Predicate;
			Property = property;
			Predicate = predicate;
			Property.IsTrigger = true;
			Property.OnSetTargetValue = OnSetTargetValue;
		}
		public MVVMTrigger (string property, object value) : this (new MVVMBinding (new MVVMRelativeSource (), property), value) {

		}
		public MVVMTrigger (string property, MVVMBinding bindingValue) : this (new MVVMBinding (new MVVMRelativeSource (), property), bindingValue) {

		}
		public MVVMTrigger (string property, Predicate<object> predicate) : this (new MVVMBinding (new MVVMRelativeSource (), property), predicate) {

		}

		public MVVMTrigger Add (MVVMSetter setter) {
			setter.Control = Control;
			Setters.Add (setter);
			return this;
		}
		public MVVMTrigger Add (params MVVMSetter[] setters) {
			foreach (MVVMSetter setter in setters) {
				Add (setter);
			}
			return this;
		}

		public void Bind () {
			foreach (MVVMSetter setter in Setters) {
				setter.Bind ();
			}
			Property.Bind ();
		}

		public void Unbind () {
			foreach (MVVMSetter setter in Setters) {
				setter.Unbind ();
			}
			Property.Unbind ();
		}

		public void Apply () {
			foreach (MVVMSetter setter in Setters) {
				setter.Apply ();
			}
		}

		public void Restore () {
			foreach (MVVMSetter setter in Setters) {
				setter.Restore ();
			}
		}

		void OnSetTargetValue (object targetValue) {
			switch (Type) {
				case MVVMTriggerType.Value:
					if (Equals (targetValue, Value)) {
						Apply ();
					} else {
						Restore ();
					}
					break;
				case MVVMTriggerType.Binding:
					if (Equals (targetValue, BindingValue.GetTargetValue ())) {
						Apply ();
					} else {
						Restore ();
					}
					break;
				case MVVMTriggerType.Predicate:
					if (Predicate (targetValue)) {
						Apply ();
					} else {
						Restore ();
					}
					break;
				default:
					throw new NotImplementedException (Type.ToString ());
			}
		}

	}

}