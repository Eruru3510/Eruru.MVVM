package demo.table;

import com.eruru.mvvm.MVVMNotifyPropertyChanged;
import com.eruru.mvvm.MVVMObservableCollection;
import com.eruru.mvvm.MVVMPropertyChanged;
import com.eruru.mvvm.MVVMRelayCommand;

import javax.swing.*;

public class ViewModel implements MVVMNotifyPropertyChanged {

	private int id = 3;
	private String name = "书4";
	private MVVMObservableCollection<Item> items = new MVVMObservableCollection<Item> () {{
		add (new Item (0, "书1"));
		add (new Item (1, "书2"));
		add (new Item (2, "书3"));
	}};
	private MVVMRelayCommand onAdd = new MVVMRelayCommand (value -> {
		if (items.stream ().anyMatch (item -> item.id == id)) {
			JOptionPane.showMessageDialog (null, String.format ("添加失败，ID：%d 的项已存在", id));
			setId (id + 1);
			return;
		}
		items.add (new Item (id, name));
		setId (id + 1);
	});
	private final MVVMPropertyChanged propertyChanged = new MVVMPropertyChanged (this);

	@Override
	public MVVMPropertyChanged getPropertyChanged () {
		return propertyChanged;
	}

	public MVVMObservableCollection<Item> getItems () {
		return items;
	}

	public void setItems (MVVMObservableCollection<Item> items) {
		this.items = items;
	}

	public int getId () {
		return id;
	}

	public void setId (int id) {
		this.id = id;
		propertyChanged.invoke ();
	}

	public String getName () {
		return name;
	}

	public void setName (String name) {
		this.name = name;
		propertyChanged.invoke ();
	}

	public MVVMRelayCommand getOnAdd () {
		return onAdd;
	}

	public void setOnAdd (MVVMRelayCommand onAdd) {
		this.onAdd = onAdd;
	}

}
