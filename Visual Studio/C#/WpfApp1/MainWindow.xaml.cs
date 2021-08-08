using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
