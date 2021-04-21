using UnityEngine;

namespace Eruru.MVVM.Demo {

	class ViewModel {

		public User User {

			get {
				return _User;
			}

			set {
				_User = value;
			}

		}
		public MVVMRelayCommand OnClick { get; set; }

		User _User = new User ();

		public ViewModel () {
			OnClick = new MVVMRelayCommand (value => Debug.Log (User.Account));
		}

	}

}