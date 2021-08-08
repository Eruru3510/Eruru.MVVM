function MVVMEvent () {

	this.actions = [];

	this.add = function (instance, action) {
		//console.log ("注册", instance);
		this.actions.push ({ key: instance, value: action })
	};

	this.remove = function (instance) {
		for (var i = 0; i < this.actions.length; i++) {
			if (this.actions[i].key === instance) {
				this.actions.splice (i, 1);
				break;
			}
		}
	};

	this.invoke = function () {
		for (var i = 0; i < this.actions.length; i++) {
			var stringBuilder = new MVVMStringBuilder ();
			for (var n = 0; n < arguments.length; n++) {
				if (n > 0) {
					stringBuilder.append (', ');
				}
				stringBuilder.append ("arguments[" + n + "]");
			}
			//console.log ("通知", this.actions[i].key);
			var value = this.actions[i].value;
			var key = this.actions[i].key;
			eval ("value.call (key, " + stringBuilder.toString () + ")");
		}
	};

}