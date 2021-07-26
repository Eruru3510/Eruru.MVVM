package demo.clickButton;

import com.eruru.mvvm.MVVMRelayCommand;

import javax.swing.*;

public class ViewModel {

	private MVVMRelayCommand onClick = new MVVMRelayCommand (value -> {
		JOptionPane.showMessageDialog (null, "You clicked this button");
	});

	public MVVMRelayCommand getOnClick () {
		return onClick;
	}

	public void setOnClick (MVVMRelayCommand onClick) {
		this.onClick = onClick;
	}

}