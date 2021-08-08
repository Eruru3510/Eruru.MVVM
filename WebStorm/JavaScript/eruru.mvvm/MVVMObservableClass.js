function MVVMObservableClass (instance) {

	this.instance = instance;
	this.classChanged = new MVVMEvent ();

	for (var name in instance) {
		var fieldName = "mvvm" + name;
		Object.defineProperty (this.instance, name, {
			get: function () {
				return this.instance[fieldName];
			},
			set: function (value) {
				this.instance[fieldName] = value;
			}
		});
	}

}