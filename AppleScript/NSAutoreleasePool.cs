// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;

namespace RhubarbGeekNz.AppleScript
{
    internal class NSAutoreleasePool : IDisposable
    {
        static IntPtr selNew = ObjC.sel_registerName("new");
        static IntPtr selDrain = ObjC.sel_registerName("drain");
        private IntPtr? pool = ObjC.msgSend(NSClass.NSAutoreleasePool, selNew);

        public void Dispose()
        {
            if (pool.HasValue)
            {
                ObjC.msgSend(pool.Value, selDrain);
            }

            pool = null;
        }
    }
}
