using Eruru.MVVM;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	public partial class FormAdd : Form {

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

		public MVVMControl Build () {
			return new MVVMControl (this) {
				DataContext = new MVVMBinding (new FormAddViewModel () { Item = Item })
			}.Add (
			   new MVVMControl (this) {
				   DataContext = new MVVMBinding ("Item")
			   }.Add (
				   new MVVMTextBox (TextBoxName) {
					   Text = new MVVMBinding ("Name")
				   },
				   new MVVMTextBox (TextBoxAge) {
					   Text = new MVVMBinding ("Age")
				   },
				   new MVVMComboBox (ComboBoxSchool) {
					   ItemsSource = new MVVMBinding (new MVVMBindingRelativeSource (3), "DataContext.Schools"),
					   Text = new MVVMBinding ("School")
				   },
				   new MVVMTextBox (TextBoxRemark) {
					   Text = new MVVMBinding ("Remark")
				   }
			   )
		   ).Build ();
		}

	}

}