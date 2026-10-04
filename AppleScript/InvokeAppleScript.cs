// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.IO;
using System.Management.Automation;

namespace RhubarbGeekNz.AppleScript
{
    [Cmdlet(VerbsLifecycle.Invoke, "AppleScript")]
    sealed public class InvokeAppleScript : PSCmdlet
    {
        const String ParameterSetNameInput = "input";
        const String ParameterSetNameScript = "script";
        const String ParameterSetNameFile = "file";
        const String ParameterSetNameUri = "uri";

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ParameterSetNameInput)]
        public String InputScript;

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetNameScript)]
        public String ScriptBlock;

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetNameFile)]
        public FileInfo FileInfo;

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetNameUri)]
        public Uri Uri;

        [Parameter(ParameterSetName = ParameterSetNameInput)]
        [Parameter(Position = 1, ParameterSetName = ParameterSetNameScript)]
        [Parameter(Position = 1, ParameterSetName = ParameterSetNameFile)]
        [Parameter(Position = 1, ParameterSetName = ParameterSetNameUri)]
        public String SubroutineName;

        [Parameter(ParameterSetName = ParameterSetNameInput)]
        [Parameter(Position = 2, ValueFromPipeline = true, ParameterSetName = ParameterSetNameScript)]
        [Parameter(Position = 2, ValueFromPipeline = true, ParameterSetName = ParameterSetNameFile)]
        [Parameter(Position = 2, ValueFromPipeline = true, ParameterSetName = ParameterSetNameUri)]
        public object[] ArgumentList;

        protected override void ProcessRecord()
        {
            using var pool = new NSAutoreleasePool();
            IntPtr appleScript = ObjC.msgSend(NSClass.NSAppleScript, NSAppleScript.alloc);
            object errorDict = null, result = null;

            switch (ParameterSetName)
            {
                case ParameterSetNameInput:
                    {
                        IntPtr stringPtr = NSString.FromString(InputScript);
                        appleScript = ObjC.msgSend(appleScript, NSAppleScript.initWithSource, stringPtr);
                    }
                    break;
                case ParameterSetNameScript:
                    {
                        IntPtr stringPtr = NSString.FromString(ScriptBlock);
                        appleScript = ObjC.msgSend(appleScript, NSAppleScript.initWithSource, stringPtr);
                    }
                    break;
                case ParameterSetNameFile:
                    {
                        IntPtr stringPtr = NSString.FromString(FileInfo.FullName);
                        IntPtr uriPtr = ObjC.msgSend(NSClass.NSURL, NSAppleEventDescriptor.fileURLWithPath, stringPtr);
                        appleScript = ObjC.msgSend(appleScript, NSAppleScript.initWithContentsOfURLerror, uriPtr, out IntPtr error);
                        if (error != IntPtr.Zero) errorDict = NSAppleEventDescriptor.ObjectFromIntPtr(error);
                    }
                    break;
                case ParameterSetNameUri:
                    {
                        IntPtr stringPtr = NSString.FromString(Uri.ToString());
                        IntPtr uriPtr = ObjC.msgSend(NSClass.NSURL, NSAppleEventDescriptor.URLWithString, stringPtr);
                        appleScript = ObjC.msgSend(appleScript, NSAppleScript.initWithContentsOfURLerror, uriPtr, out IntPtr error);
                        if (error != IntPtr.Zero) errorDict = NSAppleEventDescriptor.ObjectFromIntPtr(error);
                    }
                    break;
            }

            if (errorDict == null)
            {
                result = NSAppleScript.Invoke(appleScript, SubroutineName, ArgumentList, out errorDict);
            }

            if (errorDict != null)
            {
                String errorMessage = null;
                object errorNumber = -1;

                if (errorDict is IDictionary table)
                {
                    foreach (DictionaryEntry entry in table)
                    {
                        switch (entry.Key.ToString())
                        {
                            case "NSAppleScriptErrorMessage":
                                errorMessage = entry.Value.ToString();
                                break;
                            case "NSAppleScriptErrorNumber":
                                errorNumber = entry.Value;
                                break;
                        }
                    }
                }

                String message = errorMessage == null ? errorNumber.ToString() : errorMessage;
                Exception ex = new AppleScriptException(message, errorDict);
                WriteError(new ErrorRecord(ex, ex.GetType().Name, ErrorCategory.InvalidResult, ScriptBlock));
            }
            else
            {
                WriteObject(result, false);
            }
        }
    }
}
