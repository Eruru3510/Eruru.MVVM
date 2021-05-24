using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMImage : MVVMControl {

		public new Image Control { get; private set; }
		public MVVMBinding Sprite {

			get {
				return GetBinding (ref _Sprite, value => Sprite = value);
			}

			set {
				SetBinding (ref _Sprite, value, () => Control.sprite, targetValue => Control.sprite = targetValue as Sprite);
			}

		}

		MVVMBinding _Sprite;

		public MVVMImage (Image control) : base (control) {
			Control = control;
		}

	}

}