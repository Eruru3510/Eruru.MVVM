function mvvmDefineProperty (instance, fieldName, propertyName) {
	Object.defineProperty (instance, propertyName, {
		get: function () {
			return this[fieldName];
		},
		set: function (value) {
			this[fieldName] = value;
			this["mvvmPropertyChanged"].invoke (this, propertyName);
		}
	});
}

function mvvmStringStartsWith (text, starts) {
	if (text.length < starts.length) {
		return false;
	}
	for (var i = 0; i < starts.length; i++) {
		if (text[i] !== starts[i]) {
			return false;
		}
	}
	return true;
}

function mvvmNodeIndexOf (parent, node) {
	for (var i = 0; i < parent.childNodes.length; i++) {
		if (parent.childNodes[i] === node) {
			return i;
		}
	}
	return -1;
}

function mvvmFindByAttribute (node, attributeName) {
	for (var i = 0; i < node.childNodes.length; i++) {
		if (node.childNodes[i].nodeType !== 1) {
			continue;
		}
		if (node.childNodes[i].hasAttribute (attributeName)) {
			return node.childNodes[i];
		}
		var result = mvvmFindByAttribute (node.childNodes[i], attributeName);
		if (result != null) {
			return result;
		}
	}
	return null;
}

function mvvmStringFormat (format) {
	var stringBuilder = new MVVMStringBuilder ();
	var index = 1;
	var isStarted = false;
	for (var i = 0; i < format.length; i++) {
		if (format[i] === '%') {
			isStarted = true;
			continue;
		}
		if (isStarted) {
			isStarted = false;
			stringBuilder.append (arguments[index]);
			continue;
		}
		stringBuilder.append (format[i]);
	}
	return stringBuilder.toString ();
}

function mvvmGetEnumName (enumInstance, index) {
	for (var value in enumInstance) {
		if (enumInstance[value] === index) {
			return value;
		}
	}
	return null;
}function MVVMValueBinding (value) {
	var instance = new MVVMBinding ();
	instance.type = MVVMBindingType.value;
	instance.targetValue = value;
	return instance;
}

function MVVMBinding () {

	this.type = null;
	this.control = null;
	this.targetPropertyName = null;
	this.targetValue = null;
	this.sourcePropertyName = null;
	this.sourceValue = null;
	this.instance = null;
	this.blockSetSourceValue = false;
	this.blockOnChanged = false;
	this.mode = MVVMBindingMode.default;
	this.defaultMode = null;
	this.isDataContext = false;
	this.updateSourceTrigger = MVVMBindingUpdateSourceTrigger.default;
	this.defaultUpdateSourceTrigger = null;
	this.onSetTargetValue = null;
	this.notifyPropertyChangeds = [];
	this.isFirstBind = false;
	this.onDebind = null;
	this.path = null;
	this.element = null;
	this.elementName = null;
	this.relativeSource = null;
	this.blockSetTargetValue = false;
	this.parameter = null;
	this.stringFormat = null;

	this.autoSetType = function () {
		if (this.type === MVVMBindingType.value) {
			return;
		}
		if (this.relativeSource != null) {
			this.type = MVVMBindingType.relative;
			return;
		}
		if (this.element != null) {
			this.type = MVVMBindingType.element;
			return;
		}
		if (this.elementName != null) {
			this.type = MVVMBindingType.elementName;
			return;
		}
		this.type = MVVMBindingType.path;
	};

	this.getTargetValue = function () {
		if (this.blockSetTargetValue) {
			return this.targetValue;
		}
		return this.control.control[this.targetPropertyName];
	};

	this.setTargetValue = function (value) {
		if (this.stringFormat != null) {
			value = mvvmStringFormat (this.stringFormat, value);
		}
		this.targetValue = value;
		if (!this.blockSetTargetValue) {
			this.blockOnChanged = true;
			this.control.control[this.targetPropertyName] = value;
			this.blockOnChanged = false;
		}
		if (this.onSetTargetValue != null) {
			this.onSetTargetValue.call (this.control, value);
		}
		this.control.onChanged (this);
	};

	this.getSourceValue = function () {
		switch (this.type) {
			case MVVMBindingType.value:
				return this.targetValue;
			case MVVMBindingType.path:
			case MVVMBindingType.elementName:
			case MVVMBindingType.relative:
				if (this.sourcePropertyName == null) {
					return this.sourceValue;
				}
				var value = this.instance[this.sourcePropertyName];
				if (value instanceof MVVMBinding) {
					return value.getTargetValue ();
				}
				if (value instanceof MVVMFunction) {
					value = value.getParameter (this.parameter);
				}
				return value;
			default:
				throw "未实现" + this.type;
		}
	};

	this.setSourceValue = function (value) {
		if (this.blockSetSourceValue) {
			return;
		}
		this.sourceValue = value;
		if (this.sourcePropertyName != null) {
			var sourceValue = this.instance[this.sourcePropertyName];
			if (sourceValue instanceof MVVMBinding) {
				sourceValue.setTargetValue (value);
				return;
			}
			if (sourceValue instanceof MVVMFunction) {
				sourceValue.setParameter (this.instance, this.parameter, value);
				return;
			}
			this.instance[this.sourcePropertyName] = value;
		}
	};

	this.updateSource = function () {
		switch (this.getMode ()) {
			case MVVMBindingMode.twoWay:
			case MVVMBindingMode.oneWayToSource:
				break;
			default:
				return;
		}
		this.setSourceValue (this.getTargetValue ());
	}

	this.updateTarget = function () {
		this.setTargetValue (this.getSourceValue ());
	}

	this.bind = function () {
		if (!this.isFirstBind) {
			switch (this.type) {
				case MVVMBindingType.value:
				case MVVMBindingType.element:
				case MVVMBindingType.elementName:
				case MVVMBindingType.relative:
					return;
			}
		}
		this.isFirstBind = false;
		this.unbind ();
		switch (this.type) {
			case MVVMBindingType.value:
				break;
			case MVVMBindingType.path: {
				var dataContext = null;
				if (this.isDataContext) {
					if (this.control.parent != null) {
						dataContext = this.control.parent.getDataContext ().getTargetValue ();
					}
				} else {
					dataContext = this.control.getDataContext ().getTargetValue ();
				}
				this.bindSource (dataContext);
				break;
			}
			case MVVMBindingType.element:
				this.bindSource (this.element);
				break;
			case MVVMBindingType.elementName: {
				this.bindSource (this.control.root.find (this.elementName));
				break;
			}
			case MVVMBindingType.relative: {
				var control = this.control;
				switch (this.relativeSource.mode) {
					case MVVMBindingRelativeSourceMode.self:
						break;
					case MVVMBindingRelativeSourceMode.findAncestor:
						var ancestorCount = 0;
						while (control != null) {
							if (this.relativeSource.ancestorType == null || this.relativeSource.ancestorType.toUpperCase () === control.control.tagName) {
								ancestorCount++;
								if (ancestorCount === this.relativeSource.ancestorLevel) {
									break;
								}
							}
							control = control.parent;
						}
						break;
					default:
						throw "未实现" + this.relativeSource.mode;
				}
				this.bindSource (control);
				break;
			}
			default:
				throw "未实现" + this.type;
		}
		this.oneWaySetTargetValue ();
	};

	this.bindSource = function (dataContext) {
		this.sourceValue = dataContext;
		if (this.sourceValue != null && this.path != null) {
			var instance = this.sourceValue;
			var pathNodes = this.path.split (".");
			for (var i = 0; i < pathNodes.length; i++) {
				var propertyName = pathNodes[i];
				if (!instance.hasOwnProperty (pathNodes[i])) {
					throw "绑定" + pathNodes.join (".") + "失败，" + instance + "下没有公开的属性" + pathNodes[i];
				}
				if (!instance.hasOwnProperty ("mvvmPropertyChanged")) {
					instance.mvvmPropertyChanged = new MVVMEvent ();
				}
				instance.mvvmPropertyChanged.add (this, this.onPropertyChanged);
				this.notifyPropertyChangeds.push ({ key: instance, value: pathNodes[i] });
				var value = instance[propertyName];
				if (!(instance instanceof MVVMControl)) {
					if (value instanceof Array) {
						value = new MVVMObservableCollection (value);
					}
					var fieldName = "mvvm" + propertyName;
					if (!instance.hasOwnProperty (fieldName)) {
						instance[fieldName] = value;
						mvvmDefineProperty (instance, fieldName, propertyName);
					}
				}
				if (i === pathNodes.length - 1) {
					this.instance = instance;
					this.sourcePropertyName = propertyName;
					break;
				}
				instance = value;
				if (instance instanceof MVVMBinding) {
					instance = instance.getTargetValue ();
				}
				if (instance == null) {
					break;
				}
			}
		}
	};

	this.getMode = function () {
		return this.mode === MVVMBindingMode.default ? this.defaultMode : this.mode;
	}

	this.getUpdateSourceTrigger = function () {
		return this.updateSourceTrigger === MVVMBindingUpdateSourceTrigger.default ? this.defaultUpdateSourceTrigger : this.updateSourceTrigger;
	}

	this.oneWaySetTargetValue = function () {
		this.blockSetSourceValue = true;
		this.setTargetValue (this.getSourceValue ());
		this.blockSetSourceValue = false;
	}

	this.oneWaySetSourceValue = function () {
		this.blockOnPropertyChanged = true;
		this.setSourceValue (this.getTargetValue ());
		this.blockOnPropertyChanged = false;
	}

	this.unbind = function () {
		this.sourcePropertyName = null;
		for (var i = 0; i < this.notifyPropertyChangeds.length; i++) {
			this.notifyPropertyChangeds[i].key.mvvmPropertyChanged.remove (this);
		}
		this.notifyPropertyChangeds = [];
		if (this.onDebind != null) {
			this.onDebind.invoke ();
		}
		this.isFirstBind = true;
	};

	this.onPropertyChanged = function (sender, propertyName) {
		if (this.blockOnPropertyChanged) {
			return;
		}
		switch (this.getMode ()) {
			case MVVMBindingMode.twoWay:
			case MVVMBindingMode.oneWay:
				for (var i = 0; i < this.notifyPropertyChangeds.length; i++) {
					if (this.notifyPropertyChangeds[i].key === sender && propertyName === this.notifyPropertyChangeds[i].value) {
						//console.log ("收到通知", sender, propertyName);
						if (i === this.notifyPropertyChangeds.length - 1) {
							this.oneWaySetTargetValue ();
							break;
						}
						this.bind ();
						break;
					}
				}
				break;
		}
	};

}var MVVMBindingMode = {

	twoWay: 1,
	oneWay: 2,
	oneTime: 3,
	oneWayToSource: 4,
	default: 5

};var MVVMBindingOnChangedType = {

	propertyChanged: 1,
	lostFocus: 2

};function MVVMBindingRelativeSource () {

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

}var MVVMBindingRelativeSourceMode = {

	previousData: 1,
	templatedParent: 2,
	self: 3,
	findAncestor: 4

};var MVVMBindingType = {

	value: 1,
	path: 2,
	element: 3,
	elementName: 4,
	relative: 5

};var MVVMBindingUpdateSourceTrigger = {

	default: 1,
	propertyChanged: 2,
	lostFocus: 3,
	explicit: 4

};function MVVMClassConverter (text) {

	this.lexicalAnalyzer = new MVVMLexicalAnalyzer (text, MVVMTokenType.end, MVVMTokenType.unknown, MVVMTokenType.integer, MVVMTokenType.decimal, MVVMTokenType.string);
	this.lexicalAnalyzer.allowString = false;
	this.lexicalAnalyzer.allowNumber = false;

	this.parse = function (defaultName, allowValue) {
		var isValue = false;
		if (this.lexicalAnalyzer.getCurrent ().type === MVVMTokenType.leftBrace) {
			this.lexicalAnalyzer.moveNext ();
		} else {
			if (allowValue) {
				isValue = true;
			} else {
				return;
			}
		}
		var name = null;
		var parameters = [];
		var isValid = false;
		while (this.lexicalAnalyzer.getCurrent ().type !== MVVMTokenType.end) {
			if (this.lexicalAnalyzer.getCurrent ().type === MVVMTokenType.rightBrace) {
				isValid = true;
				break;
			}
			if (this.lexicalAnalyzer.getCurrent ().type !== MVVMTokenType.unknown) {
				throw "期望是文本" + mvvmGetEnumName (MVVMTokenType, this.lexicalAnalyzer.getCurrent ().type) + "\n" + text;
			}
			if (name == null) {
				name = this.lexicalAnalyzer.getCurrent ().value;
			} else {
				var parameter = { key: this.lexicalAnalyzer.getCurrent ().value };
				parameters.push (parameter);
				this.lexicalAnalyzer.moveNext ();
				switch (this.lexicalAnalyzer.getCurrent ().type) {
					case MVVMTokenType.equalSign:
						this.lexicalAnalyzer.moveNext ();
						switch (this.lexicalAnalyzer.getCurrent ().type) {
							case MVVMTokenType.leftBrace:
								parameter.value = this.parse (parameter.key);
								break;
							case MVVMTokenType.unknown:
								switch (parameter.key) {
									case "relativeSource":
										parameter.value = new MVVMBindingRelativeSource ();
										parameter.value.ancestorType = this.lexicalAnalyzer.getCurrent ().value;
										parameter.value.autoSetMode ();
										break;
									default:
										parameter.value = this.lexicalAnalyzer.getCurrent ().value;
										break;
								}
								break;
							default:
								throw "期望是参数值，实际为：" + mvvmGetEnumName (MVVMTokenType, this.lexicalAnalyzer.getCurrent ().type) + "\n" + text;
						}
						this.lexicalAnalyzer.moveNext ();
						switch (this.lexicalAnalyzer.getCurrent ().type) {
							case MVVMTokenType.comma:
								break;
							case MVVMTokenType.rightBrace:
								isValid = true;
								break;
							default:
								throw "期望是','或'}'，实际为：" + mvvmGetEnumName (MVVMTokenType, this.lexicalAnalyzer.getCurrent ().type) + "\n" + text;
						}
						break;
					case MVVMTokenType.comma:
						break;
					case MVVMTokenType.rightBrace:
						isValid = true;
						break;
					default:
						throw "期望是'='或','或'}'，实际为：" + mvvmGetEnumName (MVVMTokenType, this.lexicalAnalyzer.getCurrent ().type) + "\n" + text;
				}
			}
			if (isValid) {
				break;
			}
			this.lexicalAnalyzer.moveNext ();
		}
		if (isValue) {
			return new MVVMValueBinding (name);
		}
		if (!isValid) {
			return null;
		}
		var instance;
		switch (name) {
			case "binding":
			case "relativeSource":
				break;
			default:
				parameters.splice (0, 0, { key: name });
				name = defaultName == null ? "binding" : defaultName;
				break;
		}
		switch (name) {
			case "binding":
				instance = new MVVMBinding ();
				this.setParameters (instance, "path", [
					{ key: "mode", value: MVVMBindingMode },
					{ key: "updateSourceTrigger", value: MVVMBindingUpdateSourceTrigger }
				], parameters);
				break;
			case "relativeSource":
				instance = new MVVMBindingRelativeSource ();
				this.setParameters (instance, "ancestorType", [
					{ key: "mode", value: MVVMBindingRelativeSourceMode }
				], parameters);
				instance.autoSetMode ();
				break;
		}
		return instance;
	};

	this.parseAll = function (action) {
		var start = this.lexicalAnalyzer.position;
		do {
			this.lexicalAnalyzer.readTo ('{');
			if (this.lexicalAnalyzer.peek () < 0) {
				break;
			}
			var end = this.lexicalAnalyzer.position;
			this.lexicalAnalyzer.moveNext ();
			var instance = this.parse ();
			if (instance == null) {
				this.lexicalAnalyzer.position = end + 1;
			} else {
				action.call (this, start, end - start, instance);
				start = this.lexicalAnalyzer.position;
			}
		} while (this.lexicalAnalyzer.peek () > -1);
	};

	this.setParameters = function (instance, defaultParameterName, enums, parameters) {
		for (var i = 0; i < parameters.length; i++) {
			if (i === 0 && parameters[i].value == null) {
				instance[defaultParameterName] = parameters[i].key;
				continue;
			}
			var value = parameters[i].value;
			for (var n = 0; n < enums.length; n++) {
				if (enums[i].key === parameters[i].key) {
					value = enums[i].value[value];
				}
			}
			instance[parameters[i].key] = value;
		}
	};

}var MVVMCollectionChangedAction = {

	add: 0,
	move: 3,
	remove: 1,
	replace: 2,
	reset: 4

};function MVVMControl (control) {

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

}function MVVMEvent () {

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

}function MVVMFunction (action) {

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

}function MVVMLexicalAnalyzer (text, endType, unknownType, integerType, decimalType, stringType) {

	this.text = text;
	this.endType = endType;
	this.unknownType = unknownType;
	this.integerType = integerType;
	this.decimalType = decimalType;
	this.stringType = stringType;
	this.index = 0;
	this.rawIndex = 0;
	this.ignoreCase = false;
	this.allowString = true;
	this.allowNumber = true;
	this.allowIgnoreCharactersBreakKeyword = true;
	this.allowCharactersBreakKeyword = true;
	this.ignoreCharacters = [ ' ', '\t', '\r', '\n' ];
	this.breakKeywordCharacters = [ ' ', '\t', '\r', '\n' ];
	this.characters = [
		{ key: '{', value: MVVMTokenType.leftBrace },
		{ key: '}', value: MVVMTokenType.rightBrace },
		{ key: '=', value: MVVMTokenType.equalSign },
		{ key: ',', value: MVVMTokenType.comma }
	];
	this.blocks = [];
	this.symbols = [];
	this.stringStartCharacters = [ '\'', '"' ];
	this.tailCharacters = [];
	this.keywords = [];
	this._current = null;
	this.buffer = [];
	this.line = 1;
	this.lineIndex = 0;

	this.getCurrent = function () {
		if (this._current == null) {
			this.moveNext ();
		}
		return this._current;
	};

	this.addCharacter = function (character, type) {
		this.characters.push ({ key: character, value: type });
	};

	this.addTailCharacter = function (character, type) {
		this.tailCharacters.push ({ key: character, value: type });
	};

	this.addBlock = function (type, start, end) {
		this.blocks.push ({ type: type, start: start, end: end });
	};

	this.addSymbol = function (text, type, value) {
		this.symbols.push ({ key: text, type: type, value: value });
	};

	this.addKeyword = function (text, type, value) {
		this.keywords.push ({ key: text, type: type, value: value });
	};

	this.peek = function () {
		return this.buffer.length === 0 ? this.rawPeek () : this.buffer[0];
	};

	this.read = function () {
		var character;
		if (this.buffer.length === 0) {
			character = this.rawRead ();
		} else {
			character = this.buffer[0];
			this.buffer.splice (0, 1);
		}
		this.index++;
		if (character === '\n') {
			this.line++;
			this.lineIndex = 0;
		} else {
			this.lineIndex++;
		}
		return character;
	};

	this.peekChar = function () {
		return String.fromCharCode (this.peek ());
	};

	this.readChar = function () {
		return String.fromCharCode (this.read ());
	};

	this.rawPeek = function () {
		return this.rawIndex < this.text.length ? this.text[this.rawIndex].charCodeAt (0) : -1;
	};

	this.rawRead = function () {
		return this.text[this.rawIndex++].charCodeAt (0);
	};

	this.rawPeekChar = function () {
		return String.fromCharCode (this.rawPeek ());
	};

	this.rawReadChar = function () {
		return String.fromCharCode (this.rawRead ());
	};

	this.pushBuffer = function () {
		var character = this.rawRead ();
		this.buffer.push (character);
		return character;
	};

	this.clearBuffer = function () {
		while (this.buffer.length > 0) {
			this.read ();
		}
	};

	this.match = function (end, ignoreCase, clear) {
		if (this.peekChar () === end[0]) {
			if (end.length === 1) {
				if (clear) {
					this.read ();
				}
				return true;
			}
		} else {
			return false;
		}
		if (this.buffer.length === 0) {
			this.pushBuffer ();
		}
		for (var i = 1; i < end.length; i++) {
			if (this.buffer.length <= i) {
				this.pushBuffer ();
			}
			if (!this.equals (String.fromCharCode (this.buffer[i]), end[i], ignoreCase)) {
				return false;
			}
		}
		if (clear) {
			this.clearBuffer ();
		}
		return true;
	};

	this.moveNext = function () {
		this.skipIgnoreCharacters ();
		if (this.peek () === -1) {
			this._current = new MVVMToken (this.endType, null, this.index, 1, this.line, this.lineIndex);
			return false;
		}
		var startIndex = this.index;
		var line = this.line;
		var lineStartIndex = this.lineIndex;
		var text;
		for (var i = 0; i < this.blocks.length; i++) {
			if (this.match (this.blocks[i].start)) {
				text = this.readTo (this.blocks[i].end);
				this._current = new MVVMToken (this.blocks[i].type, text, startIndex, text.length, line, lineStartIndex);
				return true;
			}
		}
		for (var i = 0; i < this.symbols.length; i++) {
			if (this.match (this.symbols[i].key, this.ignoreCase, false)) {
				this._current = new MVVMToken (this.symbols[i].type, this.symbols[i].value, startIndex, this.symbols[i].key.length, line, lineStartIndex);
				return true;
			}
		}
		var character = this.peekChar ();
		var type = [];
		if (this.tryGetValue (this.characters, character, type)) {
			this._current = new MVVMToken (type[0], character, startIndex, 1, line, lineStartIndex);
			this.read ();
			return true;
		}
		if (this.allowString && this.contains (this.stringStartCharacters, character, this.ignoreCase)) {
			this.read ();
			var isClosed = [];
			text = this.readString (character, isClosed);
			this._current = new MVVMToken (this.stringType, text, startIndex, text.length + (isClosed[0] ? 2 : 1), line, lineStartIndex);
			return true;
		}
		if (this.allowNumber) {
			var parse = false;
			switch (character) {
				case '+':
				case '-':
				case '.':
					this.pushBuffer ();
					var temp = this.text[this.index];
					if ((character !== '.' && temp === '.') || this.isNumber (temp)) {
						parse = true;
					}
					break;
				default:
					if (this.isNumber (character)) {
						parse = true;
					}
					break;
			}
			if (parse) {
				var isDecimal = [];
				var isScientificNotation = [];
				text = this.readNumber (isDecimal, isScientificNotation);
				if (!isDecimal[0] || isScientificNotation[0]) {
					var longResult = parseInt (text);
					if (!isNaN (longResult)) {
						this._current = new MVVMToken (this.integerType, longResult, startIndex, text.length, line, lineStartIndex);
						return true;
					}
				}
				if (isDecimal[0] || isScientificNotation[0]) {
					var decimalResult = parseFloat (text);
					if (!isNaN (decimalResult)) {
						this._current = new MVVMToken (this.decimalType, decimalResult, startIndex, text.length, line, lineStartIndex);
						return true;
					}
				}
				this._current = new MVVMToken (this.unknownType, text, startIndex, text.length, line, lineStartIndex);
				return true;
			}
		}
		if (this.tryGetValue (this.tailCharacters, character, type)) {
			this._current = new MVVMToken (type[0], character, startIndex, 1, line, lineStartIndex);
			this.read ();
			return true;
		}
		text = this.readKeyword ();
		var keyword = [];
		if (this.tryGetValue (this.keywords, this.text, keyword)) {
			this._current = new MVVMToken (keyword[0].key, keyword[0].value, startIndex, text.length, line, lineStartIndex);
			return true;
		}
		this._current = new MVVMToken (this.unknownType, text, startIndex, text.length, line, lineStartIndex);
		return true;
	};

	this.readTo = function (end, ignoreCase, eatEnd) {
		var stringBuilder = new MVVMStringBuilder ();
		while (this.peek () > -1) {
			if (this.match (end, ignoreCase, eatEnd)) {
				break;
			}
			stringBuilder.append (this.readChar ());
		}
		return stringBuilder.toString ();
	};

	this.skipIgnoreCharacters = function () {
		while (this.peek () > -1) {
			if (this.contains (this.ignoreCharacters, this.peekChar (), this.ignoreCase)) {
				this.read ();
				continue;
			}
			break;
		}
		return this.peek ();
	};

	this.readString = function (end, isClosed) {
		var stringBuilder = new MVVMStringBuilder ();
		isClosed[0] = false;
		while (this.peek () > -1) {
			var character = this.peekChar ();
			if (character === end) {
				isClosed[0] = true;
				this.read ();
				break;
			}
			if (character === '\\') {
				stringBuilder.append (character);
				this.read ();
				if (this.peek () === -1) {
					break;
				}
				character = this.peekChar ();
			}
			stringBuilder.append (character);
			this.read ();
		}
		return stringBuilder.toString ();
	};

	this.readNumber = function (isDecimal, isScientificNotation) {
		var stringBuilder = new MVVMStringBuilder ();
		var startIndex = 1;
		isDecimal[0] = false;
		isScientificNotation[0] = false;
		var signIndex = 0;
		while (this.peek () > -1) {
			var character = this.peekChar ();
			var needBreak = false;
			switch (character) {
				case '+':
				case '-':
					if (stringBuilder.length !== signIndex) {
						needBreak = true;
					}
					startIndex = stringBuilder.length + 2;
					break;
				case '.':
					if (isDecimal[0] || isScientificNotation[0]) {
						needBreak = true;
					}
					isDecimal[0] = true;
					startIndex = stringBuilder.length + 2;
					break;
				case 'e':
				case 'E':
					if (isScientificNotation[0] || stringBuilder.length < startIndex) {
						needBreak = true;
					}
					isScientificNotation[0] = true;
					signIndex = stringBuilder.length + 1;
					break;
				default:
					if (!this.isNumber (character)) {
						needBreak = true;
					}
					break;
			}
			if (needBreak) {
				break;
			}
			stringBuilder.append (character);
			this.read ();
		}
		return stringBuilder.toString ();
	};

	this.readKeyword = function () {
		var stringBuilder = new MVVMStringBuilder ();
		while (this.peek () > -1) {
			var character = this.peekChar ();
			if (this.allowIgnoreCharactersBreakKeyword && this.contains (this.ignoreCharacters, character, this.ignoreCase)) {
				break;
			}
			if (this.allowCharactersBreakKeyword && this.containsKey (this.characters, character)) {
				break;
			}
			if (this.contains (this.breakKeywordCharacters, character, this.ignoreCase)) {
				break;
			}
			stringBuilder.append (character);
			this.read ();
		}
		return stringBuilder.toString ();
	};

	this.contains = function (characters, character, ignoreCase) {
		if (ignoreCase) {
			character = character.toUpperCase ();
		}
		for (var i = 0; i < characters.length; i++) {
			if (ignoreCase) {
				if (characters[i].toUpperCase () === character) {
					return true;
				}
			} else {
				if (characters[i] === character) {
					return true;
				}
			}
		}
		return false;
	};

	this.containsKey = function (dictionary, key) {
		for (var i = 0; i < dictionary.length; i++) {
			if (dictionary[i].key === key) {
				return true;
			}
		}
		return false;
	};

	this.tryGetValue = function (dictionary, key, value) {
		for (var i = 0; i < dictionary.length; i++) {
			if (dictionary[i].key === key) {
				value[0] = dictionary[i].value;
				return true;
			}
		}
		return false;
	};

	this.equals = function (characterA, characterB, ignoreCase) {
		if (ignoreCase) {
			return characterA.toUpperCase () === characterB.toUpperCase ();
		}
		return characterA === characterB;
	};

	this.isNumber = function (character) {
		switch (character) {
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return true;
		}
		return false;
	};

}function MVVMObservableClass (instance) {

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

}MVVMObservableCollection.prototype = [];

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

}function MVVMRelayCommand  (execute, canExecute) {

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

}function MVVMStringBuilder () {

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

}function MVVMToken (type, value, startIndex, length, line, lineStartIndex) {

	this.type = type;
	this.value = value;
	this.startIndex = startIndex;
	this.length = length;
	this.line = line;
	this.lineStartIndex = lineStartIndex;

}var MVVMTokenType = {

	end: 1,
	unknown: 2,
	leftBrace: 3,
	rightBrace: 4,
	equalSign: 5,
	comma: 6,
	integer: 7,
	decimal: 8,
	string: 9

};mvvm = new function () {

	var skipStart;
	var skipNumber;

	this.compile = function (node) {
		return this.forEach (node);
	};

	this.build = function (dataContext, node, root) {
		if (node == null) {
			node = document;
		}
		var control = this.compile (node);
		control.setDataContext (new MVVMValueBinding (dataContext));
		return control.build (root);
	};

	this.forEach = function (node, root, parent) {
		var i;
		switch (node.nodeType) {
			case 1:
				if (node.nodeName === "SCRIPT") {
					return root;
				}
				var control = parent == null ? new MVVMControl (node) : null;
				for (i = 0; i < node.attributes.length; i++) {
					if (mvvmStringStartsWith (node.attributes[i].name, "mvvm")) {
						var name = node.attributes[i].name.substring (4);
						switch (name) {
							case "itemtemplate":
								return root;
						}
						if (node.attributes[i].value.length < 0) {
							continue;
						}
						var binding = new MVVMClassConverter (node.attributes[i].value).parse ("binding", true);
						if (binding == null) {
							continue;
						}
						if (control == null) {
							control = new MVVMControl (node);
						}
						switch (name) {
							case "datacontext":
								control.setDataContext (binding);
								break;
							default:
								control.setBinding (binding, null, null, false, name);
								break;
						}
					}
				}
				if (control != null) {
					if (parent == null) {
						parent = control;
					} else {
						parent.add (control);
						parent = control;
					}
					if (root == null) {
						root = control;
					}
				}
				break;
			case 3:
				if (/{.*}/.test (node.nodeValue)) {
					var end = 0;
					new MVVMClassConverter (node.nodeValue).parseAll (function (start, length, binding) {
						var newNode;
						if (length > 0) {
							newNode = document.createTextNode (node.nodeValue.substr (start, length));
							node.parentNode.insertBefore (newNode, node);
						}
						newNode = document.createTextNode (null);
						node.parentNode.insertBefore (newNode, node);
						var control = new MVVMControl (newNode);
						control.setBinding (binding, null, null, false, "nodeValue");
						parent.add (control);
						end = this.lexicalAnalyzer.position;
					});
					if (end < node.nodeValue.length) {
						var newNode = document.createTextNode (node.nodeValue.substr (end, node.nodeValue.length - end));
						node.parentNode.insertBefore (newNode, node);
					}
					skipNumber = mvvmNodeIndexOf (node.parentNode, node) - skipStart - 1;
					node.parentNode.removeChild (node);
				}
				break;
		}
		for (i = 0; i < node.childNodes.length; i++) {
			skipNumber = 0;
			skipStart = i;
			root = this.forEach (node.childNodes[i], root, parent);
			i += skipNumber;
		}
		return root;
	}

};