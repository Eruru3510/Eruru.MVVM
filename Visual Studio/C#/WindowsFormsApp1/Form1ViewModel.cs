using Eruru.MVVM;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	class Form1ViewModel {

		public MVVMObservableCollection<Item> Items { get; set; } = new MVVMObservableCollection<Item> () {
			new Item ("小明", 18, "光明小学", "三好学生"),
			new Item ("小刚", 20, "复兴中学", "优秀班干部"),
			new Item ("吉姆格林", 19, "光明小学", "吉姆做了汽车公司经理"),
			new Item ("李雷", 25, "复兴中学", "不老实的家伙"),
			new Item ("韩梅梅", 22, "光明小学", "在一起")
		};
		public MVVMRelayCommand OnAdd { get; set; }
		public MVVMRelayCommand<Item> OnEdit { get; set; }
		public MVVMRelayCommand<Item> OnDelete { get; set; }

		public Form1ViewModel () {
			OnAdd = new MVVMRelayCommand (value => {
				FormAdd formAdd = new FormAdd ();
				MVVMControlBase control = formAdd.Build ();
				if (formAdd.ShowDialog () == DialogResult.OK) {
					Items.Add (control.DataContext.GetTargetValue<FormAddViewModel> ().Item);
				}
			});
			OnEdit = new MVVMRelayCommand<Item> (value => {
				FormAdd formAdd = new FormAdd (value.DeepClone ());
				MVVMControlBase control = formAdd.Build ();
				if (formAdd.ShowDialog () == DialogResult.OK) {
					Items[Items.IndexOf (value)] = control.DataContext.GetTargetValue<FormAddViewModel> ().Item;
				}
			});
			OnDelete = new MVVMRelayCommand<Item> (value => {
				Items.Remove (value);
			});
		}

	}

}