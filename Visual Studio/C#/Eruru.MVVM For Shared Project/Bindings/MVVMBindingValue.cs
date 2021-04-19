namespace Eruru.MVVM {

	public class MVVMBindingValue : MVVMBinding {

		public MVVMBindingValue () {

		}
		public MVVMBindingValue (object value) {
			Value = value;
		}
		public MVVMBindingValue (object value, MVVMBindingMode mode) : base (mode) {
			Value = value;
		}

		public override object GetValue () {
			return Value;
		}

		public override void SetValue (object value) {
			Value = value;
		}

	}

}