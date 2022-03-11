# Immortal Way Of Life Next Mod Framework

Next Mod is a Mod based on the BepinEx framework. It provides the ability to change data incrementally, add story and trigger to the game

## Function
You can write JSON files to insert additional functionality and data into the game.

## How To Use
Download the package and decompress it to `觅长生\BepInEx\plugins` folder

Reference Post(Chinese)：https://bbs.3dmgame.com/thread-6207429-1-1.html

Wiki(Chinese)：[Next Wiki](https://michangshengnext.fandom.com/zh/wiki/%E8%A7%85%E9%95%BF%E7%94%9FNext_Wiki)

Document(Chinese)：[Next Document](doc/Next文档.md)

The mod menu can be opened through F4 in the game (the keys can be modified)

## Credits
3dm  宵夜97  BepinEx tutorial<br>
3dm  ゞ残月﹎_|  Villain mod framework ideas<br>

## Build

Clone the library, add the Dll references in the game folder, including

`觅长生\BepInEx\core`
```
0Harmony.dll
BepInEx.dll
```
`觅长生\觅长生_Data\Managed`
```
Assembly-CSharp.dll
Assembly-CSharp-firstpass.dll
Newtonsoft.CSharp.dll
UnityEngine.dll
UnityEngine.AssetBundleModule.dll
UnityEngine.AudioModule.dll
UnityEngine.CoreModule.dll
UnityEngine.IMGUIModule.dll
UnityEngine.InputModule.dll
UnityEngine.TextRenderingModule.dll
UnityEngine.UI.dll
UnityEngine.UIModule.dll
UnityEngine.UnityWebRequestAssetBundleModule.dll
UnityEngine.UnityWebRequestAudioModule.dll
UnityEngine.UnityWebRequestModule.dll
UnityEngine.UnityWebRequestTextureModule.dll
UnityEngine.UnityWebRequestWWWModule.dll
```

After adding them, Build them directly and place the `Next.Dll` file, `NextLib` and `NextConfig` folders into the `觅长生\BepInEx\plugins` directory

## Develop mods based on Next
Next Plugin Example：https://github.com/magicskysword/NextExamplePlugin

## Use Repo
[codingseb/ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator) Conditions for events and triggers

[xiaoye97/VRoidXYTool](https://github.com/xiaoye97/VRoidXYTool) The GUI library is used

## Related Repo
[BepinEx](https://github.com/BepInEx/BepInEx) this mod based BepinEx

## Licence
[MIT Licence](https://github.com/magicskysword/Next/blob/main/LICENSE)
