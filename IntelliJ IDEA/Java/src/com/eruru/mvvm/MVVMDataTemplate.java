package com.eruru.mvvm;

public class MVVMDataTemplate {

	private MVVMFuncI<MVVMControlBase> build;

	public MVVMDataTemplate (MVVMFuncI<MVVMControlBase> build) {
		this.build = build;
	}

	public MVVMFuncI<MVVMControlBase> getBuild () {
		return build;
	}

	public void setBuild (MVVMFuncI<MVVMControlBase> build) {
		this.build = build;
	}

	public MVVMControlBase build () {
		return build.invoke ();
	}

}
