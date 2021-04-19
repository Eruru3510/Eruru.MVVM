using System;
using System.Collections;
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
			FieldInfo fieldInfo = type.GetField ("PropertyChanged", MVVMApi.BindingFlags);
			if (fieldInfo == null) {
				fieldInfo = type.BaseType.GetField ("PropertyChanged", MVVMApi.BindingFlags);
			}
			PropertyChangedEventHandler propertyChangedEventHandler = fieldInfo.GetValue (notifyPropertyChanged) as PropertyChangedEventHandler;
			if (propertyChangedEventHandler != null) {
				propertyChangedEventHandler (notifyPropertyChanged, new PropertyChangedEventArgs (propertyName));
			}
		}

		public static void Swap (this IList list, int left, int right) {
			if (list == null) {
				throw new ArgumentNullException ("list");
			}
			object temp = list[left];
			list[left] = list[right];
			list[right] = temp;
		}

	}

}