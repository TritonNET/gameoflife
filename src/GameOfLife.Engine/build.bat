@echo off
setlocal enableextensions enabledelayedexpansion

REM build.bat — CMake driver (parentheses fully escaped)

REM Defaults
set "PRESET=x64-windows"
set "CONFIG="
set "DO_CLEAN=0"
set "COPY_TO="

REM Parse args
:parse
if "%~1"=="" goto parsed

if /I "%~1"=="/clean" (
  set "DO_CLEAN=1"
  shift
  goto parse
)

if /I "%~1"=="/copy" (
  set "COPY_TO=%~2"
  shift
  shift
  goto parse
)

if not defined PRESET_SEEN (
  set "PRESET=%~1"
  set "PRESET_SEEN=1"
  shift
  goto parse
)

if not defined CONFIG (
  set "CONFIG=%~1"
  shift
  goto parse
)

shift
goto parse

:parsed
echo.
echo ================= Build Plan =================
echo   Preset : %PRESET%
if defined CONFIG (
  echo   Config : %CONFIG%
) else (
  echo   Config : ^(use preset default^)
)
echo   Clean  : %DO_CLEAN%
if defined COPY_TO (
  echo   CopyTo : %COPY_TO%
) else (
  echo   CopyTo : ^(none^)
)
echo ==============================================
echo.

REM Ensure cmake exists
where cmake >nul 2>&1
if errorlevel 1 (
  echo ERROR: cmake not found on PATH.
  exit /b 1
)

REM Initialize VS dev env on Windows-ish presets
set "NEED_VSDEV=0"
echo %PRESET% | findstr /I "windows win32 win64 x64" >nul && set "NEED_VSDEV=1"
if "%NEED_VSDEV%"=="1" (
  set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
  if exist "%VSWHERE%" (
    for /f "usebackq tokens=* delims=" %%I in (`"%VSWHERE%" -latest -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath`) do (
      set "VSINSTALL=%%I"
    )
    if defined VSINSTALL (
      set "VSDEVCMD=%VSINSTALL%\Common7\Tools\VsDevCmd.bat"
      if exist "%VSDEVCMD%" (
        echo Initializing VS Dev environment...
        call "%VSDEVCMD%" -arch=x64 -host_arch=x64 >nul
      ) else (
        echo WARNING: VsDevCmd.bat not found at "%VSDEVCMD%".
      )
    ) else (
      echo WARNING: Visual Studio install not found via vswhere. Continuing anyway.
    )
  ) else (
    echo WARNING: vswhere.exe not found. Assuming environment is already set.
  )
)

REM Clean build dir for this preset if requested
set "BDIR=out\build\%PRESET%"
if "%DO_CLEAN%"=="1" (
  echo Cleaning "%BDIR%"...
  if exist "%BDIR%" rmdir /S /Q "%BDIR%"
)

REM Configure
if defined CONFIG (
  echo Configuring ^(preset=%PRESET%, CMAKE_BUILD_TYPE=%CONFIG%^)...
  cmake --preset "%PRESET%" -DCMAKE_BUILD_TYPE=%CONFIG%
) else (
  echo Configuring ^(preset=%PRESET%^)...
  cmake --preset "%PRESET%"
)
if errorlevel 1 (
  echo ERROR: CMake configure failed.
  exit /b 1
)

REM Build
echo Building ^(preset=%PRESET%^)...
cmake --build --preset "%PRESET%" --parallel
if errorlevel 1 (
  echo ERROR: Build failed.
  exit /b 1
)

REM Optional copy (Windows DLL convenience)
if defined COPY_TO (
  if not exist "%COPY_TO%" (
    echo ERROR: Copy target "%COPY_TO%" does not exist.
    exit /b 1
  )
  set "DLLPATH=%CD%\out\build\%PRESET%\gameoflife.dll"
  if exist "%DLLPATH%" (
    echo Copying "%DLLPATH%" to "%COPY_TO%" ...
    copy /Y "%DLLPATH%" "%COPY_TO%" >nul
  ) else (
    echo NOTE: "%DLLPATH%" not found ^(maybe static lib or non-Windows target^).
  )
)

echo.
echo SUCCESS: Build completed for preset "%PRESET%".
exit /b 0
