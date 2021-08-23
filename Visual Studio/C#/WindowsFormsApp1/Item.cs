using Eruru.MVVM;
using System;
using System.ComponentModel;

namespace WindowsFormsApp1 {

	public class Item : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public string Name { get; set; }
		public int Age {

			get => _Age;

			set {
				if (value < 0) {
					throw new Exception ("年龄不能小于0岁");
				}
				_Age = value;
				this.RaisePropertyChanged ();
			}

		}
		public string School { get; set; }
		public string Remark { get; set; }

		int _Age;

		public Item () {

		}
		public Item (string name, int age, string school, string remark) {
			Name = name;
			Age = age;
			School = school;
			Remark = remark;
		}

		public Item Clone () {
			return new Item (Name, Age, School, Remark);
		}

	}

}