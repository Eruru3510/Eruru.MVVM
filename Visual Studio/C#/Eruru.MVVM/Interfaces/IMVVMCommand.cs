using System;
using System.Collections.Generic;
using System.Text;

namespace Eruru.MVVM {

	public interface IMVVMCommand {

		bool CanExecute (object parameter);

		void Execute (object parameter);

	}

	public interface IMVVMCommand<T> : IMVVMCommand {

		bool CanExecute (T parameter);

		void Execute (T parameter);

	}

}