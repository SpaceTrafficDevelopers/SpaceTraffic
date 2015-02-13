@echo off
rem Skript zjistí, zda v projektu je upravený konfiguraèní soubor, 
rem který zkopíruje do build adresáøe. 
rem Pøípadnì zkopíruje defualt z hlavního adresáøe projektu.

rem Použití: copyconfig %1 %2 
rem %1 - jméno konfiguraèního souboru bez pøípony.
rem %2 - konstanta ${ProjectDir} ve Visual Studiu
rem %3 - konstanta $(TargetDir) ve Visual Studiu

set custom=%~2.config\%~1.config
set default=%~2%~1.config.default
set target=%~3%~1.config

echo Custom: %custom%
echo Default: %default%
echo Target: %target%

if exist "%custom%" goto COPYCUSTOM

:COPYDEFAULT

echo Copying default config for %1
copy "%default%" "%target%" /y & goto END

:COPYCUSTOM

echo Copying custom config for %1
copy "%custom%" "%target%" /y & goto END

:END
echo Done.