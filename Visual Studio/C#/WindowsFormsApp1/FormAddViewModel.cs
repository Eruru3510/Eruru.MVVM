using Eruru.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	class FormAddViewModel {

		public Item Item { get; set; }
		public string[] Schools { get; set; } = { "中山小学", "复兴小学", "光明小学" };

	}

}
