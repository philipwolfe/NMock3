
Tutorial Setup:

1. The tutorials can use NUnit or MSTest.  When you open the project in Visual Studio,
ensure that the project builds.  You may have to fix some of your references.  

 - If you prefer to use NUnit:
   > Use NuGet (http://www.nuget.org) to get the NUnit package OR reference a local copy
   > Define a NUNIT Conditional Compilation symbol on the Build property page of the project
   > Delete the reference to Microsoft.VisualStudio.QualityTools.UnitTestFramework

 - If you prefer to use MSTest:
   > Reference the Microsoft.VisualStudio.QualityTools.UnitTestFramework assembly from the project
   > Ensure there are no Conditional Compilation symbols defined on the Build property page of the project

2. Get the NMock3 package from NuGet.  Use Visual Studio's Extension Manager to locate and install
the NuGet Package Manager.  After it is installed right-click on the project and select Manage NuGet
Packages.  On the left side of the dialog, select Online and then All.  Next Search for NMock3.  Click
the Install button beside the NMock3 package.

3. Verify that the project builds.