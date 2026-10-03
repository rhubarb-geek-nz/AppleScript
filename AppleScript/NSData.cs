// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;

namespace RhubarbGeekNz.AppleScript
{
    internal static class NSData
    {
        internal static IntPtr FromBytes(byte[] bytes)
        {
            return CoreFoundation.CFDataCreate(IntPtr.Zero, bytes, bytes.Length);
        }

        internal static byte[] GetBytes(IntPtr obj)
        {
            long len = CoreFoundation.CFDataGetLength(obj);
            IntPtr ptr = CoreFoundation.CFDataGetBytePtr(obj);
            byte[] ba = new byte[len];
            if (len != 0)
            {
                Marshal.Copy(ptr, ba, 0, ba.Length);
            }
            return ba;
        }
    }
}
