package demo.usePropertyChangedToNotifyTextfieldUpdateValue;

import com.eruru.mvvm.MVVMNotifyPropertyChanged;
import com.eruru.mvvm.MVVMPropertyChanged;
import com.eruru.mvvm.MVVMRelayCommand;

import java.util.Date;

public class ViewModel implements MVVMNotifyPropertyChanged {

	private String text = "This is a default value";
	private MVVMRelayCommand onClick = new MVVMRelayCommand (value -> {
		setText (new Date ().toString ());
	});
	private final MVVMPropertyChanged propertyChanged = new MVVMPropertyChanged (this);

	@Override
	public MVVMPropertyChanged getPropertyChanged () {
		return propertyChanged;
	}

	public String getText () {
		return text;
	}

	public void setText (String text) {
		System.out.printf ("Text has changed. %s to %s\n", this.text, text);
		this.text = text;
		propertyChanged.invoke ();
	}

	public MVVMRelayCommand getOnClick () {
		return onClick;
	}

	public void setOnClick (MVVMRelayCommand onClick) {
		this.onClick = onClick;
	}

}