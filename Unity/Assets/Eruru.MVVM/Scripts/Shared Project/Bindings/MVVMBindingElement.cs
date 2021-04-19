using System;
using System.ComponentModel;
using System.Reflection;

namespace Eruru.MVVM {

	public class MVVMBindingElement : MVVMBinding {

		PropertyInfo PropertyInfo;

		public MVVMBindingElement (MVVMControl control, string propertyName) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			if (propertyName == null) {
				throw new ArgumentNullException ("propertyName");
			}
			Value = control;
			PropertyName = propertyName;
			Initialize ();
		}
		public MVVMBindingElement (MVVMControl control, string propertyName, MVVMBindingMode mode) : base (mode) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			if (propertyName == null) {
				throw new ArgumentNullException ("propertyName");
			}
			Value = control;
			PropertyName = propertyName;
			Initialize ();
		}

		void Initialize () {
			UnregisterPropertyChanged ();
			PropertyInfo = Value.GetType ().GetProperty (PropertyName, MVVMApi.BindingFlags);
			if (PropertyInfo == null) {
				throw new Exception (string.Format ("{0}没有{1}属性访问器", Value, PropertyName));
			}
			if (Value is INotifyPropertyChanged) {
				INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)Value;
				notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
				NotifyPropertyChangeds.Add (notifyPropertyChanged);
			}
		}

		public override object GetValue () {
			object value = PropertyInfo.GetValue (Value, null);
			if (value is MVVMBinding) {
				return ((MVVMBinding)value).GetTargetValue ();
			}
			return value;
		}

		public override void SetValue (object value) {
			object instance = PropertyInfo.GetValue (Value, null);
			if (instance is MVVMBinding) {
				((MVVMBinding)instance).SetTargetValue (value);
			}
		}

	}

}