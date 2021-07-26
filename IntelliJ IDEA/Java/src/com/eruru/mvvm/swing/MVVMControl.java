package com.eruru.mvvm.swing;

import com.eruru.mvvm.MVVMAPI;
import com.eruru.mvvm.MVVMBinding;
import com.eruru.mvvm.MVVMControlBase;
import com.eruru.mvvm.MVVMPropertyChanged;

import java.awt.*;

public class MVVMControl extends MVVMControlBase {

	private final Component control;
	private final MVVMPropertyChanged propertyChanged = new MVVMPropertyChanged (this);

	private MVVMBinding size;
	private MVVMBinding width;
	private MVVMBinding height;

	public MVVMControl (Component control) {
		this.control = control;
	}

	@Override
	public MVVMPropertyChanged getPropertyChanged () {
		return propertyChanged;
	}

	public Component getControl () {
		return control;
	}

	public MVVMBinding getSize () {
		return getBinding (size, this::setSize);
	}

	public void setSize (MVVMBinding binding) {
		size = setBinding (binding, control::getSize, targetValue -> control.setSize (MVVMAPI.to (targetValue, Dimension.class)));
	}

	public MVVMBinding getWidth () {
		return getBinding (width, this::setWidth);
	}

	public void setWidth (MVVMBinding binding) {
		width = setBinding (binding, control::getWidth, targetValue -> control.setSize (MVVMAPI.to (targetValue, int.class), control.getHeight ()));
	}

	public MVVMBinding getHeight () {
		return getBinding (height, this::setHeight);
	}

	public void setHeight (MVVMBinding binding) {
		height = setBinding (binding, control::getHeight, targetValue -> control.setSize (control.getWidth (), MVVMAPI.to (targetValue, int.class)));
	}

	@Override
	public MVVMControl getParent () {
		return (MVVMControl) super.getParent ();
	}

	@Override
	public MVVMControl getRoot () {
		return (MVVMControl) super.getRoot ();
	}

	@Override
	public MVVMControl find (String elementName) {
		return (MVVMControl) super.find (elementName);
	}

	@Override
	public MVVMControl add (MVVMControlBase control) {
		return (MVVMControl) super.add (control);
	}

	@Override
	public MVVMControl add (MVVMControlBase... controls) {
		return (MVVMControl) super.add (controls);
	}

	@Override
	public MVVMControl add (MVVMControlBase control, int index) {
		return (MVVMControl) super.add (control, index);
	}

	@Override
	public MVVMControl get (int index) {
		return (MVVMControl) super.get (index);
	}

	@Override
	public MVVMControl build () {
		return (MVVMControl) super.build ();
	}

}