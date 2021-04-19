using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMRawImage : MVVMControl {

		public MVVMBinding Texture {

			get {
				return GetBinding (ref _Texture, value => Texture = value);
			}

			set {
				InitializeBinding (ref _Texture, value,
					targetValue => Control.texture = targetValue as Texture, () => Control.texture
				);
			}

		}

		new RawImage Control { get; set; }
		MVVMBinding _Texture;

		public MVVMRawImage (RawImage control) : base (control) {
			Control = control;
		}
		public MVVMRawImage (MVVMView view, RawImage control) : base (view, control) {
			Control = control;
		}

	}

}