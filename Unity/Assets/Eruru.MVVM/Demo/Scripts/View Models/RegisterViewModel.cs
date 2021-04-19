using System.ComponentModel;
using UnityEngine.UI;

namespace Eruru.MVVM.Demo {

	public class RegisterViewModel : INotifyPropertyChanged {

		public User User {

			get {
				return _UserModel;
			}

			set {
				_UserModel = value;
				this.RaisePropertyChanged ();
			}

		}
		public ObservableCollection<Dropdown.OptionData> Options { get; set; }
		public MVVMView ItemTemplate { get; set; }
		public ObservableCollection<string> Items { get; set; }
		public MVVMRelayCommand ResetCommand { get; set; }
		public string Text { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		User _UserModel = new User () {
			Account = "默认值"
		};

		public RegisterViewModel () {
			Text = "默认值";
			Options = new ObservableCollection<Dropdown.OptionData> () {
				new Dropdown.OptionData ("A", null),
				new Dropdown.OptionData ("B", null),
				new Dropdown.OptionData ("C", null)
			};
			Items = new ObservableCollection<string> () {
				"A",
				"B",
				"C"
			};
			ResetCommand = new MVVMRelayCommand (value => {
				User.Account = string.Empty;
				User.Password = string.Empty;
			});
		}

	}

}