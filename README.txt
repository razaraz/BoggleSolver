Bungie SDET Test
Ramón Zarazúa B.

--- Requirements ---
Windows 64-bit
MSBuild 14.0
Visual Studio 2015 (MSTest.exe)
.NET Framework 4.5.2

--- How to run ---
Run.bat will invoke the Powershell run script Run.ps1.
This script performs the following actions:
  - Verify the prerequisites
  - Build the project
  - Run Tests
  - Load the test result file
  - Display a summary 

--- Managed Boggle Solver ---

  FILL ME IN!!!!!!!!!!!!!!!!!!!!!
  !!!!!!!!!!!!!!!!!!!!!!!!!

--- Improvement opportunities ---
  - Make dictionary loading asynchronous
  - Make more efficient use of resources when loading dictionaries
  - Traverse dictionary tree in parallel asynchronously
  - Mark duplicates before adding
  - Read dictionary by blocks rather than line
  - Assume dictionaries are sorted, and traverse the tree along with reading the words
  XXXXXXXX
  - CHECK !Cache neighbor and children position maps maybe??
  - CHECK Save another tree backwards maybe to retrace
    a path we already took 
  XXXXXXXX

--- Unit Tests ---

The dictionaries I found and used for testing contained a few more words that could be formed
on the 3x3 board than the ones in the test description:
  abox
  boread
  daer
  rebox
  verby

  FILL ME IN!!!!!!!!!!!!!!!!!!!!!
  !!!!!!!!!!!!!!!!!!!!!!!!!
