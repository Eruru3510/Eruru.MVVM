// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
#if NET45_OR_GREATER

#else
namespace System.Runtime.CompilerServices {

	[AttributeUsage (AttributeTargets.Parameter, Inherited = false)]
	public sealed class CallerMemberNameAttribute : Attribute {

		public CallerMemberNameAttribute () {

		}

	}

}
#endif