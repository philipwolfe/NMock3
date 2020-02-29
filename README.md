# NMock3
An archive of the final state of NMock3

# Project Description
NMock3 builds on the work of NMock2. Specifically it adds lambda expressions to the matcher syntax. You can use lambda expressions instead of strings, making refactoring possible. The idea of Syntactic Sugar is maintained in that all expectations read like an English sentence.

#To get started with NMock3:

1. Download the binaries from the Downloads page.
1. Review the tutorials to see how NMock3 works.
1. Check out the new Cheat Sheet.
1. Check out the new API Docs.
1. Provide feedback on this site.

## We now have tutorials! Please check them out in the code.

# FAQ
## Is NMock3 backward compatible with NMock2?
The version titled 'NMock3 - RC1, .NET 4.0' on the download page is completely backward compatible. In fact, you can mix syntax within your unit tests.
## Why is the namespace NMock2 and the assembly named NMock3 of the download titled 'NMock3 - RC1, .NET 4.0'?
Typically version numbers are discouraged from being included in namespaces. We decided not to perpetuate the practice of including the version number 3 in the namespace and left it at 2. This maintains backward compatibility with all existing tests. The newest RTM versions are just NMock.
## What does the version number mean?
The version number is read as Major.Minor.Build.Framework where "Framework" is the version of the .NET framework to which the assembly is compiled. 35 = .NET 3.5, 40 = .NET 4.0
## Why another mocking framework (or the enhancement of an existing one)?
Several reasons:

1. NMock has a history of being very easy to read and understand. If it is easy to understand, developers are more likely to adopt it.
1. NMock doesn't rely on the record/replay requirements as other mocking frameworks do.
1. NMock can set up event binding expectations. (Other frameworks, such as Moq, couldn't do this at the time I looked at them)
1. NMock is now using lambdas which support code refactoring and compile-time checking!
