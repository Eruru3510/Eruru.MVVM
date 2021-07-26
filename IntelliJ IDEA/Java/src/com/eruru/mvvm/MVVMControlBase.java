package com.eruru.mvvm;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public abstract class MVVMControlBase implements MVVMNotifyPropertyChanged {

	private final Map<String, MVVMControlBase> controlNames = new HashMap<> ();
	private final ArrayList<MVVMControlBase> controls = new ArrayList<> ();
	private final ArrayList<MVVMBinding> bindings = new ArrayList<> ();

	private MVVMBinding dataContext;
	private MVVMBinding name;
	private MVVMControlBase parent;
	private MVVMControlBase root;

	public MVVMBinding getDataContext () {
		return getBinding (dataContext, this::setDataContext);
	}

	public void setDataContext (MVVMBinding binding) {
		dataContext = setBinding (binding, true, targetValue -> {
			for (MVVMBinding item : bindings) {
				item.bind ();
			}
			for (MVVMControlBase control : controls) {
				control.getDataContext ().bind ();
			}
		});
	}

	public MVVMBinding getName () {
		return getBinding (name, this::setName);
	}

	public void setName (MVVMBinding binding) {
		name = setBinding (binding);
	}

	public void setName (String name) {
		setName (new MVVMBinding ((Object) name));
	}

	public MVVMControlBase getRoot () {
		return root;
	}

	public MVVMControlBase getParent () {
		return parent;
	}

	public ArrayList<MVVMControlBase> getControls () {
		return controls;
	}

	public MVVMControlBase find (String elementName) {
		return controlNames.get (elementName);
	}

	public MVVMControlBase add (MVVMControlBase control) {
		return add (control, controls.size ());
	}

	public MVVMControlBase add (MVVMControlBase... controls) {
		for (MVVMControlBase control : controls) {
			add (control);
		}
		return this;
	}

	public MVVMControlBase add (MVVMControlBase control, int index) {
		control.parent = this;
		control.root = root;
		controls.add (index, control);
		return this;
	}

	public MVVMControlBase get (int index) {
		return controls.get (index);
	}

	public void set (int index, MVVMControlBase control) {
		controls.get (index).debind ();
		controls.set (index, control);
	}

	public void swap (int oldIndex, int newIndex) {
		MVVMControlBase control = controls.get (oldIndex);
		controls.set (oldIndex, controls.get (newIndex));
		controls.set (newIndex, control);
	}

	public void move (int oldIndex, int newIndex) {
		MVVMControlBase control = controls.get (oldIndex);
		controls.remove (oldIndex);
		controls.add (newIndex, control);
	}

	public void remove (int index) {
		controls.get (index).debind ();
		controls.remove (index);
	}

	public void removeRange (int start, int length) {
		for (int i = start + length - 1; i >= start; i--) {
			remove (i);
		}
	}

	public void clear () {
		for (MVVMControlBase control : controls) {
			control.debind ();
		}
		controls.clear ();
	}

	public MVVMControlBase build (MVVMControlBase root) {
		if (root == null) {
			root = this.root == null ? this : this.root;
		}
		controlNames.clear ();
		debind ();
		MVVMControlBase finalRoot = root;
		forEach (control -> control.register (finalRoot));
		getDataContext ().bind ();
		return this;
	}

	public MVVMControlBase build () {
		return build (null);
	}

	public void debind () {
		forEach (control -> {
			control.getDataContext ().debind ();
			for (MVVMBinding binding : control.bindings) {
				binding.debind ();
			}
		});
	}

	private void forEach (MVVMAction1<MVVMControlBase> action) {
		action.invoke (this);
		for (MVVMControlBase control : controls) {
			control.forEach (action);
		}
	}

	private void register (MVVMControlBase root) {
		this.root = root;
		if (name != null) {
			root.controlNames.put (name.getTargetValue (String.class), this);
		}
	}

	protected MVVMBinding getBinding (MVVMBinding binding, MVVMAction1<MVVMBinding> setBinding) {
		if (binding == null) {
			binding = new MVVMBinding ();
			setBinding.invoke (binding);
		}
		return binding;
	}

	protected MVVMBinding setBinding (MVVMBinding binding,
	                                  MVVMFuncI<Object> getTargetValue, MVVMAction1<Object> setTargetValue,
	                                  MVVMAction debind,
	                                  boolean hasOnChanged, boolean hasLostFocus,
	                                  boolean isDataContext, String propertyName
	) {
		if (!isDataContext && binding != null) {
			bindings.remove (binding);
		}
		if (binding == null) {
			return null;
		}
		if (propertyName == null) {
			propertyName = MVVMAPI.firstCharToLowerCase (MVVMAPI.getCallerMemberName ());
		}
		binding.control = this;
		binding.targetPropertyName = propertyName;
		binding.isDataContext = isDataContext;
		binding.onDebind = debind;
		binding.onGetTargetValue = getTargetValue;
		binding.onSetTargetValue = setTargetValue;
		binding.defaultMode = hasOnChanged ? MVVMBindingMode.TwoWay : MVVMBindingMode.OneWay;
		binding.defaultUpdateSourceTrigger = hasLostFocus ? MVVMBindingUpdateSourceTrigger.LostFocus : MVVMBindingUpdateSourceTrigger.PropertyChanged;
		if (!isDataContext) {
			bindings.add (binding);
		}
		return binding;
	}

	protected MVVMBinding setBinding (MVVMBinding binding, MVVMFuncI<Object> getTargetValue, MVVMAction1<Object> setTargetValue, boolean hasOnChanged, boolean hasLostFocus) {
		return setBinding (binding, getTargetValue, setTargetValue, null, hasOnChanged, hasLostFocus, false, MVVMAPI.getCallerMemberName ());
	}

	protected MVVMBinding setBinding (MVVMBinding binding, MVVMFuncI<Object> getTargetValue, MVVMAction1<Object> setTargetValue, boolean hasOnChanged) {
		return setBinding (binding, getTargetValue, setTargetValue, null, hasOnChanged, false, false, MVVMAPI.getCallerMemberName ());
	}

	protected MVVMBinding setBinding (MVVMBinding binding, MVVMFuncI<Object> getTargetValue, MVVMAction1<Object> setTargetValue) {
		return setBinding (binding, getTargetValue, setTargetValue, null, true, true, false, MVVMAPI.getCallerMemberName ());
	}

	protected MVVMBinding setBinding (MVVMBinding binding, boolean isDataContext, MVVMAction1<Object> setTargetValue) {
		return setBinding (binding, null, setTargetValue, null, false, false, isDataContext, MVVMAPI.getCallerMemberName ());
	}

	protected MVVMBinding setBinding (MVVMBinding binding, MVVMAction1<Object> setTargetValue, MVVMAction debind) {
		return setBinding (binding, null, setTargetValue, null, false, false, false, MVVMAPI.getCallerMemberName ());
	}

	protected MVVMBinding setBinding (MVVMBinding binding) {
		return setBinding (binding, null, null, null, false, false, false, MVVMAPI.getCallerMemberName ());
	}

	protected void onChanged (MVVMBinding binding, MVVMBindingOnChangedType onChangedType) {
		if (binding == null || binding.blockOnChanged) {
			return;
		}
		switch (binding.getMode ()) {
			case TwoWay:
			case OneWayToSource:
				switch (binding.getUpdateSourceTrigger ()) {
					case PropertyChanged:
						if (onChangedType == MVVMBindingOnChangedType.PropertyChanged) {
							binding.oneWaySetSourceValue ();
						}
						break;
					case LostFocus:
						if (onChangedType == MVVMBindingOnChangedType.LostFocus) {
							binding.oneWaySetSourceValue ();
						}
						break;
				}
				break;
		}
		getPropertyChanged ().invoke (this, binding.targetPropertyName);
	}

	protected void onChanged (MVVMBinding binding) {
		onChanged (binding, MVVMBindingOnChangedType.PropertyChanged);
	}

	protected void onCommand (MVVMBinding command, MVVMBinding parameter) {
		if (command == null) {
			return;
		}
		MVVMRelayCommand relayCommand = command.getTargetValue (MVVMRelayCommand.class);
		if (relayCommand == null) {
			return;
		}
		Object value = parameter == null ? null : parameter.getTargetValue ();
		if (relayCommand.canExecute (value)) {
			relayCommand.execute (value);
		}
	}

}