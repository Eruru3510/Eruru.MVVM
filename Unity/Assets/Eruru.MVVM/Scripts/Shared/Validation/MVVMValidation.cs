
using System.ComponentModel;

namespace Eruru.MVVM {

	public class MVVMValidation : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public bool HasError {

			get {
				return _HasError;
			}

			set {
				_HasError = value;
				this.RaisePropertyChanged ();
			}

		}
		public MVVMValidationErrorCollection Errors {

			get {
				return _Errors;
			}

			set {
				_Errors = value;
			}

		}

		MVVMValidationErrorCollection _Errors = new MVVMValidationErrorCollection ();
		bool _HasError;

		public void AddError (MVVMValidationError validationError) {
			Errors.Add (validationError);
			Errors.CurrentItem = validationError;
			HasError = true;
		}

		public void ClearError () {
			HasError = false;
			Errors.CurrentItem = null;
			Errors.Clear ();
		}

	}

}