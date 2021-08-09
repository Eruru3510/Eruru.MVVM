using UnityEngine;
using System.Collections;

namespace Eruru.MVVM.Demo.ItemsControl {

	public class AddViewModel {

		public Item Item { get; set; }
		public string[] Schools { get; set; }

		public AddViewModel () {
			Schools = new string[] { "中山小学", "复兴中学", "光明小学" };
		}

	}

}