using System;
using System.Diagnostics;

namespace Eruru.MVVM {

	public delegate void MVVMAction ();
	public delegate void MVVMAction<in T1, in T2> (T1 arg1, T2 arg2);
	public delegate TResult MVVMFunc<out TResult> ();
	public delegate TResult MVVMFunc<in T, out TResult> (T arg);
	public delegate TResult MVVMFunc<in T1, in T2, out TResult> (T1 arg1, T2 arg2);

	public static class MVVMAPI {

		public static T To<T> (object value) {
			if (value == null && typeof (T) == typeof (string)) {
				return (T)(object)string.Empty;
			}
			return (T)Convert.ChangeType (value, typeof (T));
		}

		public static object ChangeType (object value, Type type) {
			switch (Type.GetTypeCode (type)) {
				case TypeCode.Byte:
					return To<byte> (value);
				case TypeCode.UInt16:
					return To<ushort> (value);
				case TypeCode.UInt32:
					return To<uint> (value);
				case TypeCode.UInt64:
					return To<ulong> (value);
				case TypeCode.SByte:
					return To<sbyte> (value);
				case TypeCode.Int16:
					return To<short> (value);
				case TypeCode.Int32:
					return To<int> (value);
				case TypeCode.Int64:
					return To<long> (value);
				case TypeCode.Single:
					return To<float> (value);
				case TypeCode.Double:
					return To<double> (value);
				case TypeCode.Decimal:
					return To<decimal> (value);
				case TypeCode.Boolean:
					return To<bool> (value);
				case TypeCode.Char:
					return To<char> (value);
				case TypeCode.String:
					return To<string> (value);
				case TypeCode.DateTime:
					return To<DateTime> (value);
				default:
					throw new NotImplementedException (type.ToString ());
			}
		}

		public static string GetCallerMemberName () {
			string name = new StackTrace ().GetFrame (2).GetMethod ().Name;
			if (!name.StartsWith ("set_")) {
				throw new Exception ("请在属性的set内调用");
			}
			return name.Substring (4);
		}

		public static void Log (string text) {
#if UNITY_EDITOR
			//UnityEngine.Debug.LogError (text, Control.Control);
#endif
			Console.WriteLine (text);
		}

	}

}