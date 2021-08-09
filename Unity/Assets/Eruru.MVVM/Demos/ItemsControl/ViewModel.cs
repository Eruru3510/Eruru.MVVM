using UnityEngine;
using System.Collections;

namespace Eruru.MVVM.Demo.ItemsControl {

	public class ViewModel {

		public MVVMObservableCollection<Item> Items { get; set; }
		public MVVMRelayCommand OnAdd { get; set; }
		public MVVMRelayCommand OnEdit { get; set; }
		public MVVMRelayCommand OnDelete { get; set; }

		public ViewModel () {
			Items = new MVVMObservableCollection<Item> () {
				new Item ("小明", 18, "光明小学", "三好学生"),
				new Item ("小刚", 20, "复兴中学", "优秀班干部"),
				new Item ("吉姆格林", 19, "光明小学", "吉姆做了汽车公司经理"),
				new Item ("李雷", 25, "复兴中学", "不老实的家伙"),
				new Item ("韩梅梅", 22, "光明小学", "在一起")
			};
			OnAdd = new MVVMRelayCommand (value => {
				AddView addView = Object.Instantiate (Resources.Load<AddView> ("ItemsControl/WindowAdd"));
				addView.Item = new Item ();
				MVVMControl control = addView.Build ();
				addView.OnConfirm = () => {
					Items.Add (control.DataContext.GetTargetValue<AddViewModel> ().Item);
				};
			});
			OnEdit = new MVVMRelayCommand (value => {
				AddView addView = Object.Instantiate (Resources.Load<AddView> ("ItemsControl/WindowAdd"));
				Item item = (Item)value;
				addView.Item = item.Clone ();
				MVVMControl control = addView.Build ();
				addView.OnConfirm = () => {
					Items[Items.IndexOf (item)] = control.DataContext.GetTargetValue<AddViewModel> ().Item;
				};
			});
			OnDelete = new MVVMRelayCommand (value => {
				Item item = (Item)value;
				Items.Remove (item);
			});
		}

	}

}