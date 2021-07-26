namespace Eruru.MVVM {

	public class MVVMDataTemplate {

		public MVVMFunc<MVVMControl> Build { get; set; }

		public MVVMDataTemplate (MVVMFunc<MVVMControl> build) {
			Build = build;
		}

	}

}