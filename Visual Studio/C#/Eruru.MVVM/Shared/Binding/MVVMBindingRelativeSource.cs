using System;

namespace Eruru.MVVM {

	public class MVVMBindingRelativeSource {

		public MVVMRelativeSourceMode Mode { get; }
		public Type AncestorType { get; }
		public int AncestorLevel { get; }

		public MVVMBindingRelativeSource () {
			Mode = MVVMRelativeSourceMode.Self;
		}
		public MVVMBindingRelativeSource (Type ancestorType) : this (ancestorType, 1) {

		}
		public MVVMBindingRelativeSource (int ancestorLevel) : this (null, ancestorLevel) {

		}
		public MVVMBindingRelativeSource (Type ancestorType, int ancestorLevel) {
			AncestorType = ancestorType;
			AncestorLevel = ancestorLevel;
			Mode = MVVMRelativeSourceMode.FindAncestor;
		}

	}

}