package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMAPI;
import com.eruru.mvvm.MVVMBinding;

import javax.swing.*;

public class MVVMFrame extends MVVMControl {

	private final JFrame control;

	private MVVMBinding title;

	public MVVMFrame (JFrame control) {
		super (control);
		this.control = control;
	}

	public JFrame getControl () {
		return control;
	}

	public MVVMBinding getTitle () {
		return getBinding (title, this::setTitle);
	}

	public void setTitle (MVVMBinding binding) {
		title = setBinding (binding, control::getTitle, targetValue -> control.setTitle (MVVMAPI.to (targetValue, String.class)));
	}

}