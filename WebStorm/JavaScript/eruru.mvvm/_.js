mvvm = new function () {

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