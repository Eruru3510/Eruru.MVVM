using System.Drawing;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public class MVVMPictureBox : MVVMControl {

		public MVVMBinding Image {

			get => GetBinding (ref _Image, value => Image = value);

			set => InitializeBinding (ref _Image, value, propertyValue => Control.Image = propertyValue as Image, () => Control.Image);

		}

		new readonly PictureBox Control;

		MVVMBinding _Image;

		public MVVMPictureBox (PictureBox control) : base (control) {
			Control = control;
		}
		public MVVMPictureBox (MVVMView view, PictureBox control) : base (view, control) {
			Control = control;
		}

	}

}