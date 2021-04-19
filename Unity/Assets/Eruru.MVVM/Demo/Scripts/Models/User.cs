using System.ComponentModel;

namespace Eruru.MVVM.Demo {

	public class User : INotifyPropertyChanged {

		public string Account {

			get {
				return _Account;
			}

			set {
				_Account = value;
				this.RaisePropertyChanged ();
			}

		}
		public string Password {

			get {
				return _Password;
			}

			set {
				_Password = value;
				this.RaisePropertyChanged ();
			}

		}
		public event PropertyChangedEventHandler PropertyChanged;

		string _Account;
		string _Password;

	}

}