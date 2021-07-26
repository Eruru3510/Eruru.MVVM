package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMAPI;
import com.eruru.mvvm.MVVMBinding;

import javax.swing.*;

public class MVVMButton extends MVVMControl {

	private final JButton control;

	private MVVMBinding text;
	private MVVMBinding onAction;
	private MVVMBinding onActionParameter;

	public MVVMButton (JButton control) {
		super (control);
		this.control = control;
		control.addActionListener (e -> onCommand (onAction, onActionParameter));
	}

	public JButton getControl () {
		return control;
	}

	public MVVMBinding getText () {
		return getBinding (text, this::setText);
	}

	public void setText (MVVMBinding binding) {
		text = setBinding (binding, control::getText, targetValue -> control.setText (MVVMAPI.to (targetValue, String.class)));
	}

	public MVVMBinding getOnAction () {
		return getBinding (onAction, this::setOnAction);
	}

	public void setOnAction (MVVMBinding binding) {
		onAction = setBinding (binding);
	}

	public MVVMBinding getOnActionParameter () {
		return getBinding (onActionParameter, this::setOnActionParameter);
	}

	public void setOnActionParameter (MVVMBinding binding) {
		onActionParameter = setBinding (binding);
	}

}