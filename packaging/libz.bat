..\packages\LibZ.Bootstrap.1.1.0.2\tools\libz sign-and-fix --include ..\FortyOne.AudioSwitcher\bin\Release\*.dll --include ..\FortyOne.AudioSwitcher\bin\Release\*.exe --key ..\FortyOne.AudioSwitcher\fortyone.snk
..\packages\LibZ.Bootstrap.1.1.0.2\tools\libz inject-dll --assembly=..\FortyOne.AudioSwitcher\bin\Release\AudioSwitcher.exe --include=..\FortyOne.AudioSwitcher\bin\Release\*.dll --key ..\FortyOne.AudioSwitcher\fortyone.snk --move