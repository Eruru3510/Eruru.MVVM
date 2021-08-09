@echo off
del /s /q "../../../Unity/Assets/Eruru.MVVM/Scripts/Shared"
rd /s /q "../../../Unity/Assets/Eruru.MVVM/Scripts/Shared"
xcopy "Shared" "../../../Unity/Assets/Eruru.MVVM/Scripts/Shared\" /s/y