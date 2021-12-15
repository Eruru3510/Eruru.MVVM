using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;

namespace Eruru.MVVM {

	public delegate void MVVMAction ();
	public delegate void MVVMAction<in T1, in T2> (T1 arg1, T2 arg2);
	public delegate TResult MVVMFunc<out TResult> ();
	public delegate TResult MVVMFunc<in T, out TResult> (T arg);
	public delegate TResult MVVMFunc<in T1, in T2, out TResult> (T1 arg1, T2 arg2);

	public static class MVVMAPI {

		public static T To<T> (object value) {
			return (T)ChangeType (value, typeof (T));
		}

		public static object ChangeType (object value, Type type) {
			if (value == null) {
				if (type == typeof (string)) {
					return string.Empty;
				}
			} else {
				if (type.IsAssignableFrom (value.GetType ())) {
					return value;
				}
			}
			return Convert.ChangeType (value, type);
		}

		public static string GetCallerMemberName () {
			string name = new StackTrace ().GetFrame (2).GetMethod ().Name;
			if (!name.StartsWith ("set_")) {
				throw new Exception ("请在属性的set内调用");
			}
			return name.Substring (4);
		}

		public static void Log (string format, params object[] args) {
#if DEBUG
#if UNITY_EDITOR
			UnityEngine.Debug.LogFormat (format, args);
#endif
			Console.WriteLine (format, args);
#endif
		}

		public static void Error (string format, params object[] args) {
#if DEBUG
#if UNITY_EDITOR
			UnityEngine.Debug.ErrorFormat (format, args);
#endif
			ConsoleColor foregroundColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine (format, args);
			Console.ForegroundColor = foregroundColor;
#endif
		}

	}

}