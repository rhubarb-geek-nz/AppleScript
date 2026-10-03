// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;

namespace RhubarbGeekNz.AppleScript
{
    internal static class NSString
    {
        const int kCFStringEncodingUTF8 = 0x08000100;

        internal static string ToString(IntPtr str)
        {
            IntPtr ptr = CoreFoundation.CFStringCreateExternalRepresentation(IntPtr.Zero, str, kCFStringEncodingUTF8, 0);
            byte[] ba = NSData.GetBytes(ptr);
            return System.Text.Encoding.UTF8.GetString(ba);
        }

        internal static IntPtr FromString(string s)
        {
            IntPtr data = NSData.FromBytes(System.Text.Encoding.UTF8.GetBytes(s));
            return CoreFoundation.CFStringCreateFromExternalRepresentation(IntPtr.Zero, data, kCFStringEncodingUTF8);
        }
    }
}
