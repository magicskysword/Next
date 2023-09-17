# Immortal Way Of Life Next Mod Framework

![Next](preview.png)

Next Mod is a Mod based on the BepinEx framework. It provides the ability to change data incrementally, add story and trigger to the game

github url：https://github.com/magicskysword/Next

## Function

You can write JSON files to insert additional functionality and data into the game.

## How To Use

Download the package and decompress it to `觅长生\BepInEx\plugins` folder

Reference Post(Chinese)：https://bbs.3dmgame.com/thread-6207429-1-1.html

Wiki(Chinese)：[Next Wiki](https://michangshengnext.fandom.com/zh/wiki/%E8%A7%85%E9%95%BF%E7%94%9FNext_Wiki)

Document(Chinese)：[Next Document](doc/Next文档.md)

The mod menu can be opened through F4 in the game (the keys can be modified)

## Credits

3dm  宵夜97  BepinEx tutorial`<br>`
3dm  ゞ残月﹎_|  Villain mod framework ideas`<br>`
@玄武 Sponsor

## Download

[Github Release](https://github.com/magicskysword/Next/releases/latest)
[3DM BBS](https://bbs.3dmgame.com/thread-6207429-1-1.html)
[3DM Mod Site](https://mod.3dmgame.com/mod/178805)

## Build

### Preparation

1. Download the latest game version from Steam
2. Subscribe to BepinEx from Steam Workshop

### Mod build

1. Add environment variables

   * Open this computer -> Attribute -> Advanced System Settings
   * Switch to the Advanced TAB
   * Open Environment Variables
   * Creaate a new variable

     * Variable name: McsPath
     * The variable value is the game address, such as: D:\Steam\steamapps\common\觅长生
2. Open the solution and select the Next project
3. Compile the Next project
4. After compiling, the file will be automatically copied to the game directory
5. Open the game and test it

## Develop mods based on Next

Next Plugin Example：https://github.com/magicskysword/NextExamplePlugin

## Use Repo

[codingseb/ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator) Conditions for events and triggers

[xiaoye97/VRoidXYTool](https://github.com/xiaoye97/VRoidXYTool) The GUI library is used

[Tencent/xLua](https://github.com/Tencent/xLua) Emitted lua script

[rxi/json.lua](https://github.com/rxi/json.lua) lua json lib

[fairygui/FairyGUI-unity](https://github.com/fairygui/FairyGUI-unity) FairyGUI runtime library

## Related Repo

[BepinEx](https://github.com/BepInEx/BepInEx) this mod based BepinEx

## Licence

[MIT Licence](https://github.com/magicskysword/Next/blob/main/LICENSE)
