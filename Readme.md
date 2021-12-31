# 觅长生 Next Mod框架

觅长生 Next Mod是基于BepinEx框架的Mod，为觅长生游戏提供了数据增量修改、添加剧情与触发器的功能

## 功能与作用
可以通过编写json文件，以在游戏里插入额外的功能与数据。

## 使用方法

将 `Next.Dll` 与 `NextLib`文件夹 置入 `觅长生\64\BepInEx\plugins` 文件夹 (如果是32位系统则为 `觅长生\64\BepInEx\plugins`，下面不再赘述)

参考贴：https://bbs.3dmgame.com/thread-6207429-1-1.html

Mod WiKi：[Next Wiki](https://michangshengnext.fandom.com/zh/wiki/%E8%A7%85%E9%95%BF%E7%94%9FNext_Wiki)

文档附件：[Next文档](doc/Next文档.md)

在游戏内可以通过 F4 打开mod菜单（按键可修改）

## 鸣谢
3dm  宵夜97  BepinEx开发教程<br>
3md  ゞ残月﹎_|  Villain Mod的框架思路<br>

## Build
clone该库后，自行重新添加觅长生游戏文件夹里的Dll引用，包括

`觅长生\64\BepInEx\core` 里的
```
0Harmony
BepInEx
```
`觅长生\64\觅长生_Data\Managed` 里的
```
Assembly-CSharp
Assembly-CSharp-firstpass
Newtonsoft.CSharp
UnityEngine
UnityEngine.AudioModule
UnityEngine.CoreModule
UnityEngine.IMGUIModule
UnityEngine.InputModule
UnityEngine.TextRenderingModule
UnityEngine.UI
UnityEngine.UIModule
```

添加完后直接Build即可，将Build出来的 `Next.Dll` 与 `NextLib`文件夹 置入 `觅长生\64\BepInEx\plugins` 文件夹即可

## 使用库
[codingseb/ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator) 用于事件与触发器的条件判断

## 相关库
[BepinEx](https://github.com/BepInEx/BepInEx) 本mod基于BepinEx

## 许可证
[MIT许可证](https://github.com/magicskysword/Next/blob/main/LICENSE)