// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections;

namespace RhubarbGeekNz.AppleScript
{
    public sealed class AppleScriptException : Exception
    {
        public AppleScriptException(String message, object errorObject) : base(message)
        {
            if (errorObject is IDictionary dict)
            {
                foreach (DictionaryEntry entry in dict)
                {
                    Data.Add(entry.Key, entry.Value);

                    switch ((String)entry.Key)
                    {
                        case "NSAppleScriptErrorNumber":
                            if (entry.Value is Int16 i16)
                            {
                                HResult = i16;
                            }
                            else
                            {
                                if (entry.Value is Int32 i32)
                                {
                                    HResult = i32;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
