using System;

namespace Eruru.MVVM {

	public class MVVMRelativeSource {

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

		public MVVMRelativeSource () {
			_Mode = MVVMRelativeSourceMode.Self;
		}
		public MVVMRelativeSource (Type ancestorType) : this (ancestorType, 1) {

		}
		public MVVMRelativeSource (int ancestorLevel) : this (null, ancestorLevel) {

		}
		public MVVMRelativeSource (Type ancestorType, int ancestorLevel) {
			_AncestorType = ancestorType;
			_AncestorLevel = ancestorLevel;
			_Mode = MVVMRelativeSourceMode.FindAncestor;
		}

	}

}