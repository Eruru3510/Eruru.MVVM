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
}