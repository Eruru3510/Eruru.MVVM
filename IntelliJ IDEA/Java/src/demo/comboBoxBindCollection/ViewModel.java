package demo.comboBoxBindCollection;

import com.eruru.mvvm.MVVMObservableCollection;

public class ViewModel {

	private MVVMObservableCollection<String> items = new MVVMObservableCollection<String> () {{
		add ("A");
		add ("B");
		add ("C");
	}};

	public MVVMObservableCollection<String> getItems () {
		return items;
	}

	public void setItems (MVVMObservableCollection<String> items) {
		this.items = items;
	}

}
