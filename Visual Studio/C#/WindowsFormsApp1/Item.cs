using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1 {

	public class Item {

		public string Name { get; set; }
		public int Age { get; set; }
		public string School { get; set; }
		public string Remark { get; set; }

		public Item () {

		}
		public Item (string name, int age, string school, string remark) {
			Name = name;
			Age = age;
			School = school;
			Remark = remark;
		}

	}

}