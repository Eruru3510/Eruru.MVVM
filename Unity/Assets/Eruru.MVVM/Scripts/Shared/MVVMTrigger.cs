using System.Collections.Generic;

namespace Eruru.MVVM {

	public class MVVMTrigger {

		public MVVMControl Control {

			get {
				return Binding.Control;
			}

			set {
				Binding.Control = value;
				foreach (MVVMSetter setter in Setters) {
					setter.Control = value;
				}
			}

		}

		readonly MVVMBinding Binding;
		readonly object Value;
		readonly List<MVVMSetter> Setters = new List<MVVMSetter> ();

		public MVVMTrigger (MVVMBinding binding, object value) {
			Binding = binding;
			Value = value;
			Binding.IsTrigger = true;
			Binding.OnSetTargetValue = targetValue => {
				if (Equals (targetValue, Value)) {
					foreach (MVVMSetter setter in Setters) {
						setter.Apply ();
					}
				} else {
					foreach (MVVMSetter setter in Setters) {
						setter.Restore ();
					}
				}
			};
		}
		public MVVMTrigger (string property, object value) : this (new MVVMBinding (new MVVMRelativeSource (), property), value) {

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
			Binding.Bind ();
		}

		public void Unbind () {
			foreach (MVVMSetter setter in Setters) {
				setter.Unbind ();
			}
			Binding.Unbind ();
		}

	}

}