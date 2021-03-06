###############################################################################
# File: Run-Boggle.ps1
# Author: Ram�n Zaraz�a B.
# Date: Jul-14-2016
###############################################################################

$BoggleDllName = "BoggleManaged.dll"
$OutPath = Join-Path "$PSScriptRoot" "BoggleManaged\bin\Release"
$BoggleDllPath = Join-Path $OutPath $BoggleDllName

if(!(Test-Path $BoggleDllPath))
{
    Write-Host -ForegroundColor Blue "Did not find the Boggle Dll.`nBuilding..."
    
    try
    {
        & "$PSScriptRoot\Start-BoggleBuild.ps1"
    }
    catch
    {
        Write-Host -ForegroundColor DarkMagenta "Error found while building the Dll. Aborting."
        Write-Host -ForegroundColor DarkMagenta $Error[0].Exception.Message
        return
    }
}

try
{
    Write-Host -ForegroundColor Blue "Loading Boggle managed dll into powershell."
    [System.Reflection.Assembly]::LoadFile($BoggleDllPath)
}
catch
{
    Write-Host -ForegroundColor DarkMagenta "Could not load boggle assembly into powershell."
    Write-Host -ForegroundColor DarkMagenta $Error[0].Exception.Message
    return
}

# Load in windows forms to pop up a file dialog box
[System.Reflection.Assembly]::LoadWithPartialName("System.windows.forms") | Out-Null

Write-Host -ForegroundColor Blue "Loading success.`n`n`n`n"

$Play = $true
[System.Console]::ForegroundColor = "DarkGreen"
[System.Console]::Title = "Boggle"

Write-Host "Welcome to boggle!`n"
Write-Host "Boards can have a maximum of 64 tiles(characters)."
Write-Host "Unicode characters are accepted"
Write-Host "Have fun!`n`n"

while($Play)
{
    Write-Host
    Write-Host "New board"
    Write-Host "Enter the tiles for a new board. An empty line to finish."

    $Board = ""
    $Width = 0;
    $Height = 0;

    do
    {
        $input = Read-Host

        if(!$input)
        {
            break
        }

        if(!$Width)
        {
            $Width = $input.Length
        }

        if($input.Length -ne $Width)
        {
            Write-Host -ForegroundColor Magenta "Width does not match the width of the board!"
        }
        else
        {
            $Board += $input
            ++$Height
        }
    } while($true);

    if($Width * $Height -gt 64)
    {
        Write-Host -ForegroundColor Magenta "That board is too big!"
        continue
    }

    $boggleBoard = New-Object BoggleManaged.Boggle($Width, $Height, $Board)

    Write-Host "`nBoard:"
    $boggleBoard.ToString()



    $OpenFileDialog = New-Object System.Windows.Forms.OpenFileDialog
    $OpenFileDialog.initialDirectory = Join-Path $PSScriptRoot "EnglishDictionaries"
    $OpenFileDialog.filter = "Text files (*.txt) | *.txt"
    $OpenFileDialog.ShowDialog() | Out-Null
    $OpenFileDialog.Title = "Select a dictionary to use for solving this board"


    $Dictionary = $OpenFileDialog.filename
    $CaseSensitive = $false
    $MinWordLength = [Math]::Min(4, $boggleBoard.Board.Length)

    Write-Host "Solving using the following parameters:"
    Write-Host "  Dictionary:"
    Write-Host "    $Dictionary"
    Write-Host "  CaseSensitive: $CaseSensitive"
    Write-Host "  MinWordLength: $MinWordLength"

    $Solutions = $boggleBoard.Solve($Dictionary, $MinWordLength, $CaseSensitive)

    Write-Host "Solutions found:"
    Write-Host -ForegroundColor Blue $Solutions

    Write-Host
    Write-Host "Play again?[y/n]" -NoNewline

    $Play = [System.Console]::ReadKey().Key -eq [System.ConsoleKey]::Y
    Write-Host
}


Write-Host -ForegroundColor Green "Thank you for playing!"
[System.Console]::ResetColor()

Read-Host
