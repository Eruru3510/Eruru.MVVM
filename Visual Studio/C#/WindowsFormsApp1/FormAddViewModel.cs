using Eruru.MVVM;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	class FormAddViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public Model Model { get; set; } = new Model ();
		public MVVMRelayCommand OnConfirm { get; set; }
		public MVVMRelayCommand OnCancel { get; set; }
		public MVVMRelayCommand OnAdd { get; set; }

		public FormAddViewModel () {
			OnConfirm = new MVVMRelayCommand (value => {
				MessageBox.Show (value?.ToString ());
			});
			OnCancel = new MVVMRelayCommand (value => {

			});
			OnAdd = new MVVMRelayCommand (value => {
				Model.Names.Add (DateTime.Now.ToString ());
			});
		}

	}

}
