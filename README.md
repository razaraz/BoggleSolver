# Managed Boggle Solver
Ramón Zarazúa B.

A basic implementation of a boggle solver written for .NET

This implementation was made for a coding test, and I'm making it freely
available under the MIT license. 

## Requirements
Windows 64-bit
MSBuild 14.0
Visual Studio 2015 (MSTest.exe)
.NET Framework 4.5.2(tested)

## How to run
Invoke Run.bat to start. This will invoke the powershell user interface script Run-Boggle.ps1
This script verifies that the solution has been compiled, and then loads the assembly
to start the solver for the user.

The build script performs the following actions:
  - Verify the prerequisites
  - Build the project
  - Run Tests
  - Load the test result file
  - Display a summary 

## Algorithm

  The possible solutions in a boggle board grow exponentially. This algorithm is therefore
dictionary-based since the possibility space tends to be much smaller. The dictionary is parsed
and read into a tree with each node representing one letter. All the children of that node
contain letters that can follow the node path to form a complete word. When building the
dictionary tree, words are filtered out if they do not meet the minimum length requirements,
or cannot possibly be spelled using the tiles present in the board.

  To solve the board the dictionary tree is traversed recursively. When visiting each node,
we find out all the tiles that contain this letter, and if they are valid for the path. This
is calculated using bitfields for efficiency, which restricts the board size to 64 tiles.
If a word can be formed at this point in the tree, we have found a match. The algorithm
then recurses to all the child nodes if any. Finally, Nodes that have already been matched,
and all their children have been matched get removed from the tree to avoid duplicate work.
  
## Improvement opportunities
There are many opportunities for improvement of this program:

  - Read dictionary by blocks rather than line
      Reading 4KiB blocks at a time and manually processing the data is much faster,
      and takes advantage of read block sizes used by the hardware directly
  - Asynchronous dictionary loading
      Dictionary loading is done serially. A good parallelization
      opportunity is to have one thread for each root node in the tree, therefore not
      requiring any synchronization.
  - Take advantage of sorted dictionary
      Since the dictionaries are presorted, we could mantain references to previously visited
      nodes, and save words faster.
  - Asynchronous tree traversal when solving the board
      Just as when loading the dictionary, we can parallelize the solving algorithm by tree root
      node.
  - More efficient bitfield to index array
      We can precalculate 256 bit patterns to indexes into an arry, and use them to get the bitfield
      indices much faster than doing a loop.

