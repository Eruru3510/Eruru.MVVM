using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMTrackBar : MVVMControl {

		public MVVMBinding Value {

			get => GetBinding (ref _Value, value => Value = value);

			set => InitializeBinding (ref _Value, value,
				propertyValue => Control.Value = MVVMApi.ToInt (propertyValue), () => Control.Value,
				() => Control.ValueChanged += Control_ValueChanged, () => Control.ValueChanged -= Control_ValueChanged,
				() => Control.LostFocus += Control_LostFocus, () => Control.LostFocus -= Control_LostFocus
			);

		}
		public MVVMBinding Maximum {

			get => GetBinding (ref _Maximum, value => Maximum = value);

			set => InitializeBinding (ref _Maximum, value, propertyValue => Control.Maximum = MVVMApi.ToInt (propertyValue), () => Control.Maximum);

		}
		public MVVMBinding Minimum {

			get => GetBinding (ref _Minimum, value => Minimum = value);

			set => InitializeBinding (ref _Minimum, value, propertyValue => Control.Minimum = MVVMApi.ToInt (propertyValue), () => Control.Minimum);

		}

		new readonly TrackBar Control;

		MVVMBinding _Value;
		MVVMBinding _Maximum;
		MVVMBinding _Minimum;

		public MVVMTrackBar (TrackBar control) : base (control) {
			Control = control;
		}
		public MVVMTrackBar (MVVMView view, TrackBar control) : base (view, control) {
			Control = control;
		}

		private void Control_ValueChanged (object sender, EventArgs e) {
			Changed (Value, Control.Value);
		}

		private void Control_LostFocus (object sender, EventArgs e) {
			Changed (Value, Control.Value, MVVMBindingChangedType.LostFocus);
		}

	}

}