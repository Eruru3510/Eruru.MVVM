package demo.itemsControlImplementCustomViewList;

import com.eruru.mvvm.MVVMObservableCollection;
import com.eruru.mvvm.MVVMRelayCommand;

public class ViewModel {

	private int id;
	private Item tempItem;
	private MVVMObservableCollection<Item> items = new MVVMObservableCollection<Item> () {{

		add (new Item (0));
		add (new Item (0));
		add (new Item (0));

	}};
	private MVVMRelayCommand onAdd = new MVVMRelayCommand (value -> {
		items.add (newItem ());
	});
	private MVVMRelayCommand onSetNew = new MVVMRelayCommand (value -> {
		items.set (items.indexOf ((Item) value), newItem ());
	});
	private MVVMRelayCommand onMove = new MVVMRelayCommand (value -> {
		Item item = (Item) value;
		if (tempItem == null) {
			tempItem = item;
			return;
		}
		int oldIndex = items.indexOf (tempItem);
		tempItem = null;
		if (oldIndex < 0) {
			return;
		}
		items.move (oldIndex, items.indexOf (item));
	});
	private MVVMRelayCommand onDelete = new MVVMRelayCommand (value -> {
		items.remove ((Item) value);
	});
	private MVVMRelayCommand onInsert = new MVVMRelayCommand (value -> {
		Item item = (Item) value;
		items.add (items.indexOf (item), newItem ());
	});
	private MVVMRelayCommand onClear = new MVVMRelayCommand (value -> {
		items.clear ();
	});

	public MVVMObservableCollection<Item> getItems () {
		return items;
	}

	public void setItems (MVVMObservableCollection<Item> items) {
		this.items = items;
	}

	public MVVMRelayCommand getOnAdd () {
		return onAdd;
	}

	public void setOnAdd (MVVMRelayCommand onAdd) {
		this.onAdd = onAdd;
	}

	public MVVMRelayCommand getOnSetNew () {
		return onSetNew;
	}

	public void setOnSetNew (MVVMRelayCommand onSetNew) {
		this.onSetNew = onSetNew;
	}

	public MVVMRelayCommand getOnMove () {
		return onMove;
	}

	public void setOnMove (MVVMRelayCommand onMove) {
		this.onMove = onMove;
	}

	public MVVMRelayCommand getOnDelete () {
		return onDelete;
	}

	public void setOnDelete (MVVMRelayCommand onDelete) {
		this.onDelete = onDelete;
	}

	public MVVMRelayCommand getOnClear () {
		return onClear;
	}

	public void setOnClear (MVVMRelayCommand onClear) {
		this.onClear = onClear;
	}

	private Item newItem () {
		return new Item (id++);
	}

	public MVVMRelayCommand getOnInsert () {
		return onInsert;
	}

	public void setOnInsert (MVVMRelayCommand onInsert) {
		this.onInsert = onInsert;
	}

}