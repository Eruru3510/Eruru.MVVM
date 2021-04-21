using System;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMControl {

		public MVVMBinding Enabled {

			get => GetBinding (ref _Enabled, value => Enabled = value);

			set => InitializeBinding (ref _Enabled, value,
				propertyValue => Control.Enabled = MVVMApi.ToBool (propertyValue), () => Control.Enabled,
				() => Control.EnabledChanged += Control_EnabledChanged, () => Control.EnabledChanged -= Control_EnabledChanged
			);

		}
		public MVVMBinding Visible {

			get => GetBinding (ref _Visible, value => Visible = value);

			set => InitializeBinding (ref _Visible, value,
				propertyValue => Control.Visible = MVVMApi.ToBool (propertyValue), () => Control.Visible,
				() => Control.VisibleChanged += Control_VisibleChanged, () => Control.VisibleChanged -= Control_VisibleChanged
			);

		}
		public MVVMBinding Text {

			get => GetBinding (ref _Text, value => Text = value);

			set => InitializeBinding (ref _Text, value,
				propertyValue => Control.Text = MVVMApi.ToString (propertyValue), () => Control.Text,
				() => Control.TextChanged += Control_TextChanged, () => Control.TextChanged -= Control_TextChanged,
				() => Control.LostFocus += Control_LostFocus, () => Control.LostFocus -= Control_LostFocus
			);

		}
		public MVVMBinding Click {

			get => GetBinding (ref _Click, value => Click = value);

			set => InitializeBinding (ref _Click, value, null, null, () => Control.Click += Control_Click, () => Control.Click -= Control_Click);

		}
		public MVVMBinding ClickParameter {

			get => GetBinding (ref _ClickParameter, value => ClickParameter = value);

			set => InitializeBinding (ref _ClickParameter, value);

		}

		protected readonly Control Control;

		MVVMBinding _Enabled;
		MVVMBinding _Visible;
		MVVMBinding _Text;
		MVVMBinding _Click;
		MVVMBinding _ClickParameter;

		public MVVMControl (Control control) {
			Control = control ?? throw new ArgumentNullException (nameof (control));
		}
		public MVVMControl (MVVMView view, Control control) : this (control) {
			View = view ?? throw new ArgumentNullException (nameof (view));
			View.Add (this);
		}

		private void Control_TextChanged (object sender, EventArgs e) {
			Changed (Text, Control.Text);
		}

		private void Control_LostFocus (object sender, EventArgs e) {
			Changed (Text, Control.Text, MVVMBindingChangedType.LostFocus);
		}

		private void Control_VisibleChanged (object sender, EventArgs e) {
			Changed (Visible, Control.Visible);
		}

		private void Control_EnabledChanged (object sender, EventArgs e) {
			Changed (Enabled, Control.Enabled);
		}

		private void Control_Click (object sender, EventArgs e) {
			Command (Click, ClickParameter);
		}

		public static implicit operator Control (MVVMControl control) {
			if (control is null) {
				throw new ArgumentNullException (nameof (control));
			}
			return control.Control;
		}

	}

}