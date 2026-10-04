#!/usr/bin/env pwsh
# Copyright (c) 2026 Roger Brown.
# Licensed under the MIT License.

$ErrorActionPreference = 'Stop'

trap
{
	throw $PSItem
}

"-- round trip a couple of PowerShell objects"

Invoke-AppleScript @'
on echo(payload)
	return payload
end echo
'@ echo -ArgumentList @{hash='table'}

Invoke-AppleScript @'
on echo(payload)
	return payload
end echo
'@ echo -ArgumentList ([pscustomobject]@{custom='object'})

Invoke-AppleScript @'
on echo(payload)
	return payload
end echo
'@ echo -ArgumentList (Get-Date) | Select-Object -ExpandProperty DateTime

"-- load a script and pass an argument"

Invoke-AppleScript ([System.IO.FileInfo]"$PSScriptRoot/test.applescript") echo "Hello World"

"-- use Uri to identify script and perform calculation"

Invoke-AppleScript ([Uri]"file://$PSScriptRoot/test.applescript") add 2,2

"-- show an error as a warning"

try
{
	Invoke-AppleScript foo
}
catch
{
	Write-Warning $_.Exception.Message
}

"-- pipe parameter and invoke a handler"

"Hello World" | Invoke-AppleScript -FileInfo "$PSScriptRoot/test.applescript" echo

"-- done"
