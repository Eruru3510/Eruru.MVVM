package com.eruru.mvvm;

public class MVVMCollectionChanged extends MVVMEvent<MVVMNotifyCollectionChanged, MVVMAction6<Object, MVVMNotifyCollectionChangedAction, Integer, Object[], Integer, Object[]>> {

	public MVVMCollectionChanged (MVVMNotifyCollectionChanged notifyCollectionChanged) {
		super (notifyCollectionChanged);
	}

	public void invoke (Object sender, MVVMNotifyCollectionChangedAction action, int oldIndex, Object[] oldItems, int newIndex, Object[] newItems) {
		forEach (collectionChanged -> collectionChanged.invoke (sender, action, oldIndex, oldItems, newIndex, newItems));
	}

	public void invoke (MVVMNotifyCollectionChangedAction action, int oldIndex, Object[] oldItems, int newIndex, Object[] newItems) {
		invoke (getInstance (), action, oldIndex, oldItems, newIndex, newItems);
	}

	public void invoke (Object sender, MVVMNotifyCollectionChangedAction action, int index, Object[] items) {
		invoke (sender, action, index, items, index, items);
	}

	public void invoke (MVVMNotifyCollectionChangedAction action, int index, Object[] items) {
		invoke (getInstance (), action, index, items, index, items);
	}

	public void invoke (Object sender, MVVMNotifyCollectionChangedAction action, int index, Object[] oldItems, Object[] newItems) {
		invoke (sender, action, index, oldItems, index, newItems);
	}

	public void invoke (MVVMNotifyCollectionChangedAction action, int index, Object[] oldItems, Object[] newItems) {
		invoke (getInstance (), action, index, oldItems, index, newItems);
	}

	public void invoke (Object sender, MVVMNotifyCollectionChangedAction action, Object[] oldItems, int oldIndex, int newIndex) {
		invoke (sender, action, oldIndex, oldItems, newIndex, oldItems);
	}

	public void invoke (MVVMNotifyCollectionChangedAction action, Object[] oldItems, int oldIndex, int newIndex) {
		invoke (getInstance (), action, oldIndex, oldItems, newIndex, oldItems);
	}

	public void invoke (Object sender, MVVMNotifyCollectionChangedAction action) {
		invoke (sender, action, -1, null, -1, null);
	}

	public void invoke (MVVMNotifyCollectionChangedAction action) {
		invoke (getInstance (), action, -1, null, -1, null);
	}

}