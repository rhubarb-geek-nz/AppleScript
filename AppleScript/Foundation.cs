// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace RhubarbGeekNz.AppleScript
{
    internal static partial class Foundation
    {
        private const string FoundationLibrary = "/System/Library/Frameworks/Foundation.framework/Foundation";

        [LibraryImport(FoundationLibrary)]
        public static partial IntPtr NSGetSizeAndAlignment(IntPtr p, out IntPtr size, out IntPtr align);
    }
}
