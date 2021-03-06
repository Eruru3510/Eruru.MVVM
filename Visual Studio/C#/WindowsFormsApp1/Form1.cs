using Eruru.MVVM;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	public partial class Form1 : Form {

		public Form1 () {
			InitializeComponent ();
			new MVVMControl (this) {
				DataContext = new MVVMBinding (new Form1ViewModel ())
			}.Add (
				new MVVMItemsControl (FlowLayoutPanel) {
					ItemsSource = new MVVMBinding ("Items"),
					ItemTemplate = new MVVMDataTemplate (() => new UserControlForm1Item ().Build ())
				},
				new MVVMButton (ButtonAdd) {
					Click = new MVVMBinding ("OnAdd")
				}
			).Build ();

		}

	}

}