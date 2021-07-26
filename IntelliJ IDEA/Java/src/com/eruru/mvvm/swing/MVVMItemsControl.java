package com.eruru.mvvm.swing;

import com.eruru.mvvm.*;

import javax.swing.*;
import java.awt.*;

public class MVVMItemsControl extends MVVMControl {

	private final Container control;

	private MVVMBinding itemsSource;
	private MVVMBinding itemTemplate;
	private MVVMNotifyCollectionChanged notifyCollectionChanged;

	public MVVMItemsControl (Container control) {
		super (control);
		this.control = control;
	}

	public Container getControl () {
		return control;
	}

	public MVVMBinding getItemsSource () {
		return getBinding (itemsSource, this::setItemsSource);
	}

	public void setItemsSource (MVVMBinding binding) {
		itemsSource = setBinding (binding, targetValue -> {
			cancelCollectionChanged ();
			reset ();
			if (targetValue instanceof Iterable) {
				for (Object item : (Iterable<?>) targetValue) {
					add (item);
				}
			}
			if (targetValue instanceof MVVMNotifyCollectionChanged) {
				notifyCollectionChanged = (MVVMNotifyCollectionChanged) targetValue;
				notifyCollectionChanged.getCollectionChanged ().add (this, this::collectionChanged);
			}
		}, this::cancelCollectionChanged);
	}

	public MVVMBinding getItemTemplate () {
		return getBinding (itemTemplate, this::setItemTemplate);
	}

	public void setItemTemplate (MVVMBinding binding) {
		itemTemplate = setBinding (binding);
	}

	protected Object convert (Object value) {
		MVVMDataTemplate dataTemplate = null;
		if (itemTemplate != null) {
			dataTemplate = itemTemplate.getTargetValue (MVVMDataTemplate.class);
		}
		MVVMControlBase control;
		if (dataTemplate == null) {
			control = new MVVMLabel (new JLabel ()) {{
				setText (new MVVMBinding ());
			}};
		} else {
			control = dataTemplate.build ();
		}
		return control;
	}

	private MVVMControl register (Object value, int index) {
		MVVMControl control = (MVVMControl) convert (value);
		super.add (control, index);
		control.setDataContext (new MVVMBinding (value));
		control.build (getRoot ());
		this.control.add (control.getControl (), index);
		this.control.revalidate ();
		this.control.repaint ();
		return control;
	}

	protected void add (Object value, int index) {
		register (value, index);
	}

	protected void add (Object value) {
		add (value, super.getControls ().size ());
	}

	protected void removeAt (int index) {
		super.remove (index);
		control.remove (index);
		control.revalidate ();
		control.repaint ();
	}

	protected void replace (int index, Object value) {
		removeAt (index);
		register (value, index);
	}

	public void move (int oldIndex, int newIndex) {
		super.move (oldIndex, newIndex);
		control.add (control.getComponent (oldIndex), newIndex);
		control.revalidate ();
		control.repaint ();
	}

	protected void reset () {
		super.clear ();
		control.removeAll ();
		control.revalidate ();
		control.repaint ();
	}

	void cancelCollectionChanged () {
		if (notifyCollectionChanged != null) {
			notifyCollectionChanged.getCollectionChanged ().remove (this);
			notifyCollectionChanged = null;
		}
	}

	void collectionChanged (Object sender, MVVMNotifyCollectionChangedAction action, int oldIndex, Object[] oldItems, int newIndex, Object[] newItems) {
		switch (action) {
			case Add:
				this.add (newItems[0], newIndex);
				break;
			case Remove:
				this.removeAt (oldIndex);
				break;
			case Replace:
				this.replace (oldIndex, newItems[0]);
				break;
			case Move:
				this.move (oldIndex, newIndex);
				break;
			case Reset:
				this.reset ();
				break;
			default:
				throw new UnsupportedOperationException (action.toString ());
		}
	}

}