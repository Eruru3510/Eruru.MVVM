using Eruru.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	public partial class UserControlForm1Item : UserControl {

		public UserControlForm1Item () {
			InitializeComponent ();
			Anchor = AnchorStyles.Left | AnchorStyles.Right;
		}

		public MVVMControl Build () {
			return new MVVMControl (this).Add (
				new MVVMLabel (LabelName) {
					Text = new MVVMBinding ("Name")
				},
				new MVVMLabel (LabelAge) {
					Text = new MVVMBinding ("Age")
				},
				new MVVMLabel (LabelSchool) {
					Text = new MVVMBinding ("School")
				},
				new MVVMLabel (LabelRemark) {
					Text = new MVVMBinding ("Remark")
				},
				new MVVMButton (ButtonEdit) {
					Click = new MVVMBinding (new MVVMBindingRelativeSource (typeof (MVVMItemsControl)), "DataContext.OnEdit"),
					ClickParameter = new MVVMBinding ()
				},
				new MVVMButton (ButtonDelete) {
					Click = new MVVMBinding (new MVVMBindingRelativeSource (typeof (MVVMItemsControl)), "DataContext.OnDelete"),
					ClickParameter = new MVVMBinding ()
				}
			);
		}

	}

}