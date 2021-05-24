using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMRawImage : MVVMControl {

		public new RawImage Control { get; private set; }
		public MVVMBinding Texture {

			get {
				return GetBinding (ref _Texture, value => Texture = value);
			}

			set {
				SetBinding (ref _Texture, value, () => Control.texture, targetValue => Control.texture = targetValue as Texture);
			}

		}

		MVVMBinding _Texture;

		public MVVMRawImage (RawImage control) : base (control) {
			Control = control;
		}

	}

}