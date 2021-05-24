using System;
using System.ComponentModel;
using UnityEngine;

namespace Eruru.MVVM.Demo {

	public class DemoModel : INotifyPropertyChanged, ICloneable {

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
		public bool IsOn {

			get {
				return _IsOn;
			}

			set {
				_IsOn = value;
				this.RaisePropertyChanged ();
			}

		}
		public float Value {

			get {
				return _Value;
			}

			set {
				_Value = value;
				this.RaisePropertyChanged ();
			}

		}
		public float MaxValue {

			get {
				return _MaxValue;
			}

			set {
				_MaxValue = value;
				this.RaisePropertyChanged ();
			}

		}
		public float MinValue {

			get {
				return _MinValue;
			}

			set {
				_MinValue = value;
				this.RaisePropertyChanged ();
			}

		}
		public MVVMObservableCollection<string> Options {

			get {
				return _Options;
			}

			set {
				_Options = value;
				this.RaisePropertyChanged ();
			}

		}

		string _Text;
		bool _IsOn;
		float _Value;
		float _MaxValue;
		float _MinValue;
		MVVMObservableCollection<string> _Options;

		public object Clone () {
			return new DemoModel () {
				Text = Text,
				IsOn = IsOn,
				Value = Value,
				MaxValue = MaxValue,
				MinValue = MinValue,
				Options = Options
			};
		}

	}

}