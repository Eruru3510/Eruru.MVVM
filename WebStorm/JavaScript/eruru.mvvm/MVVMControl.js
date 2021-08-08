function MVVMControl (control) {

	this.root = null;
	this.control = control;
	this.bindings = [];
	this.controlNames = [];
	this.controls = [];
	this.name = null;
	this.control.mvvmcontrol = this;
	this.itemsControlControls = [];
	this.mvvmPropertyChanged = new MVVMEvent ();

	this.registerControl = function (control) {
		control.parent = this;
		control.root = this.root;
	};

	this.add = function (control) {
		this.insert (this.controls.length, control);
	};

	this.insert = function (index, control) {
		this.registerControl (control);
		this.controls.splice (index, 0, control);
	};

	this.removeAt = function (index) {
		this.controls.splice (index, 1);
		this.controls[index].unbind ();
	};

	this.move = function (oldIndex, newIndex) {
		var control = this.controls[oldIndex];
		this.controls.splice (oldIndex, 1);
		this.controls.splice (newIndex, 0, control);
	};

	this.set = function (index, control) {
		this.controls[index].unbind ();
		this.controls[index] = control;
		this.registerControl (control);
	};

	this.clear = function () {
		for (var i = 0; i < this.controls.length; i++) {
			this.controls[i].unbind ();
		}
		this.controls.splice (0);
	};

	this.setBinding = function (binding, setTargetValue, unbind, isDataContext, propertyName) {
		if (!isDataContext && !binding) {
			this.bindings.push (binding);
		}
		if (binding == null) {
			return null;
		}
		this[propertyName] = binding;
		binding.control = this;
		binding.onUnbind = unbind;
		binding.onSetTargetValue = setTargetValue;
		binding.targetPropertyName = propertyName;
		binding.isDataContext = isDataContext;
		binding.defaultMode = MVVMBindingMode.oneWay;
		binding.defaultUpdateSourceTrigger = MVVMBindingUpdateSourceTrigger.propertyChanged;
		binding.autoSetType ();
		var control = this;
		switch (propertyName) {
			case "value":
				this.control.addEventListener ("input", function () {
					this.mvvmcontrol.onChanged (this.mvvmcontrol.value, MVVMBindingOnChangedType.propertyChanged);
				});
				this.control.addEventListener ("blur", function () {
					this.mvvmcontrol.onChanged (this.mvvmcontrol.value, MVVMBindingOnChangedType.lostFocus);
				});
				binding.defaultMode = MVVMBindingMode.twoWay;
				binding.defaultUpdateSourceTrigger = MVVMBindingUpdateSourceTrigger.lostFocus;
				break;
			case "class":
				binding.blockSetTargetValue = true;
				binding.onSetTargetValue = function (targetValue) {
					if (typeof targetValue === "boolean") {
						if (targetValue) {
							control.control.classList.add (binding.sourcePropertyName);
						} else {
							control.control.classList.remove (binding.sourcePropertyName);
						}
						return;
					}
					control.control.className = targetValue;
				};
				break;
			case "itemssource":
				binding.blockSetTargetValue = true;
				this.itemTemplate = mvvmFindByAttribute (this.control, "mvvmitemtemplate");
				this.itemTemplate.style.display = "none";
				binding.onSetTargetValue = function (targetValue) {
					control.cancelCollectionChanged ();
					control.itemsControlReset ();
					if (targetValue != null && targetValue.forEach) {
						targetValue.forEach (function (value) {
							control.itemsControlAdd (value);
						});
					}
					if (targetValue instanceof MVVMObservableCollection) {
						this.notifyCollectionChanged = targetValue;
						this.notifyCollectionChanged.collectionChanged.add (this, control.collectionChanged);
					}
				};
				binding.onUnbind = this.cancelCollectionChanged;
				break;
			default:
				if (mvvmStringStartsWith (propertyName, "on")) {
					binding.blockSetTargetValue = true;
					this.control.addEventListener (propertyName.substring (2), function () {
						this.mvvmcontrol.onCommand (this.mvvmcontrol[propertyName], this.mvvmcontrol[propertyName + "parameter"]);
					});
				}
				break;
		}
		if (!isDataContext) {
			this.bindings.push (binding);
		}
		return binding;
	};

	this.setDataContext = function (binding) {
		binding.blockSetTargetValue = true;
		this.setBinding (binding, function (targetValue) {
			for (var i = 0; i < this.bindings.length; i++) {
				this.bindings[i].bind ();
			}
			for (i = 0; i < this.controls.length; i++) {
				this.controls[i].getDataContext ().bind ();
			}
		}, null, true, "dataContext");
	};

	this.getDataContext = function () {
		if (!this.hasOwnProperty ("dataContext")) {
			this.setDataContext (new MVVMBinding ());
		}
		return this.dataContext;
	}

	this.build = function (root) {
		if (root == null) {
			root = this.root == null ? this : this.root;
		}
		this.controlNames = [];
		this.unbind ();
		this.forEach (function (control) {
			control.register (root);
		});
		this.getDataContext ().bind ();
		return this;
	};

	this.forEach = function (action) {
		action (this);
		for (var i = 0; i < this.controls.length; i++) {
			this.controls[i].forEach (action);
		}
	};

	this.register = function (root) {
		this.root = root;
		if (this.name != null) {
			root.controlNames.put (this.name.getTargetValue (), this);
		}
	};

	this.unbind = function () {
		this.forEach (function (control) {
			control.getDataContext ().unbind ();
			for (var i = 0; i < control.bindings.length; i++) {
				control.bindings[i].unbind ();
			}
		});
	};

	this.onChanged = function (binding, onChangedType) {
		if (binding == null || binding.blockOnChanged) {
			return;
		}
		switch (binding.getMode ()) {
			case MVVMBindingMode.twoWay:
			case MVVMBindingMode.oneWayToSource:
				switch (binding.getUpdateSourceTrigger ()) {
					case MVVMBindingUpdateSourceTrigger.propertyChanged:
						if (onChangedType === MVVMBindingOnChangedType.propertyChanged) {
							binding.oneWaySetSourceValue ();
						}
						break;
					case MVVMBindingUpdateSourceTrigger.lostFocus:
						if (onChangedType === MVVMBindingOnChangedType.lostFocus) {
							binding.oneWaySetSourceValue ();
						}
						break;
				}
				break;
		}
		this.mvvmPropertyChanged.invoke (this, binding.targetPropertyName);
	};

	this.onCommand = function (command, parameter) {
		if (command == null) {
			return;
		}
		var relayCommand = command.getTargetValue ();
		if (relayCommand == null) {
			return;
		}
		var value = parameter == null ? null : parameter.getTargetValue ();
		if (relayCommand instanceof Function) {
			relayCommand.call (command.instance, value);
			return;
		}
		if (relayCommand.canExecute (value)) {
			relayCommand.execute (value);
		}
	}

	this.cancelCollectionChanged = function () {
		if (this.notifyCollectionChanged != null) {
			this.notifyCollectionChanged.collectionChanged.remove (this);
			this.notifyCollectionChanged = null;
		}
	}

	this.collectionChanged = function (sender, action, oldIndex, oldItems, newIndex, newItems) {
		switch (action) {
			case MVVMCollectionChangedAction.add:
				this.itemsControlInsert (newItems[0], newIndex);
				break;
			case MVVMCollectionChangedAction.remove:
				this.itemsControlRemoveAt (oldIndex);
				break;
			case MVVMCollectionChangedAction.replace:
				this.itemsControlReplace (oldIndex, newItems[0]);
				break;
			case MVVMCollectionChangedAction.move:
				this.itemsControlMove (oldIndex, newIndex);
				break;
			case MVVMCollectionChangedAction.reset:
				this.itemsControlReset ();
				break;
			default:
				throw "未实现" + action;
		}
	}

	this.itemsControlConvert = function (value) {
		var node = this.itemTemplate.cloneNode (true);
		node.removeAttribute ("mvvmitemtemplate");
		node.style.display = "";
		return mvvm.compile (node);
	};

	this.itemsControlRegister = function (value, index) {
		var control = this.itemsControlConvert (value);
		control.parent = this;
		control.root = this.root;
		control.setDataContext (new MVVMValueBinding (value));
		control.build (this.root);
		this.itemsControlControls.splice (index, 0, control);
		var node = this.itemTemplate;
		for (var i = 0; i <= index; i++) {
			node = node.nextSibling;
		}
		this.itemTemplate.parentNode.insertBefore (control.control, node);
	};

	this.itemsControlAdd = function (value) {
		this.itemsControlInsert (value, this.itemsControlControls.length);
	};

	this.itemsControlInsert = function (value, index) {
		this.itemsControlRegister (value, index);
	};

	this.itemsControlRemoveAt = function (index) {
		this.itemTemplate.parentNode.removeChild (this.itemsControlControls[index].control);
		this.itemsControlControls[index].unbind ();
		this.itemsControlControls.splice (index, 1);
	};

	this.itemsControlReplace = function (index, value) {
		this.itemsControlRemoveAt (index);
		this.itemsControlInsert (value, index);
	};

	this.itemsControlMove = function (oldIndex, newIndex) {

	};

	this.itemsControlReset = function () {
		for (var i = this.itemsControlControls.length - 1; i >= 0; i--) {
			this.itemsControlRemoveAt (i);
		}
	};

}