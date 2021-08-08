function MVVMValueBinding (value) {
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
				throw "未实现" + mvvmGetEnumName (MVVMBindingType, this.type);
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

}