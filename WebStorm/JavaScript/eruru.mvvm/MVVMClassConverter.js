function MVVMClassConverter (text) {

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

}