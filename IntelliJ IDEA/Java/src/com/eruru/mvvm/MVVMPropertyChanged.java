package com.eruru.mvvm;

public class MVVMPropertyChanged extends MVVMEvent<MVVMNotifyPropertyChanged, MVVMAction2<Object, String>> {

	public MVVMPropertyChanged (MVVMNotifyPropertyChanged notifyPropertyChanged) {
		super (notifyPropertyChanged);
	}

	public void invoke (Object sender, String propertyName) {
		forEach (propertyChanged -> propertyChanged.invoke (sender, propertyName));
	}

	public void invoke (String propertyName) {
		forEach (propertyChanged -> propertyChanged.invoke (getInstance (), propertyName));
	}

	public void invoke (Object sender) {
		invoke (sender, MVVMAPI.getCallerMemberName ());
	}

	public void invoke () {
		invoke (getInstance (), MVVMAPI.getCallerMemberName ());
	}

}