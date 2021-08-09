using UnityEngine;
using UnityEngine.EventSystems;

namespace Eruru.MVVM {

	public partial class MVVMControl {

		public Behaviour Control { get; private set; }

		public MVVMControl (Behaviour control) {
			Control = control;
		}

	}

}