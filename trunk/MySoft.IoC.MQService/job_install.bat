@Echo starting to installing "MySoft.IoC.MQService"

@Set Path=%ProgramFiles%\Microsoft Visual Studio 8\SDK\v2.0\Bin;%windir%\Microsoft.NET\Framework\v2.0.50727;%programFiles%\Microsoft Visual Studio 8\VC\bin;c:\Program Files\Microsoft Visual Studio 8\Common7\IDE;%ProgramFiles%\Microsoft Visual Studio 8\VC\vcpackages;%PATH%
@Set LIB=%ProgramFiles%\Microsoft Visual Studio 8\VC\lib;%ProgramFiles%\Microsoft Visual Studio 8\SDK\v2.0\Lib;%LIB%
@Set INCLUDE=%ProgramFiles%\Microsoft Visual Studio 8\VC\include;%ProgramFiles%\Microsoft Visual Studio 8\SDK\v2.0\include;%INCLUDE%
@Set NetSamplePath=%ProgramFiles%\Microsoft Visual Studio 8\SDK\v2.0
@Set VCBUILD_DEFAULT_CFG=Debug^|Win32
@Set VCBUILD_DEFAULT_OPTIONS=/useenv
@echo Setting environment to use Microsoft .NET Framework v2.0 SDK tools.
@echo For a list of SDK tools, see the 'StartTools.htm' file in the bin folder.

installutil MySoft.IoC.MQService.exe /install

@Echo ended to installing "MySoft.IoC.MQService"

pause