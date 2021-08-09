using System;
using System.Windows;

namespace WpfApp1 {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow () {
			InitializeComponent ();
			DataContext = new ViewModel ();
		}
		class ViewModel {

			public string[] Items { get; set; } = { "A", "B", "C" };
			public string Text {

				get => _Text;

				set {
					_Text = value;
					Console.WriteLine ("Set");
				}

			}

			string _Text;

		}

		private void Button_Click (object sender, RoutedEventArgs e) {
			Slider.Content = DateTime.Now;
		}
	}
}
