using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp1 {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow () {
			InitializeComponent ();
			DataContext = new ViewModel ();
		}

		private void Button_Click (object sender, RoutedEventArgs e) {

		}
	}

	class ViewModel : INotifyPropertyChanged {

		public string Text {

			get {
				Console.WriteLine ("Get");
				return _Text;
			}

			set {
				Console.WriteLine ("Set");
				_Text = value;
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Text)));
			}

		}

		string _Text = "默认值";

		public event PropertyChangedEventHandler PropertyChanged;

	}

}