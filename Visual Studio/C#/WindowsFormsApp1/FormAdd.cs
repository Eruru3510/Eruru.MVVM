using Eruru.MVVM;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	public partial class FormAdd : Form, IMVVMView {

		readonly Item Item;

		public FormAdd (Item item = null) {
			InitializeComponent ();
			Item = item ?? new Item ();
			ButtonConfirm.Click += (sender, e) => {
				DialogResult = DialogResult.OK;
				Close ();
			};
			ButtonCancel.Click += (sender, e) => {
				DialogResult = DialogResult.Cancel;
				Close ();
			};
		}

		public MVVMControlBase Build () {
			return new MVVMControl (this) {
				DataContext = new MVVMBinding (new FormAddViewModel (Item))
			}.Add (
				new MVVMControl (this) {
					DataContext = new MVVMBinding ("Item")
				}.Add (
					new MVVMTextBox (TextBoxName) {
						Text = new MVVMBinding ("Name")
					},
					new MVVMTextBox (TextBoxAge) {
						Text = new MVVMBinding (path: "Age", validatesOnExceptions: true, updateSourceTrigger: MVVMUpdateSourceTrigger.PropertyChanged)
					}.AddTrigger (
						new MVVMTrigger ("Validation.HasError", true).Add (
							new MVVMSetter ("ForeColor", Color.Red),
							new MVVMSetter (new MVVMBinding ("LabelError", "Text"), new MVVMBinding (new MVVMRelativeSource (), "Validation.Errors.CurrentItem.ErrorContent"))
						)
					),
					new MVVMComboBox (ComboBoxSchool) {
						ItemsSource = new MVVMBinding (new MVVMRelativeSource (3), "DataContext.Schools"),
						SelectedItem = new MVVMBinding ("School")
					},
					new MVVMTextBox (TextBoxRemark) {
						Text = new MVVMBinding ("Remark")
					}
				),
				new MVVMLabel (LabelError) {
					Name = "LabelError"
				}
			).Build ();
		}

	}

}