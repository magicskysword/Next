# [English ReadMe Click Here](ReadmeEn.md)

# 觅长生 Next Mod框架

![Next](preview.png)

觅长生 Next Mod是基于BepinEx框架的Mod，为觅长生游戏提供了数据增量修改、添加剧情与触发器的功能

github地址：https://github.com/magicskysword/Next

## 功能与作用

可以通过编写json文件，以在游戏里插入额外的功能与数据。

## 使用方法

从Steam创意工坊安装：[创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2824845357)

## **Next Mod 制作文档**

* [文档附件](doc/Next文档.md) （保持最新）
* [Bilibili Wiki](https://wiki.biligame.com/mcs/Next%E9%A6%96%E9%A1%B5)
* [Fandom Wiki](https://michangshengnext.fandom.com/zh/wiki/%E8%A7%85%E9%95%BF%E7%94%9FNext_Wiki) （停止更新）

在游戏内可以通过 F4 打开mod菜单（按键可修改）

## 鸣谢

3dm  宵夜97  BepinEx开发教程 `<br>`
3dm  ゞ残月﹎_|  Villain Mod的框架思路 `<br>`
@玄武 赞助者

## 下载地址

在Steam创意工坊或游戏内创意工坊，搜索Next并订阅

* 以下方式已经停止更新

[Github Release](https://github.com/magicskysword/Next/releases/latest)

[3DM 论坛](https://bbs.3dmgame.com/thread-6207429-1-1.html)

[3DM Mod站](https://mod.3dmgame.com/mod/178805)

## Build

### 准备工作

1. 从Steam下载最新版本的觅长生
2. 从创意工坊订阅BepinEx

### Mod构建

1. 添加环境变量
   * 打开 此电脑 -> 属性 -> 高级系统设置
   * 切换到【高级】页签
   * 打开环境变量
   * 在用户变量栏点击新建：
     * 变量名为：McsPath
     * 变量值为游戏地址，如：D:\Steam\steamapps\common\觅长生
2. 打开解决方案，选择Next项目
3. 编译Next项目
4. 在编译完成后，文件会自动被复制到 游戏目录：
5. 打开游戏，进行测试

## 基于Next开发Mod

Next插件范例：https://github.com/magicskysword/NextExamplePlugin

## 使用库

[codingseb/ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator) 用于事件与触发器的条件判断

[xiaoye97/VRoidXYTool](https://github.com/xiaoye97/VRoidXYTool) 使用了其中的GUI库

[Tencent/xLua](https://github.com/Tencent/xLua) 嵌入lua脚本

[rxi/json.lua](https://github.com/rxi/json.lua) lua json库

[fairygui/FairyGUI-unity](https://github.com/fairygui/FairyGUI-unity) FairyGUI运行时库

## 相关库

[BepinEx](https://github.com/BepInEx/BepInEx) 本mod基于BepinEx

## 许可证

[MIT许可证](https://github.com/magicskysword/Next/blob/main/Licenses/NextLICENSE)
