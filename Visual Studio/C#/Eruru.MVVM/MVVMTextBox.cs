using System;
using System.Drawing;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMTextBox : MVVMControl {

		public TextBox TextBox { get; }

		public MVVMTextBox (TextBox textBox) : base (textBox) {
			TextBox = textBox;
		}

		protected override void OnSetText (MVVMBinding text) {
			text.DefaultMode = MVVMBindingMode.TwoWay;
		}

	}

}