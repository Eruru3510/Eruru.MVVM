using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp1 {

	static class Program {

		[DllImport ("kernel32.dll", SetLastError = true)]
		[return: MarshalAs (UnmanagedType.Bool)]
		static extern bool AllocConsole ();

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main () {
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			Application.Run (new Form1 ());
		}

	}

}