function MVVMStringBuilder () {

	this.items = [];

	this.append = function (value) {
		this.items.push (value);
	};

	this.clear = function () {
		this.items.splice (0);
	};

	this.toString = function () {
		return this.items.join ("");
	};

}