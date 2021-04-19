using System;
using System.Diagnostics;
using System.Reflection;

namespace Eruru.MVVM {

	public delegate TResult MVVMFunc<out TResult> ();
	public delegate TResult MVVMFunc<in T, out TResult> (T arg);
	public delegate void MVVMAction ();

	static class MVVMApi {

		public static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static int ToInt (object value) {
			try {
				return Convert.ToInt32 (value);
			} catch {
				return default (int);
			}
		}
		public static float ToFloat (object value) {
			try {
				return Convert.ToSingle (value);
			} catch {
				return default (float);
			}
		}
		public static string ToString (object value) {
			try {
				return Convert.ToString (value);
			} catch {
				return default (string);
			}
		}
		public static bool ToBool (object value) {
			try {
				return Convert.ToBoolean (value);
			} catch {
				return default (bool);
			}
		}

		public static string GetCallerMemberName () {
			string name = new StackTrace ().GetFrame (2).GetMethod ().Name;
			if (!name.StartsWith ("set_")) {
				throw new Exception ("请在属性访问器的set内调用");
			}
			return name.Substring (4);
		}

	}

}