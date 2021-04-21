using System.Windows.Forms;
using Eruru.MVVM;

namespace WindowsFormsApp1 {

	public partial class Form1 : MVVMView {

		readonly Form1ViewModel Form1ViewModel = new Form1ViewModel ();

		public Form1 () {
			InitializeComponent ();
			DataContext = Form1ViewModel;
			/*
			new MVVMControl (this, textBox1) { Text = new MVVMBindingSource ("Player.Name") };
			new MVVMControl (this, textBox2) { Text = new MVVMBindingSource ("Player.Name") };
			Form1ViewModel.Player = new Player () { Name = "Named" };
			return;*/
			MVVMControl mvvmTextBox1 = new MVVMControl (this, textBox1);
			MVVMControl mvvmTextBox2 = new MVVMControl (this, textBox2) { Text = new MVVMBinding (mvvmTextBox1, "Text") };
			new MVVMItemsControl (this, flowLayoutPanel1) {
				DataTemplate = value => {
					Label label = new Label () {
						BorderStyle = BorderStyle.FixedSingle
					};
					return new MVVMControl (label) {
						Text = new MVVMBinding ("Name"),
						Click = new MVVMBinding ("OnClick"),
						ClickParameter = new MVVMBinding ()
					};
				},
				ItemsSource = new MVVMBinding ("Players")
			};
			new MVVMComboBox (this, comboBox1) {
				ItemsSource = new MVVMBinding ("Players")
			};
			new MVVMControl (this, button1) {
				Click = new MVVMBinding ("OnClickAdd"),
				ClickParameter = new MVVMBinding (mvvmTextBox1, nameof (MVVMControl.Text))
			};
			new MVVMControl (this, button2) { Click = new MVVMBinding ("OnClickRemove") };
		}

	}

}