function MVVMLexicalAnalyzer (text, endType, unknownType, integerType, decimalType, stringType) {

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

}