using System;

namespace Eruru.MVVM {

	public class MVVMBindingRelativeSource {

		public MVVMBindingRelativeSourceMode Mode { get; }
		public Type AncestorType { get; }
		public int AncestorLevel { get; }

		public MVVMBindingRelativeSource () {
			Mode = MVVMBindingRelativeSourceMode.Self;
		}
		public MVVMBindingRelativeSource (Type ancestorType) : this (ancestorType, 1) {

		}
		public MVVMBindingRelativeSource (Type ancestorType, int ancestorLevel) {
			AncestorType = ancestorType;
			AncestorLevel = ancestorLevel;
			Mode = MVVMBindingRelativeSourceMode.FindAncestor;
		}

	}

}