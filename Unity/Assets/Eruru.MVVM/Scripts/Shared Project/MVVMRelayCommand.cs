using System;

namespace Eruru.MVVM {

	public class MVVMRelayCommand {

		readonly Predicate<object> _CanExecute;
		readonly Action<object> _Execute;

		public MVVMRelayCommand (Action<object> execute, Predicate<object> canExecute) {
			if (execute == null) {
				throw new ArgumentNullException ("execute");
			}
			if (canExecute == null) {
				throw new ArgumentNullException ("canExecute");
			}
			_Execute = execute;
			_CanExecute = canExecute;
		}
		public MVVMRelayCommand (Action<object> execute) {
			if (execute == null) {
				throw new ArgumentNullException ("execute");
			}
			_Execute = execute;
		}

		public bool CanExecute (object parameter) {
			return _CanExecute == null ? true : _CanExecute (parameter);
		}

		public void Execute (object parameter) {
			if (_Execute != null) {
				_Execute (parameter);
			}
		}

	}

}