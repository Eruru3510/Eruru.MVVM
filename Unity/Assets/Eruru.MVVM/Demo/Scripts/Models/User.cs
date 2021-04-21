namespace Eruru.MVVM.Demo {

	public class User {

		public string Account {

			get {
				return _Account;
			}

			set {
				_Account = value;
			}

		}
		public string Password {

			get {
				return _Password;
			}

			set {
				_Password = value;
			}

		}

		string _Account;
		string _Password;

	}

}