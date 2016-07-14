###############################################################################
# File: Start-BoggleBuild.ps1
# Author: Ramón Zarazúa B.
# Date: Jul-14-2016
###############################################################################


$ErrorActionPreference = "Stop"

###############################################################################
#                             Verification                                    #
###############################################################################
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

###############################################################################
#                                Compiling                                    #
###############################################################################
Write-Host -ForegroundColor Blue "Compiling project"
$SolutionName = "Boggle.sln"
$SolutionPath = Join-Path $PSScriptRoot $SolutionName
$Configuration = "Release"
$Platform = "x64"

Write-Host -BackgroundColor DarkBlue "`tInvoking MSBuild for:`n`t`tSolution: $SolutionName`n`t`tConfiguration: $Configuration`n`t`tPlatform: $Platform"
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

###############################################################################
#                                   Test                                      #
###############################################################################
Write-Host -ForegroundColor Blue "Running Unit Tests."

$UnitTestAssembly = "BoggleTests\bin\Release\BoggleTests.dll"

function Run-MSTest($TestContainer, $Category)
{ 
	$MSTestArgs = [string[]]@(
								"/testcontainer:$UnitTestAssembly"
	)

	if($Category)
	{ 
		$MSTestArgs += "/category:Interface"
	}

	Write-Host -BackgroundColor DarkBlue "`tInvoking MSTest for:`n`t`tTestContainer: $TestContainer`n`t`tCategory: Category"
	$MSTestProc = Start-Process -FilePath $MSTestExePath -ArgumentList $MSTestArgs -PassThru -WindowStyle Hidden

	$MSTestProc.WaitForExit();

	$ResultsFile = Get-ChildItem "$PSScriptRoot\TestResults\" -File | Sort-Object CreationTime | Select-Object -Last 1 -ExpandProperty FullName
	if(!(Test-Path $ResultsFile))
	{
		Write-Error "Could not find the results trx file"
	}
	
	# Parse trx, and display summary
	$Results = [xml](Get-Content $ResultsFile)

	Write-Host -ForegroundColor DarkGreen "Test run completed. Results:"
	$Results.TestRun.ResultSummary.Counters | select executed, passed, error, failed, timeout | Format-Table | Out-String | Write-Host -ForegroundColor DarkGreen
	"Duration: {0}" -f ([datetime]$Results.TestRun.Times.finish - [datetime]$Results.TestRun.Times.start).ToString() | Write-Host -ForegroundColor DarkGreen

	if($MSTestProc.ExitCode)
	{
		start $ResultsFile
		Write-Error "Tests Failed. Opening up the results file in Visual Studio."
	}
}

Run-MSTest -TestContainer $UnitTestAssembly -Category "Interface"
Run-MSTest -TestContainer "BoggleTests\bin\Release\FunctionalTests.orderedtest"
Run-MSTest -TestContainer $UnitTestAssembly -Category "Performance"

Write-Host -ForegroundColor Green "Build and testing success. Press any key to continue."

[System.Console]::ReadKey()
