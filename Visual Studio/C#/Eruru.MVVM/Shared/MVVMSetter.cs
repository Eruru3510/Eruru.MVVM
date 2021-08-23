using System;

namespace Eruru.MVVM {

	public class MVVMSetter {

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
			}

		}

		readonly MVVMBinding Property;
		readonly MVVMBinding BindingValue;
		readonly object Value;

		bool IsApplied;
		object RawValue;

		public MVVMSetter (MVVMBinding property, object value) {
			Type = MVVMTriggerType.Value;
			Property = property;
			Value = value;
			Property.IsTrigger = true;
		}
		public MVVMSetter (MVVMBinding property, MVVMBinding value) {
			Type = MVVMTriggerType.Binding;
			Property = property;
			BindingValue = value;
			Property.IsTrigger = true;
			BindingValue.IsTrigger = true;
		}
		public MVVMSetter (string property, object value) : this (new MVVMBinding (new MVVMRelativeSource (), property), value) {

		}
		public MVVMSetter (string property, MVVMBinding value) : this (new MVVMBinding (new MVVMRelativeSource (), property), value) {

		}

		public void Apply () {
			if (!IsApplied) {
				IsApplied = true;
				RawValue = Property.GetSourceValue ();
			}
			switch (Type) {
				case MVVMTriggerType.Binding:
					Property.SetSourceValue (BindingValue.GetTargetValue ());
					break;
				case MVVMTriggerType.Value:
					Property.SetSourceValue (Value);
					break;
				default:
					throw new NotImplementedException (Type.ToString ());
			}
		}

		public void Restore () {
			if (IsApplied) {
				IsApplied = false;
			}
			Property.SetSourceValue (RawValue);
		}

		public void Bind () {
			Property.Bind ();
			RawValue = Property.GetSourceValue ();
			if (BindingValue != null) {
				BindingValue.Bind ();
			}
		}

		public void Unbind () {
			Property.Unbind ();
			if (BindingValue != null) {
				BindingValue.Unbind ();
			}
		}

	}

}