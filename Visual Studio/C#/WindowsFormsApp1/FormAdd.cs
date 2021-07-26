using Eruru.MVVM;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	public partial class FormAdd : Form {

		public FormAdd () {
			InitializeComponent ();
			MVVMControl control = new MVVMControl (this) {
				DataContext = new MVVMBinding (new FormAddViewModel ())
			}.Add (
				new MVVMControl (this) {
					DataContext = new MVVMBinding ("Model")
				}.Add (
					new MVVMLabel (Label) {
						Text = new MVVMBinding ("TextBox", "Text")
					},
					new MVVMTextBox (TextBox) {
						Name = "TextBox",
						Text = new MVVMBinding ("Text")
					},
					new MVVMComboBox (ComboBox) {
						ItemsSource = new MVVMBinding ("Names"),
						SelectedIndex = new MVVMBinding ("TextBox", "Text")
					},
					new MVVMItemsControl (FlowLayoutPanel) {
						ItemsSource = new MVVMBinding ("Names"),
					}
				),
				new MVVMButton (button1) {
					Click = new MVVMBinding ("OnAdd")
				},
				new MVVMButton (ButtonConfirm) {
					Click = new MVVMBinding ("OnConfirm"),
					ClickParameter = new MVVMBinding ()
				},
				new MVVMButton (ButtonCancel) {
					Click = new MVVMBinding ("OnCancel")
				}
			).Build ();
			ButtonCancel.Click += (sender, e) => {
				control.DataContext.SetTargetValue (new FormAddViewModel ());
			};
		}

	}

}