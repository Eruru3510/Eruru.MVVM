function MVVMRelayCommand  (execute, canExecute) {

	this._execute = execute;
	this._canExecute = canExecute;

	this.canExecute = function (parameter) {
		return this._canExecute == null || this._canExecute (parameter);
	};

	this.execute = function (parameter) {
		if (this._execute != null) {
			this._execute (parameter);
		}
	};

}