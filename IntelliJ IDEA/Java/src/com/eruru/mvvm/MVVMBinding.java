package com.eruru.mvvm;

import java.io.*;
import java.lang.reflect.Method;
import java.util.AbstractMap;
import java.util.ArrayList;
import java.util.Map;

public class MVVMBinding implements Serializable {

	private String stringFormat;

	MVVMControlBase control;
	String targetPropertyName;
	String sourcePropertyName;
	boolean isDataContext;
	MVVMFuncI<Object> onGetTargetValue;
	MVVMAction1<Object> onSetTargetValue;
	MVVMAction onDebind;
	MVVMBindingMode defaultMode;
	MVVMBindingUpdateSourceTrigger defaultUpdateSourceTrigger;
	MVVMBindingType type;
	MVVMBindingMode mode = MVVMBindingMode.Default;
	MVVMBindingUpdateSourceTrigger updateSourceTrigger = MVVMBindingUpdateSourceTrigger.Default;
	MVVMBindingRelativeSource relativeSource;
	String path;
	Object targetValue;
	Object sourceValue;
	String elementName;
	Method getMethod;
	Method setMethod;
	Object instance;
	MVVMControlBase element;
	boolean blockSetSourceValue;
	boolean blockOnPropertyChanged;
	boolean blockOnChanged;
	boolean isFirstBind = true;
	ArrayList<Map.Entry<MVVMNotifyPropertyChanged, String>> notifyPropertyChangeds = new ArrayList<> ();

	public MVVMBinding () {
		type = MVVMBindingType.Path;
	}

	public MVVMBinding (MVVMBindingMode mode) {
		this ();
		this.mode = mode;
	}

	public MVVMBinding (MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this ();
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (String path) {
		this ();
		this.path = path;
	}

	public MVVMBinding (String path, MVVMBindingMode mode) {
		this (path);
		this.mode = mode;
	}

	public MVVMBinding (String path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (path);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (String path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (path, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (Object value) {
		targetValue = value;
		type = MVVMBindingType.Value;
	}

	public MVVMBinding (Object value, MVVMBindingMode mode) {
		this (value);
		this.mode = mode;
	}

	public MVVMBinding (Object value, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (value);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (Object value, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (value, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource) {
		this.relativeSource = relativeSource;
		type = MVVMBindingType.Relative;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingMode mode) {
		this (relativeSource);
		this.mode = mode;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (relativeSource);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (relativeSource, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, String path) {
		this (relativeSource);
		this.path = path;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, String path, MVVMBindingMode mode) {
		this (relativeSource, path);
		this.mode = mode;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, String path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (relativeSource, path);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMBindingRelativeSource relativeSource, String path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (relativeSource, path, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (String elementName, String path) {
		this.elementName = elementName;
		this.path = path;
		this.type = MVVMBindingType.ElementName;
	}

	public MVVMBinding (String elementName, String path, MVVMBindingMode mode) {
		this (elementName, path);
		this.mode = mode;
	}

	public MVVMBinding (String elementName, String path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (elementName, path);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (String elementName, String path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (elementName, path, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMControlBase element) {
		this.element = element;
		this.type = MVVMBindingType.Element;
	}

	public MVVMBinding (MVVMControlBase element, MVVMBindingMode mode) {
		this (element);
		this.mode = mode;
	}

	public MVVMBinding (MVVMControlBase element, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (element);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMControlBase element, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (element, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMControlBase element, String path) {
		this (element);
		this.path = path;
	}

	public MVVMBinding (MVVMControlBase element, String path, MVVMBindingMode mode) {
		this (element, path);
		this.mode = mode;
	}

	public MVVMBinding (MVVMControlBase element, String path, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (element, path);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMBinding (MVVMControlBase element, String path, MVVMBindingMode mode, MVVMBindingUpdateSourceTrigger updateSourceTrigger) {
		this (element, path, mode);
		this.updateSourceTrigger = updateSourceTrigger;
	}

	public MVVMControlBase getControl () {
		return control;
	}

	public String getStringFormat () {
		return stringFormat;
	}

	public void setStringFormat (String stringFormat) {
		this.stringFormat = stringFormat;
	}

	public Object getTargetValue () {
		if (onGetTargetValue == null) {
			return targetValue;
		}
		return onGetTargetValue.invoke ();
	}

	public <T> T getTargetValue (Class<T> type) {
		return MVVMAPI.to (getTargetValue (), type);
	}

	public void setTargetValue (Object value) {
		if (stringFormat != null) {
			value = String.format (stringFormat, value);
		}
		targetValue = value;
		if (onSetTargetValue != null) {
			blockOnChanged = true;
			onSetTargetValue.invoke (value);
			blockOnChanged = false;
		}
		control.onChanged (this);
	}

	public Object getSourceValue () {
		switch (type) {
			case Value:
				return targetValue;
			case Path:
			case ElementName:
			case Relative:
				if (sourcePropertyName == null) {
					return sourceValue;
				}
				Object value = null;
				try {
					value = getMethod.invoke (instance);
				} catch (Exception exception) {
					exception.printStackTrace ();
				}
				if (getMethod.getReturnType () == MVVMBinding.class) {
					return ((MVVMBinding) value).getTargetValue ();
				}
				return value;
			default:
				throw new UnsupportedOperationException (type.toString ());
		}
	}

	public <T> T getSourceValue (Class<T> type) {
		return MVVMAPI.to (getSourceValue (), type);
	}

	public void setSourceValue (Object value) {
		if (blockSetSourceValue) {
			return;
		}
		sourceValue = value;
		if (sourcePropertyName != null) {
			if (getMethod.getReturnType () == MVVMBinding.class) {
				try {
					Object binding = getMethod.invoke (instance);
					if (binding != null) {
						((MVVMBinding) binding).setTargetValue (value);
					}
				} catch (Exception exception) {
					exception.printStackTrace ();
				}
				return;
			}
			try {
				setMethod.invoke (instance, MVVMAPI.to (value, getMethod.getReturnType ()));
			} catch (Exception exception) {
				exception.printStackTrace ();
			}
		}
	}

	public void updateSource () {
		switch (getMode ()) {
			case twoWay:
			case oneWayToSource:
				break;
			default:
				return;
		}
		setSourceValue (getTargetValue ());
	}

	public void updateTarget () {
		setTargetValue (getSourceValue ());
	}

	public MVVMBinding clone () {
		try {
			ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream ();
			ObjectOutputStream objectOutputStream = new ObjectOutputStream (byteArrayOutputStream);
			objectOutputStream.writeObject (this);
			ByteArrayInputStream byteArrayInputStream = new ByteArrayInputStream (byteArrayOutputStream.toByteArray ());
			ObjectInputStream objectInputStream = new ObjectInputStream (byteArrayInputStream);
			return (MVVMBinding) objectInputStream.readObject ();
		} catch (Exception exception) {
			exception.printStackTrace ();
		}
		return null;
	}

	void bind () {
		if (!isFirstBind) {
			switch (type) {
				case Value:
				case Element:
				case ElementName:
				case Relative:
					return;
			}
		}
		isFirstBind = false;
		debind ();
		switch (type) {
			case Value:
				break;
			case Path: {
				Object dataContext = null;
				if (isDataContext) {
					if (control.getParent () != null) {
						dataContext = control.getParent ().getDataContext ().getTargetValue ();
					}
				} else {
					dataContext = control.getDataContext ().getTargetValue ();
				}
				bindSource (dataContext);
				break;
			}
			case Element:
				bindSource (element);
				break;
			case ElementName: {
				bindSource (control.getRoot ().find (elementName));
				break;
			}
			case Relative: {
				MVVMControlBase control = this.control;
				switch (relativeSource.mode) {
					case Self:
						break;
					case FindAncestor:
						int ancestorCount = 0;
						while (control != null) {
							if (relativeSource.ancestorType == null || relativeSource.ancestorType.isAssignableFrom (control.getClass ())) {
								ancestorCount++;
								if (ancestorCount == relativeSource.ancestorLevel) {
									break;
								}
							}
							control = control.getParent ();
						}
						break;
					default:
						throw new UnsupportedOperationException (relativeSource.mode.toString ());
				}
				bindSource (control);
				break;
			}
			default:
				throw new UnsupportedOperationException (type.toString ());
		}
		oneWaySetTargetValue ();
	}

	MVVMBindingMode getMode () {
		return mode == MVVMBindingMode.Default ? defaultMode : mode;
	}

	MVVMBindingUpdateSourceTrigger getUpdateSourceTrigger () {
		return updateSourceTrigger == MVVMBindingUpdateSourceTrigger.Default ? defaultUpdateSourceTrigger : updateSourceTrigger;
	}

	void oneWaySetTargetValue () {
		blockSetSourceValue = true;
		setTargetValue (getSourceValue ());
		blockSetSourceValue = false;
	}

	void oneWaySetSourceValue () {
		blockOnPropertyChanged = true;
		setSourceValue (getTargetValue ());
		blockOnPropertyChanged = false;
	}

	void debind () {
		sourcePropertyName = null;
		for (Map.Entry<MVVMNotifyPropertyChanged, String> notifyPropertyChanged : notifyPropertyChangeds) {
			notifyPropertyChanged.getKey ().getPropertyChanged ().remove (this);
		}
		notifyPropertyChangeds.clear ();
		if (onDebind != null) {
			onDebind.invoke ();
		}
		isFirstBind = true;
	}

	void bindSource (Object dataContext) {
		sourceValue = dataContext;
		if (sourceValue != null && path != null) {
			Object instance = sourceValue;
			Class<?> type = instance.getClass ();
			String[] pathNodes = path.split ("\\.");
			for (int i = 0; i < pathNodes.length; i++) {
				Method getMethod;
				Method setMethod = null;
				try {
					String name = MVVMAPI.firstCharToUpperCase (pathNodes[i]);
					getMethod = type.getMethod (String.format ("get%s", name));
					try {
						setMethod = type.getMethod (String.format ("set%s", name), getMethod.getReturnType ());
					} catch (Exception ignored) {

					}
				} catch (Exception exception) {
					new Exception (String.format ("绑定%s失败，%s下没有公开的属性%s", String.join (".", pathNodes), instance, pathNodes[i])).printStackTrace ();
					exception.printStackTrace ();
					return;
				}
				if (instance instanceof MVVMNotifyPropertyChanged) {
					MVVMNotifyPropertyChanged notifyPropertyChanged = (MVVMNotifyPropertyChanged) instance;
					notifyPropertyChanged.getPropertyChanged ().add (this, this::onPropertyChanged);
					notifyPropertyChangeds.add (new AbstractMap.SimpleEntry<> (notifyPropertyChanged, pathNodes[i]));
				}
				if (i == pathNodes.length - 1) {
					this.instance = instance;
					this.getMethod = getMethod;
					this.setMethod = setMethod;
					sourcePropertyName = pathNodes[i];
					break;
				}
				try {
					instance = getMethod.invoke (instance);
				} catch (Exception exception) {
					exception.printStackTrace ();
				}
				if (instance instanceof MVVMBinding) {
					instance = ((MVVMBinding) instance).getTargetValue ();
				}
				if (instance == null) {
					break;
				}
				type = instance.getClass ();
			}
		}
	}

	private void onPropertyChanged (Object sender, String propertyName) {
		if (blockOnPropertyChanged) {
			return;
		}
		switch (getMode ()) {
			case twoWay:
			case oneWay:
				propertyName = MVVMAPI.firstCharToLowerCase (propertyName);
				for (int i = 0; i < notifyPropertyChangeds.size (); i++) {
					if (notifyPropertyChangeds.get (i).getKey () == sender && propertyName.equals (notifyPropertyChangeds.get (i).getValue ())) {
						if (i == notifyPropertyChangeds.size () - 1) {
							oneWaySetTargetValue ();
							break;
						}
						bind ();
						break;
					}
				}
				break;
		}
	}

}