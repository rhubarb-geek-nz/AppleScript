// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;

namespace RhubarbGeekNz.AppleScript
{
    internal static class NSClass
    {
        static internal readonly IntPtr NSAppleEventDescriptor = GetClass("NSAppleEventDescriptor");
        static internal readonly IntPtr NSAppleScript = GetClass("NSAppleScript");
        static internal readonly IntPtr NSDictionary = GetClass("NSDictionary");
        static internal readonly IntPtr NSString = GetClass("NSString");
        static internal readonly IntPtr NSData = GetClass("NSData");
        static internal readonly IntPtr NSNumber = GetClass("NSNumber");
        static internal readonly IntPtr NSConcreteValue = GetClass("NSConcreteValue");
        static internal readonly IntPtr NSDate = GetClass("NSDate");
        static internal readonly IntPtr NSAutoreleasePool = GetClass("NSAutoreleasePool");
        static internal readonly IntPtr NSURL = GetClass("NSURL");

        static private IntPtr GetClass(String className)
        {
            return ObjC.objc_getClass(className);
        }
    }
}
