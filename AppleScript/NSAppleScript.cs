// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;

namespace RhubarbGeekNz.AppleScript
{
    internal static class NSAppleScript
    {
        const Int32 kASAppleScriptSuite = 0x61736372; // ascr
        const Int32 kASSubroutineEvent = 0x70736272; // psbr
        internal static IntPtr alloc = ObjC.sel_registerName("alloc");
        internal static IntPtr initWithSource = ObjC.sel_registerName("initWithSource:");
        internal static IntPtr initWithContentsOfURLerror = ObjC.sel_registerName("initWithContentsOfURL:error:");
        static IntPtr executeAndReturnError = ObjC.sel_registerName("executeAndReturnError:");
        static IntPtr executeAppleEvent = ObjC.sel_registerName("executeAppleEvent:error:");
        static IntPtr appleEventWithEventClass = ObjC.sel_registerName("appleEventWithEventClass:eventID:targetDescriptor:returnID:transactionID:");
        static IntPtr setParamDescriptor = ObjC.sel_registerName("setParamDescriptor:forKeyword:");

        static internal object Invoke(IntPtr appleScript, String handlerParam, object[] args, out object errorDict, int depth)
        {
            IntPtr outError, resultPtr;

            if (handlerParam != null)
            {
                NSAppleEventDescriptor desc = new NSAppleEventDescriptor(depth);
                IntPtr handlerDesc = NSAppleEventDescriptor.FromString(handlerParam);
                IntPtr listParameters = desc.DescriptorFromObject(args);
                IntPtr targetDesc = desc.DescriptorFromObject(null);
                IntPtr eventDesc = ObjC.msgSend(NSClass.NSAppleEventDescriptor, appleEventWithEventClass, kASAppleScriptSuite, kASSubroutineEvent, targetDesc, -1, 0);
                ObjC.msgSend(eventDesc, setParamDescriptor, handlerDesc, NSAppleEventDescriptor.keyASSubroutineName);
                ObjC.msgSend(eventDesc, setParamDescriptor, listParameters, NSAppleEventDescriptor.keyDirectObject);

                resultPtr = ObjC.msgSend(appleScript, executeAppleEvent, eventDesc, out outError);
            }
            else
            {
                resultPtr = ObjC.msgSend(appleScript, executeAndReturnError, out outError);
            }

            errorDict = outError == IntPtr.Zero ? null : NSAppleEventDescriptor.ObjectFromIntPtr(outError);

            return resultPtr == IntPtr.Zero ? null : NSAppleEventDescriptor.ObjectFromDescriptor(resultPtr);
        }
    }
}
