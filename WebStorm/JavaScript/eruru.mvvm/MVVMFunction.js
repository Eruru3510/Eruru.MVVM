function MVVMFunction (action) {

	this.action = action;
	this.parameters = [];
	for (var i = 1; i < arguments.length; i++) {
		this.parameters[i - 1] = arguments[i];
	}

	this.getParameter = function (index) {
		return this.parameters[index];
	};

	this.setParameter = function (instance, index, value) {
		this.parameters[index] = value;
		var stringBuilder = new MVVMStringBuilder ();
		for (var i = 0; i < this.parameters.length; i++) {
			if (i > 0) {
				stringBuilder.append (', ');
			}
			stringBuilder.append ("that.parameters[" + i + "]");
		}
		var that = this;
		eval ("that.action.call (instance, " + stringBuilder.toString () + ")");
	};

}