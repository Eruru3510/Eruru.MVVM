using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Eruru.MVVM {

	public delegate void MVVMAction ();
	public delegate TResult MVVMFunc<out TResult> ();
	public delegate TResult MVVMFunc<in T, out TResult> (T arg);
	public delegate TResult MVVMFunc<in T1, in T2, out TResult> (T1 arg1, T2 arg2);

	static class MVVMAPI {

		public static BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public static byte ToByte (object value) {
			try {
				return Convert.ToByte (value);
			} catch {
				return default (byte);
			}
		}
		public static ushort ToUShort (object value) {
			try {
				return Convert.ToUInt16 (value);
			} catch {
				return default (ushort);
			}
		}
		public static uint ToUInt (object value) {
			try {
				return Convert.ToUInt32 (value);
			} catch {
				return default (uint);
			}
		}
		public static ulong ToULong (object value) {
			try {
				return Convert.ToUInt64 (value);
			} catch {
				return default (ulong);
			}
		}
		public static sbyte ToSByte (object value) {
			try {
				return Convert.ToSByte (value);
			} catch {
				return default (sbyte);
			}
		}
		public static short TOShort (object value) {
			try {
				return Convert.ToInt16 (value);
			} catch {
				return default (short);
			}
		}
		public static int ToInt (object value) {
			try {
				return Convert.ToInt32 (value);
			} catch {
				return default (int);
			}
		}
		public static long ToLong (object value) {
			try {
				return Convert.ToInt64 (value);
			} catch {
				return default (long);
			}
		}
		public static float ToFloat (object value) {
			try {
				return Convert.ToSingle (value);
			} catch {
				return default (float);
			}
		}
		public static double ToDouble (object value) {
			try {
				return Convert.ToDouble (value);
			} catch {
				return default (double);
			}
		}
		public static decimal ToDecimal (object value) {
			try {
				return Convert.ToDecimal (value);
			} catch {
				return default (decimal);
			}
		}
		public static bool ToBool (object value) {
			try {
				return Convert.ToBoolean (value);
			} catch {
				return default (bool);
			}
		}
		public static char ToChar (object value) {
			try {
				return Convert.ToChar (value);
			} catch {
				return default (char);
			}
		}
		public static string ToString (object value) {
			try {
				return Convert.ToString (value) ?? string.Empty;
			} catch {
				return string.Empty;
			}
		}
		public static DateTime ToDateTime (object value) {
			try {
				return Convert.ToDateTime (value);
			} catch {
				return default (DateTime);
			}
		}
		public static Color ToColor (object value) {
			try {
				return (Color)value;
			} catch {
				return Color.black;
			}
		}

		public static object ChangeType (object value, Type type) {
			if (type == null) {
				throw new ArgumentNullException ("type");
			}
			switch (Type.GetTypeCode (type)) {
				case TypeCode.Byte:
					return ToByte (value);
				case TypeCode.UInt16:
					return ToUShort (value);
				case TypeCode.UInt32:
					return ToUInt (value);
				case TypeCode.UInt64:
					return ToULong (value);
				case TypeCode.SByte:
					return ToSByte (value);
				case TypeCode.Int16:
					return TOShort (value);
				case TypeCode.Int32:
					return ToInt (value);
				case TypeCode.Int64:
					return ToLong (value);
				case TypeCode.Single:
					return ToFloat (value);
				case TypeCode.Double:
					return ToDouble (value);
				case TypeCode.Decimal:
					return ToDecimal (value);
				case TypeCode.Boolean:
					return ToBool (value);
				case TypeCode.Char:
					return ToChar (value);
				case TypeCode.String:
					return ToString (value);
				case TypeCode.DateTime:
					return ToDateTime (value);
				default:
					throw new NotImplementedException ();
			}
		}

		public static string GetCallerMemberName () {
			string name = new StackTrace ().GetFrame (2).GetMethod ().Name;
			if (!name.StartsWith ("set_")) {
				throw new Exception ("请在属性访问器的set内调用");
			}
			return name.Substring (4);
		}

		public static void SetPropertyValue (PropertyInfo propertyInfo, object instance, object value) {
			if (propertyInfo.PropertyType == typeof (MVVMBinding)) {
				((MVVMBinding)propertyInfo.GetValue (instance, null)).SetTargetValue (value);
				return;
			}
			propertyInfo.SetValue (instance, ChangeType (value, propertyInfo.PropertyType), null);
		}

	}

}