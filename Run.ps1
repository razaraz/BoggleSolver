
$ErrorActionPreference = "Stop"

Write-Host -ForegroundColor Blue "Verifying prerequisites"
$MSBuildVerRegKey = 'HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0'

if(Test-Path $MSBuildVerRegKey)
{
	Write-Host -ForegroundColor Green "`tFound MSBuild 14.0"
}
else
{
	Write-Error "MSBuild Version 14.0 is required to compile this project."
}

$MSBuildToolsPath = Get-ItemPropertyValue -Path $MSBuildVerRegKey -Name MSBuildToolsPath
$MSBuildExePath = Join-Path $MSBuildToolsPath 'MSBuild.exe'

if(Test-Path $MSBuildExePath)
{
	Write-Host -ForegroundColor Green "`tFound MSBuild exe at $MSBuildExePath"
}
else
{
	Write-Error "Could not find MSBuild executable in path $MSBuildExePath"
}

if($env:VS140COMNTOOLS)
{
	Write-Host -ForegroundColor Green "`tFound Visual Studio 2015 Common Tools"
}
else
{
	Write-Error "Visual Studio 2015 is required to compile this project."
}

$MSTestExePath = Join-Path $env:VS140COMNTOOLS "..\IDE\MSTest.exe" 
if(Test-Path $MSTestExePath)
{
	Write-Host -ForegroundColor Green "`tFound MSTest exe at $MSTestExePath"
}
else
{
	Write-Error "Could not find MSTest executable in path $MSTestExePath"
}

Write-Host -ForegroundColor Blue "Prerequisites verified"

# Compile
Write-Host -ForegroundColor Blue "Compiling project"
$SolutionName = "Boggle.sln"
$SolutionPath = Join-Path $PSScriptRoot $SolutionName
$Configuration = "Release"
$Platform = "x64"

Write-Host -ForegroundColor DarkBlue "`tInvoking MSBuild for:`n`t`tSolution: $SolutionName`n`t`tConfiguration: $Configuration`n`t`tPlatform: $Platform"
$MSBuildArgs = [string[]]@(
							$SolutionPath,
							"/property:Configuration=$Configuration",
							"/property:Platform=$Platform",
							"/verbosity:quiet",
							"/noconsolelogger",
							"/fileLogger1",
							"/fileloggerparameters1:LogFile=BuildErrors.log;ErrorsOnly",
							"/fileLogger2",
							"/fileloggerparameters2:LogFile=BuildDetails.log;Verbosity=detailed",
							"/fileLogger3",
							"/fileloggerparameters3:LogFile=BuildSummary.log;Verbosity=quiet;Summary;PerformanceSummary")

$MSBuildProc = Start-Process -FilePath $MSBuildExePath -ArgumentList $MSBuildArgs -WindowStyle Hidden -PassThru
$MSBuildProc.WaitForExit();

if($MSBuildProc.ExitCode -ne 0)
{
	Write-Error "MSBuild Encountered an error during compilation. For more information refer to BuildErrors.log"
}

Write-Host -ForegroundColor Blue "Compilation Successful."

# Run Unit Tests
Write-Host -ForegroundColor Blue "Running Unit Tests."
$UnitTestAssembly = "BoggleTests\bin\Release\BoggleTests.dll"
$MSTestArgs = [string[]]@(
							"/testcontainer:$UnitTestAssembly"
)
$MSTestProc = Start-Process -FilePath $MSTestExePath -ArgumentList $MSTestArgs -PassThru

$MSTestProc.WaitForExit();
echo "MSTest Exit code $($MSTestProc.ExitCode)"
# Parse MSTest Report
# Generate Report Summary