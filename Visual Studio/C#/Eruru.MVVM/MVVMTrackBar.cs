using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMTrackBar : MVVMControl {

		public TrackBar TrackBar { get; }
		public MVVMBinding Value {

			get {
				return GetBinding (ref _Value, binding => Value = binding);
			}

			set {
				SetBinding (ref _Value, value, () => TrackBar.Value, targetValue => TrackBar.Value = MVVMAPI.To<int> (targetValue), true);
			}

		}
		public MVVMBinding Maximum {

			get {
				return GetBinding (ref _Maximum, binding => Maximum = binding);
			}

			set {
				SetBinding (ref _Maximum, value, () => TrackBar.Maximum, targetValue => TrackBar.Maximum = MVVMAPI.To<int> (targetValue));
			}

		}
		public MVVMBinding Minimum {

			get {
				return GetBinding (ref _Minimum, binding => Minimum = binding);
			}

			set {
				SetBinding (ref _Minimum, value, () => TrackBar.Minimum, targetValue => TrackBar.Minimum = MVVMAPI.To<int> (targetValue));
			}

		}

		MVVMBinding _Value;
		MVVMBinding _Maximum;
		MVVMBinding _Minimum;

		public MVVMTrackBar (TrackBar trackBar) : base (trackBar) {
			TrackBar = trackBar;
			TrackBar.ValueChanged += (sender, e) => OnChanged (_Value);
		}

		protected override void OnLostFocus (object sender, EventArgs e) {
			OnChanged (_Value, MVVMOnChangedType.LostFocus);
		}

	}

}