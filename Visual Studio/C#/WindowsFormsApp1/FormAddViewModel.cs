namespace WindowsFormsApp1 {

	class FormAddViewModel {

		public Item Item { get; set; }
		public string[] Schools { get; set; } = { "中山小学", "复兴中学", "光明小学" };

		public FormAddViewModel (Item item) {
			Item = item;
		}

	}

}