using System;
using System.ComponentModel;

namespace Eruru.MVVM.Demo {

	public class DemoAddViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public DemoModel Model {

			get {
				return _Model;
			}

			set {
				_Model = value;
				this.RaisePropertyChanged ();
			}

		}
		public MVVMRelayCommand OnClick { get; set; }
		public Action OnConfirm;

		DemoModel _Model;

		public DemoAddViewModel () {
			Model = new DemoModel () {
				Text = "默认值",
				Options = new MVVMObservableCollection<string> () {
					"A",
					"B",
					"C"
				}
			};
			Model.MaxValue = Model.Options.Count - 1;
			Model.Value = 1;
			OnClick = new MVVMRelayCommand (value => {
				if (OnConfirm != null) {
					OnConfirm ();
				}
			});
		}

	}

}