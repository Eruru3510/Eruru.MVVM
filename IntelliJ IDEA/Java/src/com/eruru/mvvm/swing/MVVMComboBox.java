package com.eruru.mvvm.swing;

import javax.swing.*;

public class MVVMComboBox extends MVVMItemsControl {

	private final JComboBox control;

	public MVVMComboBox (JComboBox control) {
		super (control);
		this.control = control;
	}

	public JComboBox getControl () {
		return control;
	}

	@Override
	protected Object convert (Object value) {
		return value;
	}

	@Override
	protected void add (Object value, int index) {
		control.insertItemAt (value, index);
	}

	@Override
	protected void add (Object value) {
		control.addItem (value);
	}

	@Override
	protected void removeAt (int index) {
		control.removeItemAt (index);
	}

	@Override
	protected void replace (int index, Object value) {
		control.removeItemAt (index);
		control.insertItemAt (value, index);
	}

	@Override
	public void move (int oldIndex, int newIndex) {
		Object value = control.getItemAt (oldIndex);
		control.removeItemAt (oldIndex);
		control.insertItemAt (value, newIndex);
	}

	@Override
	protected void reset () {
		control.removeAllItems ();
	}

}