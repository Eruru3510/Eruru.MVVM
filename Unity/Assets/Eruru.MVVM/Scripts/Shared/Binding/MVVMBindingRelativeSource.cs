using System;

namespace Eruru.MVVM {

	public class MVVMBindingRelativeSource {

		public MVVMRelativeSourceMode Mode {

			get {
				return _Mode;
			}

		}
		public Type AncestorType {

			get {
				return _AncestorType;
			}

		}
		public int AncestorLevel {

			get {
				return _AncestorLevel;
			}

		}

		readonly MVVMRelativeSourceMode _Mode;
		readonly int _AncestorLevel;
		readonly Type _AncestorType;

		public MVVMBindingRelativeSource () {
			_Mode = MVVMRelativeSourceMode.Self;
		}
		public MVVMBindingRelativeSource (Type ancestorType) : this (ancestorType, 1) {

		}
		public MVVMBindingRelativeSource (int ancestorLevel) : this (null, ancestorLevel) {

		}
		public MVVMBindingRelativeSource (Type ancestorType, int ancestorLevel) {
			_AncestorType = ancestorType;
			_AncestorLevel = ancestorLevel;
			_Mode = MVVMRelativeSourceMode.FindAncestor;
		}

	}

}