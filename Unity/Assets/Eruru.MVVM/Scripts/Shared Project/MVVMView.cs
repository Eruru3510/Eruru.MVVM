using System;
using System.Collections.Generic;

namespace Eruru.MVVM {

	public partial class MVVMView {

		public object DataContext {

			get {
				return _DataContext;
			}

			set {
				_DataContext = value;
				foreach (MVVMControl control in MVVMControls) {
					control.DataContext.Rebinding (value);
				}
			}

		}
		public List<MVVMControl> MVVMControls {

			get {
				return _MVVMControls;
			}

		}

		readonly List<MVVMControl> _MVVMControls = new List<MVVMControl> ();

		object _DataContext;

		public void Add (MVVMControl control) {
			if (control == null) {
				throw new ArgumentNullException ("control");
			}
			MVVMControls.Add (control);
		}

	}

}