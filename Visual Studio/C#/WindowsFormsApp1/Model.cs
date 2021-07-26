using Eruru.MVVM;
using System;
using System.ComponentModel;

namespace WindowsFormsApp1 {

	class Model : INotifyPropertyChanged {

		public string Text {

			get {
				Console.WriteLine ($"Get {this}.{nameof (Text)} {_Text}");
				return _Text;
			}

			set {
				_Text = value;
				Console.WriteLine ($"Set {this}.{nameof (Text)} {value}");
				this.RaisePropertyChanged ();
			}

		}
		public MVVMObservableCollection<string> Names { get; set; } = new MVVMObservableCollection<string> () {
			"Jack",
			"Steve",
			"John"
		};
		public event PropertyChangedEventHandler PropertyChanged;

		string _Text = "默认值";

	}

}