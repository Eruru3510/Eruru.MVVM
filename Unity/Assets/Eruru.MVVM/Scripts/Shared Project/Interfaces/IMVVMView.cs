namespace Eruru.MVVM {

	public interface IMVVMView {

		MVVMControl Control { get; set; }
		MVVMAction OnLoaded { get; set; }

	}

}