package com.eruru.mvvm;

import java.util.*;
import java.util.stream.Stream;

public class MVVMObservableCollection<T> implements MVVMNotifyCollectionChanged, MVVMNotifyPropertyChanged, Iterable<T>, List<T> {

	private final MVVMCollectionChanged collectionChanged = new MVVMCollectionChanged (this);
	private final ArrayList<T> items = new ArrayList<> ();
	private final Object[] oldItems = new Object[1];
	private final Object[] newItems = new Object[1];
	private final MVVMPropertyChanged propertyChanged = new MVVMPropertyChanged (this);
	private final String sizeName = "size";

	public MVVMCollectionChanged getCollectionChanged () {
		return collectionChanged;
	}

	public MVVMPropertyChanged getPropertyChanged () {
		return propertyChanged;
	}

	@Override
	public void add (int index, T element) {
		newItems[0] = element;
		items.add (index, element);
		propertyChanged.invoke (sizeName);
		collectionChanged.invoke (MVVMNotifyCollectionChangedAction.Add, index, newItems);
	}

	public boolean add (T value) {
		add (items.size (), value);
		return true;
	}

	public T remove (int index) {
		T value = items.get (index);
		oldItems[0] = value;
		items.remove (index);
		propertyChanged.invoke (sizeName);
		collectionChanged.invoke (MVVMNotifyCollectionChangedAction.Remove, index, oldItems);
		return value;
	}

	public boolean remove (Object value) {
		for (int i = 0; i < items.size (); i++) {
			if (items.get (i) == value) {
				remove (i);
				return true;
			}
		}
		return false;
	}

	public void removeRange (int start, int length) {
		for (int i = start + length - 1; i >= start; i--) {
			remove (i);
		}
	}

	public T set (int index, T value) {
		oldItems[0] = items.get (index);
		newItems[0] = value;
		items.set (index, value);
		collectionChanged.invoke (MVVMNotifyCollectionChangedAction.Replace, index, oldItems, newItems);
		return value;
	}

	public void move (int oldIndex, int newIndex) {
		T value = items.get (oldIndex);
		oldItems[0] = value;
		items.remove (oldIndex);
		items.add (newIndex, value);
		collectionChanged.invoke (MVVMNotifyCollectionChangedAction.Move, oldItems, oldIndex, newIndex);
	}

	public void clear () {
		items.clear ();
		propertyChanged.invoke (sizeName);
		collectionChanged.invoke (MVVMNotifyCollectionChangedAction.Reset);
	}

	public int size () {
		return items.size ();
	}

	public int getSize () {
		return size ();
	}

	public T get (int index) {
		return items.get (index);
	}

	@Override
	public Object[] toArray () {
		return items.toArray ();
	}

	@Override
	public <T1> T1[] toArray (T1[] a) {
		return items.toArray (a);
	}

	@Override
	public boolean containsAll (Collection<?> c) {
		return items.containsAll (c);
	}

	@Override
	public boolean addAll (Collection<? extends T> c) {
		throw new UnsupportedOperationException ();
	}

	@Override
	public boolean addAll (int index, Collection<? extends T> c) {
		throw new UnsupportedOperationException ();
	}

	@Override
	public boolean removeAll (Collection<?> c) {
		throw new UnsupportedOperationException ();
	}

	@Override
	public boolean retainAll (Collection<?> c) {
		throw new UnsupportedOperationException ();
	}

	@Override
	public boolean contains (Object o) {
		return items.contains (o);
	}

	@Override
	public boolean isEmpty () {
		return items.isEmpty ();
	}

	public Stream<T> stream () {
		return items.stream ();
	}

	public Stream<T> parallelStream () {
		return items.parallelStream ();
	}

	@Override
	public int indexOf (Object value) {
		return items.indexOf (value);
	}

	@Override
	public int lastIndexOf (Object value) {
		return items.lastIndexOf (value);
	}

	@Override
	public ListIterator<T> listIterator () {
		return items.listIterator ();
	}

	@Override
	public ListIterator<T> listIterator (int index) {
		return items.listIterator (index);
	}

	@Override
	public List<T> subList (int fromIndex, int toIndex) {
		return items.subList (fromIndex, toIndex);
	}

	@Override
	public Iterator<T> iterator () {
		return items.iterator ();
	}

}