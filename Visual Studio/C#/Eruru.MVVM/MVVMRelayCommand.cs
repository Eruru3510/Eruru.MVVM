using System;

namespace Eruru.MVVM {

	public class MVVMRelayCommand : IMVVMCommand {

		readonly Predicate<object> _CanExecute;
		readonly Action<object> _Execute;

		public MVVMRelayCommand (Action<object> execute, Predicate<object> canExecute) {
			_Execute = execute;
			_CanExecute = canExecute;
		}
		public MVVMRelayCommand (Action<object> execute) : this (execute, null) {

		}

		public bool CanExecute (object parameter) {
			return _CanExecute == null || _CanExecute (parameter);
		}

		public void Execute (object parameter) {
			if (_Execute != null) {
				_Execute (parameter);
			}
		}

	}

	public class MVVMRelayCommand<T> : IMVVMCommand<T> {

		readonly Predicate<T> _CanExecute;
		readonly Action<T> _Execute;

		public MVVMRelayCommand (Action<T> execute, Predicate<T> canExecute) {
			_Execute = execute;
			_CanExecute = canExecute;
		}
		public MVVMRelayCommand (Action<T> execute) : this (execute, null) {

		}

		public bool CanExecute (T parameter) {
			return _CanExecute == null || _CanExecute (parameter);
		}

		public void Execute (T parameter) {
			if (_Execute != null) {
				_Execute (parameter);
			}
		}

		bool IMVVMCommand.CanExecute (object parameter) {
			return CanExecute (MVVMAPI.To<T> (parameter));
		}

		void IMVVMCommand.Execute (object parameter) {
			Execute (MVVMAPI.To<T> (parameter));
		}

	}

}