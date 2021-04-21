using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApp1 {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow () {
			InitializeComponent ();
			DataContext = new ViewModel ();
		}
	}

	class ViewModel : INotifyPropertyChanged {

		public User User {

			get => _User;

			set {
				_User = value;
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (User)));
			}

		}

		User _User = new User ();

		public event PropertyChangedEventHandler PropertyChanged;

	}

	class User : INotifyPropertyChanged {

		public string Name {

			get => _Name;

			set {
				_Name = value;
				Console.WriteLine ($"Set {nameof (Name)} = {value}");
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (User)));
			}

		}

		string _Name = "1";

		public event PropertyChangedEventHandler PropertyChanged;
	}

}