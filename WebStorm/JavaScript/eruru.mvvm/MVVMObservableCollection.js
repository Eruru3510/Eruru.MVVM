MVVMObservableCollection.prototype = [];

function MVVMObservableCollection (items) {

	this.collectionChanged = new MVVMEvent ();
	this.mvvmPropertyChanged = new MVVMEvent ();
	this.oldItems = [ null ];
	this.newItems = [ null ];
	this.lengthName = "length";

	this.initialize = function () {
		if (items != null) {
			this.addRange (items);
		}
	};

	this.add = function (value) {
		this.insert (this.length, value);
	};

	this.addRange = function (values) {
		this.insertRange (this.length, values);
	};

	this.insert = function (index, value) {
		this.newItems[0] = value;
		this.splice (index, 0, value);
		this.mvvmPropertyChanged.invoke (this, this.lengthName);
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.add, index, this.newItems, index, this.newItems);
	};

	this.insertRange = function (index, values) {
		var newItems = values;
		var i = 0;
		var that = this;
		values.forEach (function (value) {
			that.splice (index + i, 0, value);
			i++;
		});
		this.mvvmPropertyChanged.invoke (this, this.lengthName);
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.add, index, newItems, index, newItems);
	};

	this.move = function (oldIndex, newIndex) {
		this.oldItems[0] = this[oldIndex];
		this.splice (oldIndex, 1);
		this.splice (newIndex, 0, this.oldItems[0]);
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.move, oldIndex, this.oldItems, newIndex, this.oldItems);
	};

	this.removeAt = function (index) {
		this.oldItems[0] = this[index];
		this.splice (index, 1);
		this.mvvmPropertyChanged.invoke (this, this.lengthName);
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.remove, index, this.oldItems, index, this.oldItems);
	};

	this.remove = function (value) {
		var index = this.indexOf (value);
		if (index < 0) {
			return false;
		}
		this.removeAt (index);
		return true;
	};

	this.removeAll = function (func) {
		var count = 0;
		for (var i = 0; i < this.length; i++) {
			if (func (this[i])) {
				count++;
				this.removeAt (i--);
			}
		}
		return count;
	};

	this.removeRange = function (index, count) {
		var oldItems = this.splice (index, count);
		this.mvvmPropertyChanged.invoke (this, this.lengthName);
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.remove, index, oldItems, index, oldItems);
	};

	this.set = function (index, value) {
		this.oldItems[0] = this[index];
		this.newItems[0] = value;
		this[index] = value;
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.replace, index, this.oldItems, index, this.newItems);
	};

	this.clear = function () {
		this.splice (0);
		this.mvvmPropertyChanged.invoke (this, this.lengthName);
		this.collectionChanged.invoke (this, MVVMCollectionChangedAction.reset, -1, null, -1, null);
	};

	this.forEach = function (action) {
		for (var i = 0; i < this.length; i++) {
			action (this[i]);
		}
	};

	this.get = function (index) {
		return this[index];
	};

	this.indexOf = function (value) {
		for (var i = 0; i < this.length; i++) {
			if (this[i] === value) {
				return i;
			}
		}
		return -1;
	};

	this.lastIndexOf = function (value) {
		for (var i = this.length - 1; i >= 0; i--) {
			if (this[i] === value) {
				return i;
			}
		}
		return -1;
	};

	this.initialize ();

}