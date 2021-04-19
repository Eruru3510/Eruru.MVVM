using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMImage : MVVMControl {

		public MVVMBinding Sprite {

			get {
				return GetBinding (ref _Sprite, value => Sprite = value);
			}

			set {
				InitializeBinding (ref _Sprite, value,
					targetValue => Control.sprite = targetValue as Sprite, () => Control.sprite
				);
			}

		}

		new Image Control { get; set; }
		MVVMBinding _Sprite;

		public MVVMImage (Image control) : base (control) {
			Control = control;
		}
		public MVVMImage (MVVMView view, Image control) : base (view, control) {
			Control = control;
		}

	}

}