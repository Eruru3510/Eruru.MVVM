using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Eruru.MVVM {

	public class MVVMDropdown : MVVMItemsControlBase {

		new Dropdown Control { get; set; }

		public MVVMDropdown (Dropdown control) : base (control) {
			Control = control;
			Initialize ();
		}
		public MVVMDropdown (MVVMView view, Dropdown control) : base (view, control) {
			Control = control;
			Initialize ();
		}

		void Initialize () {
			Clear ();
		}

		protected override object ToItem (object value) {
			if (value is Dropdown.OptionData) {
				return (Dropdown.OptionData)value;
			}
			if (value is Sprite) {
				return new Dropdown.OptionData ((Sprite)value);
			}
			return new Dropdown.OptionData (value == null ? null : value.ToString ());
		}

		protected override void Add (object value) {
			Control.options.Add ((Dropdown.OptionData)value);
		}

		protected override void RemoveAt (int index) {
			Control.options.RemoveAt (index);
		}

		protected override void Replace (int index, object value) {
			Control.options[index] = (Dropdown.OptionData)value;
		}

		protected override void Move (int oldIndex, int newIndex) {
			Control.options.Swap (oldIndex, newIndex);
		}

		protected override void Clear () {
			Control.options.Clear ();
		}

	}

}