using System.ComponentModel;
using Eruru.MVVM;

namespace WindowsFormsApp1 {

	public class Form1ViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public string Text { get; set; }
		public Player Player {

			get => _Player;

			set {
				_Player = value;
				this.RaisePropertyChanged ();
			}

		}
		public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player> ();
		public MVVMRelayCommand OnClickAdd { get; set; }
		public MVVMRelayCommand OnClickRemove { get; set; }

		Player _Player;

		public Form1ViewModel () {
			Text = "默认值";
			Players.Add (new Player () { Name = "玩家1" });
			Players.Add (new Player () { Name = "玩家2" });
			OnClickAdd = new MVVMRelayCommand (value => {
				Players.Add (new Player () { Name = value?.ToString () });
			});
			OnClickRemove = new MVVMRelayCommand (value => {
				Players.RemoveAt (0);
			});
		}

	}

}