using System.ComponentModel;
using System.Windows;

namespace WpfApp1 {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow () {
			InitializeComponent ();
			DataContext = new ViewModel () { User = new User () { Name = "1" } };
			(DataContext as ViewModel).User = new User () { Name = "2" };
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

		User _User;

		public event PropertyChangedEventHandler PropertyChanged;

	}

	class User : INotifyPropertyChanged {

		public string Name {

			get => _Name;

			set {
				_Name = value;
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (User)));
			}

		}

		string _Name;

		public event PropertyChangedEventHandler PropertyChanged;
	}

}
