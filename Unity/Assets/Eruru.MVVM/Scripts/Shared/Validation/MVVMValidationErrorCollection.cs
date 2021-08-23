namespace Eruru.MVVM {

	public class MVVMValidationErrorCollection : MVVMObservableCollection<MVVMValidationError> {

		public MVVMValidationError CurrentItem {

			get {
				return _CurrentItem;
			}

			set {
				_CurrentItem = value;
				this.RaisePropertyChanged ();
			}

		}

		MVVMValidationError _CurrentItem;

	}

}