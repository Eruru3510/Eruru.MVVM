package com.eruru.mvvm;

public class MVVMAPI {

	public static <T> T to (Object value, Class<T> type) {
		try {
			if (value == null || value.getClass () != type) {
				if (Byte.class.equals (type) || byte.class.equals (type)) {
					value = value == null ? 0B0 : Byte.parseByte (value.toString ());
				} else if (Short.class.equals (type) || short.class.equals (type)) {
					value = value == null ? (short) 0 : Short.parseShort (value.toString ());
				} else if (Integer.class.equals (type) || int.class.equals (type)) {
					value = value == null ? 0 : Integer.parseInt (value.toString ());
				} else if (Long.class.equals (type) || long.class.equals (type)) {
					value = value == null ? 0L : Long.parseLong (value.toString ());
				} else if (Float.class.equals (type) || float.class.equals (type)) {
					value = value == null ? 0F : Float.parseFloat (value.toString ());
				} else if (Double.class.equals (type) || double.class.equals (type)) {
					value = value == null ? 0D : Double.parseDouble (value.toString ());
				} else if (String.class.equals (type)) {
					value = value == null ? "" : value.toString ();
				}
			}
			return (T) value;
		} catch (Exception exception) {
			exception.printStackTrace ();
			try {
				return type.newInstance ();
			} catch (Exception exception1) {
				exception1.printStackTrace ();
				return null;
			}
		}
	}

	public static String getCallerMemberName () {
		return Thread.currentThread ().getStackTrace ()[3].getMethodName ().substring (3);
	}

	public static String firstCharToLowerCase (String text) {
		if (text.length () == 0) {
			return text;
		}
		char[] chars = text.toCharArray ();
		if (chars[0] >= 'A' && chars[0] <= 'Z') {
			chars[0] += 32;
			return String.valueOf (chars);
		} else {
			return text;
		}
	}

	public static String firstCharToUpperCase (String str) {
		if (str.length () == 0) {
			return str;
		}
		char[] chars = str.toCharArray ();
		if (chars[0] >= 'a' && chars[0] <= 'z') {
			chars[0] -= 32;
			return String.valueOf (chars);
		} else {
			return str;
		}
	}

}