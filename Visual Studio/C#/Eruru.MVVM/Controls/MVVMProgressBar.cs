using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMProgressBar : MVVMControl {

		public MVVMBinding Value {

			get => GetBinding (ref _Value, value => Value = value);

			set => InitializeBinding (ref _Value, value, propertyValue => Control.Value = MVVMApi.ToInt (propertyValue), () => Control.Value);

		}
		public MVVMBinding Maximum {

			get => GetBinding (ref _Maximum, value => Maximum = value);

			set => InitializeBinding (ref _Maximum, value, propertyValue => Control.Maximum = MVVMApi.ToInt (propertyValue), () => Control.Maximum);

		}
		public MVVMBinding Minimum {

			get => GetBinding (ref _Minimum, value => Minimum = value);

			set => InitializeBinding (ref _Minimum, value, propertyValue => Control.Minimum = MVVMApi.ToInt (propertyValue), () => Control.Minimum);

		}

		new readonly ProgressBar Control;

		MVVMBinding _Value;
		MVVMBinding _Maximum;
		MVVMBinding _Minimum;

		public MVVMProgressBar (ProgressBar control) : base (control) {
			Control = control;
		}
		public MVVMProgressBar (MVVMView view, ProgressBar control) : base (view, control) {
			Control = control;
		}

	}

}