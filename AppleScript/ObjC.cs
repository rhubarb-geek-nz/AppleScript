// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace RhubarbGeekNz.AppleScript
{
    internal static partial class ObjC
    {
        private const string libobjc_dylib = "/usr/lib/libobjc.A.dylib";
        [LibraryImport(libobjc_dylib)]
        public static partial IntPtr object_getClassName(IntPtr obj);

        [LibraryImport(libobjc_dylib)]
        public static partial IntPtr sel_registerName([MarshalAs(UnmanagedType.LPStr)] string selectorName);

        internal static string GetObjCClassName(IntPtr obj)
        {
            IntPtr ptr = object_getClassName(obj);
            return Marshal.PtrToStringAnsi(ptr);
        }

        [LibraryImport(libobjc_dylib)]
        public static partial IntPtr objc_getClass([MarshalAs(UnmanagedType.LPStr)] string className);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, IntPtr a);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, IntPtr a, IntPtr b);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, IntPtr a, IntPtr b, IntPtr c);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, out IntPtr a);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, double a);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, long a, double b, out IntPtr c);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, [MarshalAs(UnmanagedType.I1)] bool a);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, IntPtr a, out IntPtr b);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr msgSend(IntPtr ptr, IntPtr sel, Int32 a, Int32 b, IntPtr c, Int16 d, Int32 e);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial IntPtr getValue(IntPtr ptr, IntPtr sel, [Out] byte[] bytes);

        [LibraryImport(libobjc_dylib, EntryPoint = "objc_msgSend")]
        internal static partial double msgSend_fpret(IntPtr ptr, IntPtr sel);
    }
}
