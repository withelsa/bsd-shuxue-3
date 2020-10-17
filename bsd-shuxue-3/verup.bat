@ECHO OFF

CD /D "%~dp0"
set dt=%date:~0,10%
set ts=%time:~0,8%
set build_year=%date:~0,4%
set build_month=%date:~5,2%
set build_day=%date:~8,2%

ECHO ���±���ʱ���...
SET "BUILD_VERSION_FILE=.\BuildVersion.cs"
ECHO. DEL /F %BUILD_VERSION_FILE%
DEL /F %BUILD_VERSION_FILE%
ECHO. namespace bsd_shuxue_3 >> %BUILD_VERSION_FILE%
ECHO. { >> %BUILD_VERSION_FILE%
ECHO. 	internal class BuildVersion >> %BUILD_VERSION_FILE%
ECHO. 	{ >> %BUILD_VERSION_FILE%
ECHO. 		public const string BUILD_VERSION = "%dt% %ts%"; >> %BUILD_VERSION_FILE%
ECHO. 	} >> %BUILD_VERSION_FILE%
ECHO. } >> %BUILD_VERSION_FILE%

ECHO ���°汾��Ϣ ...
SET "ASSEMBLY_INFO_FILE=.\Properties\AssemblyInfo.cs"
SET "ASSEMBLY_INFO_TEMPLATE=.\Properties\AssemblyInfo.tmpl"
ECHO. DEL /F %ASSEMBLY_INFO_FILE%
DEL /F %ASSEMBLY_INFO_FILE%
ECHO. COPY /Y %ASSEMBLY_INFO_TEMPLATE% %ASSEMBLY_INFO_FILE%
COPY /Y %ASSEMBLY_INFO_TEMPLATE% %ASSEMBLY_INFO_FILE%
ECHO. >> %ASSEMBLY_INFO_FILE%
ECHO. [assembly: AssemblyTitle("��ʦ����ѧ���꼶 (%dt% %ts%)")] >> %ASSEMBLY_INFO_FILE%
ECHO. [assembly: AssemblyDescription("��ʦ����ѧ���꼶 (%dt% %ts%)")] >> %ASSEMBLY_INFO_FILE%
ECHO. [assembly: AssemblyVersion("1.0.%build_year%.%build_month%%build_day%")] >> %ASSEMBLY_INFO_FILE%
ECHO. [assembly: AssemblyFileVersion("1.0.%build_year%.%build_month%%build_day%")] >> %ASSEMBLY_INFO_FILE%

pause