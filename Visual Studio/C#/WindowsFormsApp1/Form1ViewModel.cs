using Eruru.MVVM;
using System;
using System.ComponentModel;

namespace WindowsFormsApp1 {

	class Form1ViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public MVVMObservableCollection<Model> Books { get; set; } = new MVVMObservableCollection<Model> ();
		public MVVMRelayCommand OnAdd {

			get {
				Console.WriteLine ("get onadd");
				return _OnAdd;
			}

			set {
				Console.WriteLine ("set onadd");
				_OnAdd = value;
			}

		}

		MVVMRelayCommand _OnAdd;

		public Form1ViewModel () {
			OnAdd = new MVVMRelayCommand (value => {
				new FormAdd ().ShowDialog ();
			});
		}

	}

}
