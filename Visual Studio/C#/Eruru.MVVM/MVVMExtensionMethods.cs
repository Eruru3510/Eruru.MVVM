using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Eruru.MVVM {

	public static class MVVMExtensionMethods {

		static readonly Dictionary<INotifyPropertyChanged, FieldInfo> NotifyPropertyChangeds = new Dictionary<INotifyPropertyChanged, FieldInfo> ();
		static readonly Dictionary<string, PropertyChangedEventArgs> propertyChangedEventArguments = new Dictionary<string, PropertyChangedEventArgs> ();

		public static void RaisePropertyChanged (this INotifyPropertyChanged notifyPropertyChanged, [CallerMemberName] string propertyName = null) {
			if (propertyName == null) {
				propertyName = MVVMAPI.GetCallerMemberName ();
			}
			FieldInfo fieldInfo;
			if (!NotifyPropertyChangeds.TryGetValue (notifyPropertyChanged, out fieldInfo)) {
				Type type = notifyPropertyChanged.GetType ();
				while (type != null) {
					fieldInfo = type.GetField ("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
					if (fieldInfo != null) {
						break;
					}
					type = type.BaseType;
				}
				NotifyPropertyChangeds.Add (notifyPropertyChanged, fieldInfo);
			}
			PropertyChangedEventHandler propertyChangedEventHandler = fieldInfo.GetValue (notifyPropertyChanged) as PropertyChangedEventHandler;
			if (propertyChangedEventHandler != null) {
				PropertyChangedEventArgs propertyChangedEventArgs;
				if (!propertyChangedEventArguments.TryGetValue (propertyName, out propertyChangedEventArgs)) {
					propertyChangedEventArgs = new PropertyChangedEventArgs (propertyName);
					propertyChangedEventArguments.Add (propertyName, propertyChangedEventArgs);
				}
				propertyChangedEventHandler (notifyPropertyChanged, propertyChangedEventArgs);
			}
		}

	}

}