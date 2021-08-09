using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMTrackBar : MVVMControl {

		public new TrackBar Control { get; }
		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, binding => Value = binding);
			}

			set {
				SetBinding (ref _Value, value, () => Control.Value, targetValue => Control.Value = MVVMAPI.To<int> (targetValue), defaultMode: MVVMBindingMode.TwoWay);
			}

		}
		public MVVMBinding Maximum {

			get {
				return GetBinding (ref _Maximum, binding => Maximum = binding);
			}

			set {
				SetBinding (ref _Maximum, value, () => Control.Maximum, targetValue => Control.Maximum = MVVMAPI.To<int> (targetValue));
			}

		}
		public MVVMBinding Minimum {

			get {
				return GetBinding (ref _Minimum, binding => Minimum = binding);
			}

			set {
				SetBinding (ref _Minimum, value, () => Control.Minimum, targetValue => Control.Minimum = MVVMAPI.To<int> (targetValue));
			}

		}

		MVVMBinding _Value;
		MVVMBinding _Maximum;
		MVVMBinding _Minimum;

		public MVVMTrackBar (TrackBar control) : base (control) {
			Control = control;
			Control.ValueChanged += Control_ValueChanged;
			Control.LostFocus += Control_LostFocus;
		}

		private void Control_ValueChanged (object sender, EventArgs e) {
			OnChanged (_Value);
		}

		private void Control_LostFocus (object sender, EventArgs e) {
			OnChanged (_Value, MVVMOnChangedType.LostFocus);
		}

	}

}