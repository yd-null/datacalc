#ifndef MyAppVersion
  #define MyAppVersion "0.1.0"
#endif
#ifndef Architecture
  #define Architecture "x64"
#endif
#ifndef SourceDir
  #define SourceDir "publish"
#endif
#if Architecture == "x64"
  #define InnoArchitecture "x64os"
#else
  #define InnoArchitecture Architecture
#endif

#define MyAppName "DateCalc"
#define MyAppPublisher "yd-null"
#define MyAppURL "https://github.com/yd-null/datacalc"
#define MyAppCLSID "432EB8D3-949A-4F4D-AAF5-893ED3AEDB8A"
#define MyAppId "{{" + MyAppCLSID + "}"

[Setup]
AppId={#MyAppId}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
OutputDir=Installer
OutputBaseFilename={#MyAppName}_{#MyAppVersion}_{#Architecture}
ArchitecturesAllowed={#InnoArchitecture}
ArchitecturesInstallIn64BitMode={#InnoArchitecture}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Registry]
Root: HKCU; Subkey: "Software\Classes\CLSID\{#MyAppId}"; ValueType: string; ValueName: ""; ValueData: "Date Calculator"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\CLSID\{#MyAppId}\LocalServer32"; ValueType: string; ValueName: ""; ValueData: """{app}\DateCalc.exe"" -RegisterProcessAsComServer"; Flags: uninsdeletekey

[UninstallDelete]
Type: filesandordirs; Name: "{app}"
