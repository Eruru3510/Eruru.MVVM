package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMAPI;
import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.MVVMBindingOnChangedType;

import javax.swing.*;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import java.awt.event.FocusEvent;
import java.awt.event.FocusListener;

public class MVVMTextField extends MVVMControl {

	private final JTextField control;

	private MVVMBinding text;

	public MVVMTextField (JTextField control) {
		super (control);
		this.control = control;
		control.getDocument ().addDocumentListener (new DocumentListener () {
			@Override
			public void insertUpdate (DocumentEvent e) {
				onChanged (text);
			}

			@Override
			public void removeUpdate (DocumentEvent e) {
				onChanged (text);
			}

			@Override
			public void changedUpdate (DocumentEvent e) {

			}
		});
		control.addFocusListener (new FocusListener () {
			@Override
			public void focusGained (FocusEvent e) {

			}

			@Override
			public void focusLost (FocusEvent e) {
				onChanged (text, MVVMBindingOnChangedType.LostFocus);
			}
		});
	}

	public JTextField getControl () {
		return control;
	}

	public MVVMBinding getText () {
		return getBinding (text, this::setText);
	}

	public void setText (MVVMBinding binding) {
		text = setBinding (binding, control::getText, targetValue -> control.setText (MVVMAPI.to (targetValue, String.class)), true, true);
	}

}