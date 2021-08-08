function MVVMBindingRelativeSource () {

	this.mode = null;
	this.ancestorType = null;
	this.ancestorLevel = 1;

	this.autoSetMode = function () {
		if (this.ancestorType != null) {
			this.mode = MVVMBindingRelativeSourceMode.findAncestor;
			return;
		}
		this.mode = MVVMBindingRelativeSourceMode.self;
	};

}