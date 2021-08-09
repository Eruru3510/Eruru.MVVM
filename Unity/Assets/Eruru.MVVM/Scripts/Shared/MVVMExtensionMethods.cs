using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public static class MVVMExtensionMethods {

		static readonly Dictionary<INotifyPropertyChanged, FieldInfo> PropertyChangeds = new Dictionary<INotifyPropertyChanged, FieldInfo> ();
		static readonly Dictionary<string, PropertyChangedEventArgs> PropertyNames = new Dictionary<string, PropertyChangedEventArgs> ();

		public static void RaisePropertyChanged (this INotifyPropertyChanged notifyPropertyChanged, [CallerMemberName] string propertyName = null) {
			if (propertyName == null) {
				propertyName = MVVMAPI.GetCallerMemberName ();
			}
			FieldInfo fieldInfo;
			if (!PropertyChangeds.TryGetValue (notifyPropertyChanged, out fieldInfo)) {
				Type type = notifyPropertyChanged.GetType ();
				while (type != null) {
					fieldInfo = type.GetField ("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
					type = type.BaseType;
					if (fieldInfo != null) {
						break;
					}
				}
				PropertyChangeds.Add (notifyPropertyChanged, fieldInfo);
			}
			PropertyChangedEventHandler propertyChangedEventHandler = fieldInfo.GetValue (notifyPropertyChanged) as PropertyChangedEventHandler;
			if (propertyChangedEventHandler != null) {
				PropertyChangedEventArgs propertyChangedEventArgs;
				if (!PropertyNames.TryGetValue (propertyName, out propertyChangedEventArgs)) {
					propertyChangedEventArgs = new PropertyChangedEventArgs (propertyName);
					PropertyNames.Add (propertyName, propertyChangedEventArgs);
				}
				propertyChangedEventHandler (notifyPropertyChanged, new PropertyChangedEventArgs (propertyName));
			}
		}

	}

}