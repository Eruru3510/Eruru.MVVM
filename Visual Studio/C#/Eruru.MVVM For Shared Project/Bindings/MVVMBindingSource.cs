using System;
using System.ComponentModel;
using System.Reflection;

namespace Eruru.MVVM {

	public class MVVMBindingSource : MVVMBinding {

		readonly string Path;

		PropertyInfo PropertyInfo;

		public MVVMBindingSource () {

		}
		public MVVMBindingSource (string path) {
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Path = path;
		}
		public MVVMBindingSource (string path, MVVMBindingMode mode) : base (mode) {
			if (path == null) {
				throw new ArgumentNullException ("path");
			}
			Path = path;
		}

		public override object GetValue () {
			return PropertyInfo == null ? Value : PropertyInfo.GetValue (Value, null);
		}

		public override void SetValue (object value) {
			if (PropertyInfo == null) {
				Value = value;
				return;
			}
			PropertyInfo.SetValue (Value, Convert.ChangeType (value, PropertyInfo.PropertyType), null);
		}

		protected override void _Rebinding (object dataContext) {
			UnregisterPropertyChanged ();
			if (dataContext == null || string.IsNullOrEmpty (Path)) {
				PropertyInfo = null;
				Value = dataContext;
				return;
			}
			string[] nodes = Path.Split ('.');
			object currentInstance = dataContext;
			Type type = dataContext.GetType ();
			for (int i = 0; i < nodes.Length; i++) {
				PropertyInfo propertyInfo = type.GetProperty (nodes[i]);
				if (propertyInfo == null) {
					throw new Exception (string.Format ("{0}没有{1}属性访问器", Value, nodes[i]));
				}
				if (currentInstance is INotifyPropertyChanged) {
					INotifyPropertyChanged notifyPropertyChanged = (INotifyPropertyChanged)currentInstance;
					notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
					NotifyPropertyChangeds.Add (notifyPropertyChanged);
				}
				if (i < nodes.Length - 1) {
					currentInstance = propertyInfo.GetValue (currentInstance, null);
					if (currentInstance == null) {
						return;
					}
					type = currentInstance.GetType ();
					continue;
				}
				Value = currentInstance;
				PropertyInfo = propertyInfo;
				PropertyName = nodes[i];
			}
		}

	}

}