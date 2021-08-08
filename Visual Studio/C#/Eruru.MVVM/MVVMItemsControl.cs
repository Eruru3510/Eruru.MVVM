using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Eruru.MVVM {

	public partial class MVVMItemsControl : MVVMControl {

		List<MVVMControl> Items = new List<MVVMControl> ();

		public MVVMItemsControl (Control control) : base (control) {

		}

		MVVMControl Register (int index, object value) {
			MVVMControl control = (MVVMControl)Convert (value);
			control._Parent = this;
			control.DataContext = new MVVMBinding (value);
			control.Build (Root);
			Items.Insert (index, control);
			Control.Controls.Add (control.Control);
			Control.Controls.SetChildIndex (control.Control, index);
			return control;
		}

		protected virtual void Insert (int index, object value) {
			Register (index, value);
		}

		protected virtual void InsertRange (int index, IEnumerable collection) {
			int i = index;
			foreach (object value in collection) {
				Insert (i, value);
				i++;
			}
		}

		protected virtual void Add (object value) {
			Insert (Items.Count, value);
		}

		protected virtual void AddRange (IEnumerable collection) {
			foreach (object value in collection) {
				Add (value);
			}
		}

		protected new virtual void RemoveAt (int index) {
			Items[index].Unbind ();
			Items.RemoveAt (index);
			Control.Controls.RemoveAt (index);
		}

		protected virtual void Replace (int index, object value) {
			RemoveAt (index);
			Insert (index, value);
		}

		protected new virtual void Move (int oldIndex, int newIndex) {
			MVVMControl control = Items[oldIndex];
			Items.RemoveAt (oldIndex);
			Items.Insert (newIndex, control);
			Control.Controls.SetChildIndex (Control.Controls[oldIndex], newIndex);
		}

		protected virtual void Reset () {
			foreach (MVVMControl control in Items) {
				control.Unbind ();
			}
			Items.Clear ();
			Control.Controls.Clear ();
		}

	}

}