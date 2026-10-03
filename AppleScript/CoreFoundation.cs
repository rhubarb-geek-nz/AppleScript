// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace RhubarbGeekNz.AppleScript
{
    internal static partial class CoreFoundation
    {
        private const string CoreFoundationLibrary = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

        [LibraryImport(CoreFoundationLibrary)]
        public static partial IntPtr CFDataGetBytePtr(IntPtr theData);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial nint CFDataGetLength(IntPtr theData);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial IntPtr CFDataCreate(IntPtr allocator, [In] byte[] bytes, nint length);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial IntPtr CFStringCreateFromExternalRepresentation(IntPtr allocator, IntPtr data, uint encoding);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial IntPtr CFStringCreateExternalRepresentation(IntPtr alloc, IntPtr theString, uint encoding, byte lossByte);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial long CFDictionaryGetCount(IntPtr dict);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial void CFDictionaryGetKeysAndValues(IntPtr dict, IntPtr[] keys, IntPtr[] values);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial long CFNumberGetByteSize(IntPtr num);

        [LibraryImport(CoreFoundationLibrary)]
        public static partial long CFNumberGetType(IntPtr num);

        [LibraryImport(CoreFoundationLibrary)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static partial bool CFNumberGetValue(IntPtr num, long type, byte[] values);
    }
}
