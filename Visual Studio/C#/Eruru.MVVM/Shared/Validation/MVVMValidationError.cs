using System;
using System.ComponentModel;

namespace Eruru.MVVM {

	public class MVVMValidationError : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public object ErrorContent {

			get {
				return _ErrorContent;
			}

			set {
				_ErrorContent = value;
				this.RaisePropertyChanged ();
			}

		}
		public Exception Exception {

			get {
				return _Exception;
			}

			set {
				_Exception = value;
				this.RaisePropertyChanged ();
			}

		}

		Exception _Exception;
		object _ErrorContent;

		public MVVMValidationError (object errorContent, Exception exception) {
			_ErrorContent = errorContent;
			_Exception = exception;
		}

	}

}