using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public static class MVVMExtensionMethods {

		public static void RaisePropertyChanged (this INotifyPropertyChanged notifyPropertyChanged, [CallerMemberName] string propertyName = null) {
			if (notifyPropertyChanged == null) {
				throw new ArgumentNullException ("notifyPropertyChanged");
			}
			if (propertyName == null) {
				propertyName = MVVMApi.GetCallerMemberName ();
			}
			Type type = notifyPropertyChanged.GetType ();
			FieldInfo fieldInfo = null;
			while (type != null) {
				fieldInfo = type.GetField ("PropertyChanged", MVVMApi.BindingFlags);
				type = type.BaseType;
				if (fieldInfo != null) {
					break;
				}
			}
			PropertyChangedEventHandler propertyChangedEventHandler = fieldInfo.GetValue (notifyPropertyChanged) as PropertyChangedEventHandler;
			if (propertyChangedEventHandler != null) {
				propertyChangedEventHandler (notifyPropertyChanged, new PropertyChangedEventArgs (propertyName));
			}
		}

	}

}