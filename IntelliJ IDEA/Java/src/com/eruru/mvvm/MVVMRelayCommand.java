package com.eruru.mvvm;

public class MVVMRelayCommand {

	MVVMPredicate1<Object> _CanExecute;
	MVVMAction1<Object> _Execute;

	public MVVMRelayCommand (MVVMAction1<Object> execute, MVVMPredicate1<Object> canExecute) {
		_Execute = execute;
		_CanExecute = canExecute;
	}

	public MVVMRelayCommand (MVVMAction1<Object> execute) {
		_Execute = execute;
	}

	public boolean canExecute (Object parameter) {
		return _CanExecute == null || _CanExecute.invoke (parameter);
	}

	public void execute (Object parameter) {
		if (_Execute != null) {
			_Execute.invoke (parameter);
		}
	}

}
