package com.eruru.mvvm;

public class MVVMTableColumn {

	private String name;
	private MVVMBinding binding;

	public MVVMTableColumn (String name, MVVMBinding binding) {
		this.name = name;
		this.binding = binding;
	}

	public MVVMTableColumn (String name) {
		this.name = name;
	}

	public String getName () {
		return name;
	}

	public void setName (String name) {
		this.name = name;
	}

	public MVVMBinding getBinding () {
		return binding;
	}

	public void setBinding (MVVMBinding binding) {
		this.binding = binding;
	}

}