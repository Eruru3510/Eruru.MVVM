package com.eruru.mvvm;

import java.util.AbstractMap;
import java.util.ArrayList;
import java.util.ConcurrentModificationException;
import java.util.Map;

public class MVVMEvent<Instance, Action> {

	private final ArrayList<Map.Entry<Object, Action>> events = new ArrayList<> ();
	private final Instance instance;

	public MVVMEvent (Instance instance) {
		this.instance = instance;
	}

	public Instance getInstance () {
		return instance;
	}

	public void add (Object instance, Action action) {
		events.add (new AbstractMap.SimpleEntry<> (instance, action));
	}

	public void remove (Object instance) {
		for (int i = 0; i < events.size (); i++) {
			if (events.get (i).getKey () == instance) {
				events.remove (i);
				break;
			}
		}
	}

	protected void forEach (MVVMAction1<Action> action) {
		try {
			for (Map.Entry<Object, Action> event : events) {
				action.invoke (event.getValue ());
			}
		} catch (ConcurrentModificationException ignored) {

		}
	}

}