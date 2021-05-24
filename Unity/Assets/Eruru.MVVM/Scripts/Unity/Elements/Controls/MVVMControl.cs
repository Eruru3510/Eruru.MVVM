using UnityEngine;

namespace Eruru.MVVM {

	public partial class MVVMControl {

		public string Name {

			get {
				return Control.name;
			}

			set {
				Control.name = value;
			}

		}

		public MonoBehaviour Control { get; private set; }

	}

}