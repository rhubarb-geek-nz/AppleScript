# rhubarb-geek-nz/AppleScript
AppleScript for PowerShell

## Invoke-AppleScript

This runs [AppleScript](https://developer.apple.com/library/archive/documentation/AppleScript/Conceptual/AppleScriptLangGuide/introduction/ASLR_intro.html) either from text in memory or a file.

```
Invoke-AppleScript [-ScriptBlock] <string> [[-SubroutineName] <string>] [[-ArgumentList] <Object[]>] [<CommonParameters>]

Invoke-AppleScript [-FileInfo] <FileInfo> [[-SubroutineName] <string>] [[-ArgumentList] <Object[]>] [<CommonParameters>]

Invoke-AppleScript [-Uri] <uri> [[-SubroutineName] <string>] [[-ArgumentList] <Object[]>] [<CommonParameters>]
```

See [test.ps1](test.ps1) for examples.

## Implementation Notes

This uses [LibraryImport](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.libraryimportattribute) rather than [DllImport](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.dllimportattribute) and has no native support library.
It uses Objective-C directly to access classes, selectors and send messages.
The classes used come from [CoreFoundation](https://developer.apple.com/documentation/corefoundation) and [Foundation](https://developer.apple.com/documentation/foundation).

It uses a managed [NSAutoreleasePool](https://developer.apple.com/documentation/foundation/nsautoreleasepool) during the execution of the PowerShell code.

The loading of classes and selectors is done using the C API from [libobjc.A.dylib](AppleScript/ObjC.cs), so no memory management during static loading.

The majority of the work is converting PowerShell objects to and from [NSAppleEventDescriptor](https://developer.apple.com/documentation/foundation/nsappleeventdescriptor).

The project includes unit tests for validating the round-trip handling of different types.

The four character [DescType](https://developer.apple.com/documentation/coreservices/desctype?language=objc) codes are defined using constants.

## Gotchas

This mechanism is for executing independent scripts, or scripts with subroutines defined. It cannot run the 'on run argv' type scripts, this is a known problem. This implementation uses kASAppleScriptSuite:kASSubroutineEvent along with keyASSubroutineName and keyDirectObject.
