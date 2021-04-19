using System;
using UnityEngine;

namespace Eruru.MVVM {

	partial class MVVMControl {

		public MVVMBinding Enabled {

			get {
				return GetBinding (ref _Enabled, value => Enabled = value);
			}

			set {
				InitializeBinding (ref _Enabled, value, targetValue => Control.enabled = MVVMApi.ToBool (targetValue), () => Control.enabled);
			}

		}
		public MVVMBinding ActiveSelf {

			get {
				return GetBinding (ref _ActiveSelf, value => ActiveSelf = value);
			}

			set {
				InitializeBinding (ref _ActiveSelf, value,
					targetValue => Control.gameObject.SetActive (MVVMApi.ToBool (targetValue)), () => Control.gameObject.activeSelf
				);
			}

		}

		internal readonly MonoBehaviour Control;

		MVVMBinding _Enabled;
		MVVMBinding _ActiveSelf;

		public MVVMControl (MonoBehaviour control) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			Control = control;
		}
		public MVVMControl (MVVMView view, MonoBehaviour control) : this (control) {
			if (view == null) {
				throw new ArgumentNullException ("view");
			}
			View = view;
			view.Add (this);
		}

	}

}