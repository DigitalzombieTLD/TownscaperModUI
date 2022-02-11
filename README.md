Townscaper - ModUI Utility mod - v0.8.0 by Digitalzombie
===========================================================

https://github.com/DigitalzombieTLD/TownscaperModUI/raw/master/ModUI.JPG

How to install:
===============

1. Download "Melon Loader" by LavaGang:
https://github.com/LavaGang/MelonLoader/releases/latest/

2. Start the MelonLoader.Installer.exe

2.1. Click "Select" and navigate to your Townscaper folder and select the Townscaper.exe (usually: C:\Program Files(x86)\Steam\steamapps\common\Townscaper\Townscaper.exe)

2.2. Untick the "Latest" checkbox and choose version 0.4.3

2.3. Click install 

2.4. During the installation a "Mods" folder gets created it your game folder. MelonLoader does not(!) change any game files. 
	 You can uninstall anytime through the installer or by deleting the "version.dll" file.

3. Download the mod (latest release) from: https://github.com/DigitalzombieTLD/TownscaperModUI/releases/latest/

4. Extract the all files from the "TownscaperModUI_Release_0.8.0.zip" into your games Mods folder

5. Start the game! 

===========================
===========================

What does it do?
=================

Utility mod for other mods. Does nothing on it's own.
Modders can add their mod controls to a unified UI for controls and saving/loading of settings.


How to use?
=================

- Place the "ModUI.dll" and "ModUI.unity3d" files in your mods folder
- Reference the "ModUI.dll" in your mod solution
- Usage examples can be found in the Townscaper template mod -> https://github.com/DigitalzombieTLD/TownscaperTemplate 


Features
=================

- Simple creation of different UI elements: Button, Slider, Text Inputfield, Toggle, Keybindings with customizable colors
- Simple saving and retreiving of values to a ini save file: Int, Float, Bool, String, Color32
- ini file gets created automatically, default values can be provided
- Settings are hold in RAM. Saving to file happens on closing of the ModUI sidepanel or manually via method call

===========================
===========================


Keep yourself up to date on the progress on:
https://www.youtube.com/c/DigitalzombieDev

Thanks to:
LavaGang for MelonLoader

ThirdParty  code:
Il2Cpp Asset Bundle Manager - LavaGang
https://github.com/LavaGang/UnityEngine.Il2CppAssetBundleManager
Licensed under the Apache License, Version 2.0

INI File Parser - Ricardo Amores Hernández
https://github.com/rickyah/ini-parser
Licensed under MIT License

Tween - Digital Ruby 
https://assetstore.unity.com/packages/tools/animation/tween-55983
Licensed under MIT License

Townscaper ModUI is licensed under Apache License, Version 2.0

===========================
===========================

Changelog:
==========
0.8.0	- First release
