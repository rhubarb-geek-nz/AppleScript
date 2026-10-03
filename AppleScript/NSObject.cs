// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;

namespace RhubarbGeekNz.AppleScript
{
    internal static class NSObject
    {
        static private readonly IntPtr isKindOfClass = ObjC.sel_registerName("isKindOfClass:");

        static internal bool IsKindOfClass(IntPtr obj, IntPtr cls)
        {
            return 0 != (byte)ObjC.msgSend(obj, isKindOfClass, cls);
        }
    }
}
