// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace RhubarbGeekNz.AppleScript
{
    internal static class NSValue
    {
        static IntPtr objCType = ObjC.sel_registerName("objCType");
        static IntPtr getValue = ObjC.sel_registerName("getValue:");

        internal static string ObjCType(IntPtr obj)
        {
            return Marshal.PtrToStringAnsi(ObjC.msgSend(obj, objCType));
        }

        public static byte[] GetValue(IntPtr value)
        {
            IntPtr p = ObjC.msgSend(value, objCType);
            Foundation.NSGetSizeAndAlignment(p, out var size, out var align);

            byte[] bytes = new byte[size];

            ObjC.getValue(value, getValue, bytes);

            return bytes;
        }

    }
}
