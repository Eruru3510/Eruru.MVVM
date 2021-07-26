package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMAPI;
import com.eruru.mvvm.MVVMBinding;

import javax.swing.*;

public class MVVMLabel extends MVVMControl {

	private final JLabel control;

	private MVVMBinding text;

	public MVVMLabel (JLabel control) {
		super (control);
		this.control = control;
	}

	public JLabel getControl () {
		return control;
	}

	public MVVMBinding getText () {
		return getBinding (text, this::setText);
	}

	public void setText (MVVMBinding binding) {
		text = setBinding (binding, control::getText, targetValue -> control.setText (MVVMAPI.to (targetValue, String.class)));
	}

}