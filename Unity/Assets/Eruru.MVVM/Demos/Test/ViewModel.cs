using UnityEngine;
using System.Collections;
using System.ComponentModel;

namespace Eruru.MVVM.Demo.Test {

	public class ViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public string Text {

			get {
				return _Text;
			}

			set {
				_Text = value;
				this.RaisePropertyChanged ();
			}

		}

		string _Text = null;

	}

}