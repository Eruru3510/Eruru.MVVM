using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMCheckBox : MVVMControl {

		public CheckBox CheckBox { get; }
		public MVVMBinding Checked {

			get {
				return GetBinding (ref _Checked, binding => Checked = binding);
			}

			set {
				SetBinding (ref _Checked, value, () => CheckBox.Checked, targetValue => CheckBox.Checked = MVVMAPI.To<bool> (targetValue), true);
			}

		}

		MVVMBinding _Checked;

		public MVVMCheckBox (CheckBox checkBox) : base (checkBox) {
			CheckBox = checkBox;
			CheckBox.CheckedChanged += (sender, e) => OnChanged (_Checked);
		}

		protected override void OnLostFocus (object sender, EventArgs e) {
			OnChanged (_Checked, MVVMOnChangedType.LostFocus);
		}

	}

}