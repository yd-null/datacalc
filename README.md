# DateCalc

A date arithmetic extension for the PowerToys Command Palette on Windows 11.

## Usage

Open **Date Calculator** in Command Palette, then enter an expression:

```text
today + 30d
tomorrow - 2w
2026-08-17 + 1m
17/08/2026 - 1y
```

Dates use the current Windows regional format. ISO `yyyy-MM-dd` dates always work. The aliases `today`, `tomorrow`, and `yesterday` are also accepted.

| Unit | Meaning |
| --- | --- |
| `d` | days |
| `w` | weeks |
| `m` | calendar months |
| `y` | calendar years |

Press Enter on a result to copy it in the current Windows short-date format.

## Development

The parser is platform-neutral and can be tested anywhere with the .NET 10 SDK:

```shell
dotnet test DateCalc.Core.Tests/DateCalc.Core.Tests.csproj
```

Building and deploying the extension requires Windows 11, PowerToys, Developer Mode, and Visual Studio 2022 with the WinUI/Windows App SDK workloads:

1. Open `DateCalc.sln` and select `x64` or `ARM64`.
2. Use **Build > Deploy DateCalc**. Running the unpackaged profile does not deploy the app extension.
3. Open Command Palette and run **Reload Command Palette extensions**.
4. Open **Date Calculator**.

## Releases

Pushing a version tag such as `v0.1.0` runs the Windows release workflow and creates x64 and ARM64 installers in a GitHub Release.

For discovery in Command Palette, submit those installer URLs to WinGet and include this tag in each locale manifest:

```yaml
Tags:
- windows-commandpalette-extension
```

The WinGet installer manifest must also declare the matching Windows App Runtime dependency. A GitHub Release alone can be installed manually, but it is not discoverable from Command Palette.
