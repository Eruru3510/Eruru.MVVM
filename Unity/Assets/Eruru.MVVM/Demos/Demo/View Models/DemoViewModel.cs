using System;
using System.ComponentModel;
using UnityEngine;

namespace Eruru.MVVM.Demo {

	public class DemoViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public MVVMObservableCollection<DemoModel> Models {

			get {
				return _Models;
			}

			set {
				_Models = value;
				this.RaisePropertyChanged ();
			}

		}
		public MVVMRelayCommand OnClick { get; set; }
		public MVVMRelayCommand OnDelete { get; set; }
		public MVVMRelayCommand OnEdit { get; set; }

		MVVMObservableCollection<DemoModel> _Models = new MVVMObservableCollection<DemoModel> ();

		public DemoViewModel () {
			OnClick = new MVVMRelayCommand (value => {
				OpenAddView (demoAddViewModel => {
					Models.Add ((DemoModel)demoAddViewModel.Model.Clone ());
				});
			});
			OnEdit = new MVVMRelayCommand (value => {
				MVVMControl control = (MVVMControl)value;
				DemoModel demoModel = (DemoModel)control.DataContext.GetTargetValue ();
				int index = Models.IndexOf (demoModel);
				demoModel = (DemoModel)demoModel.Clone ();
				OpenAddView (demoAddViewModel => {
					Models[index] = demoAddViewModel.Model;
				}, demoModel);
			});
			OnDelete = new MVVMRelayCommand (value => {
				Models.Remove ((DemoModel)value);
			});
		}

		void OpenAddView (Action<DemoAddViewModel> onConfirm, DemoModel demoModel = null) {
			DemoAddView demoAddView = UnityEngine.Object.Instantiate (Resources.Load<DemoAddView> ("Demo Add View"));
			demoAddView.OnLoaded = () => {
				DemoAddViewModel demoAddViewModel = (DemoAddViewModel)demoAddView.Control.DataContext.GetTargetValue ();
				if (demoModel != null) {
					demoAddViewModel.Model = demoModel;
				}
				demoAddViewModel.OnConfirm = () => {
					onConfirm (demoAddViewModel);
				};
			};
		}

	}

}