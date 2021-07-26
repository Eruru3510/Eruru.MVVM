package com.eruru.mvvm;

import java.io.Serializable;

public class MVVMBindingRelativeSource implements Serializable {

	MVVMBindingRelativeSourceMode mode;
	Class<?> ancestorType;
	int ancestorLevel;

	public MVVMBindingRelativeSource () {
		mode = MVVMBindingRelativeSourceMode.Self;
	}

	public MVVMBindingRelativeSource (Class<?> ancestorType) {
		this (ancestorType, 1);
	}

	public MVVMBindingRelativeSource (Class<?> ancestorType, int ancestorLevel) {
		this.ancestorType = ancestorType;
		this.ancestorLevel = ancestorLevel;
		mode = MVVMBindingRelativeSourceMode.FindAncestor;
	}

}