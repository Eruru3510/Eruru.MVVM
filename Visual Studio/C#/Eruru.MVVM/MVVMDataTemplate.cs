namespace Eruru.MVVM {

	public class MVVMDataTemplate {

		public MVVMFunc<MVVMControlBase> Build { get; set; }

		public MVVMDataTemplate (MVVMFunc<MVVMControlBase> build) {
			Build = build;
		}

	}

}