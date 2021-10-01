# 觅长生 Next Mod框架

觅长生 Next Mod是基于BepinEx框架的Mod，为觅长生游戏提供了数据增量修改、添加剧情与触发器的功能

## 功能与作用
可以通过编写json文件，以在游戏里插入额外的功能与数据。

## 使用方法

将 `Next.Dll` 与 `Microsoft.CSharp.dll` (本工程提供的版本) 置入 `觅长生\BepInEx\plugins` 文件夹

参考贴：https://bbs.3dmgame.com/thread-6207429-1-1.html

Mod WiKi：[Next Wiki](https://michangshengnext.fandom.com/zh/wiki/%E8%A7%85%E9%95%BF%E7%94%9FNext_Wiki)

文档附件：[Next文档](doc/Next文档.md)

在游戏内可以通过 F4 打开mod菜单（按键可修改）

## 鸣谢
3dm  宵夜97  BepinEx开发教程<br>
3md  ゞ残月﹎_|  Villain Mod的框架思路<br>

## 使用库
[ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator) 用于事件与触发器的条件判断

## 相关库
[BepinEx](https://github.com/BepInEx/BepInEx) 本mod基于BepinEx

## 许可证
MIT 许可证