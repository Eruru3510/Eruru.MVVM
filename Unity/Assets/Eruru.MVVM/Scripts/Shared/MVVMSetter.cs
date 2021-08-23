using System;

namespace Eruru.MVVM {

	public class MVVMSetter {

		public MVVMSetterType Type { get; private set; }
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

		object RawValue;

		public MVVMSetter (MVVMBinding property, object value) {
			Type = MVVMSetterType.Value;
			Property = property;
			Value = value;
			Property.IsTrigger = true;
		}
		public MVVMSetter (MVVMBinding property, MVVMBinding value) {
			Type = MVVMSetterType.Binding;
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
			RawValue = Property.GetSourceValue ();
			switch (Type) {
				case MVVMSetterType.Binding:
					Property.SetSourceValue (BindingValue.GetTargetValue ());
					break;
				case MVVMSetterType.Value:
					Property.SetSourceValue (Value);
					break;
				default:
					throw new NotImplementedException (Type.ToString ());
			}
		}

		public void Restore () {
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