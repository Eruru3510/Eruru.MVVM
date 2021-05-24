using System;

namespace Eruru.MVVM {

	public class MVVMBindingRelativeSource {

		public MVVMBindingRelativeSourceMode Mode { get; set; }
		public Type AncestorType { get; set; }
		public int AncestorLevel { get; set; }

		public MVVMBindingRelativeSource () {
			Mode = MVVMBindingRelativeSourceMode.Self;
		}
		public MVVMBindingRelativeSource (Type ancestorType) {
			AncestorType = ancestorType;
			Mode = MVVMBindingRelativeSourceMode.FindAncestor;
			AncestorLevel = 1;
		}
		public MVVMBindingRelativeSource (Type ancestorType, int ancestorLevel) {
			AncestorType = ancestorType;
			AncestorLevel = ancestorLevel;
			Mode = MVVMBindingRelativeSourceMode.FindAncestor;
		}

	}

}