@echo off
setlocal enabledelayedexpansion
set FileNames=
for %%i in (*.js) do (
	if "!FileNames!" == "" (
		set FileNames=%%i
	) else (
		set FileNames=!FileNames!+%%i
	)
)
copy /b !FileNames! "../eruru.mvvm.js"