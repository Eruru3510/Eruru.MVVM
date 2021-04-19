using System.ComponentModel;
using System.Windows.Forms;
using Eruru.MVVM;

namespace WindowsFormsApp1 {

	public class Player : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public string Name {

			get => _Name;

			set {
				_Name = value;
				this.RaisePropertyChanged ();
			}

		}
		public MVVMRelayCommand OnClick { get; set; } = new MVVMRelayCommand (value => {
			MessageBox.Show ($"你点击了{(value as Player)?.Name}");
		});

		string _Name = "Unnamed";

		public override string ToString () {
			return Name;
		}

	}

}