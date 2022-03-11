# Next Patch Mod 制作文档

## 1 什么是 Patch Mod
Patch Mod是对需要 Next插件Mod（简称Next） 进行加载的mod的称呼，以便与需要 BepinEx 加载的Mod进行区分。（Next本身需要BepinEx进行加载）

## 2 新建 Patch Mod
在 `觅长生/BepInEx/plugins/Next` 文件夹下，新建文件夹。<br/>
文件夹以 mod 开头，如 `mod测试`<br/>
只有以"mod"开头的文件夹能被读取。

## 3 Mod描述文件
Mod描述文件是用于描述mod名称、作者、版本与介绍。

该文件本身不影响mod加载，但是完善的mod描述文件有利于留下mod作者的信息，以及进行版本管理等。

### 3.1 创建描述文件

新建json文件，保存如 `mod测试/modConfig.json` 的路径中

文件结构：

modConfig.json
```json
{
    "Name" : "测试Mod",
    "Author" : "佚名",
    "Version" : "1.0.0",
    "Description" : "测试用的Mod。"
}
```

## 4 数据Patch
数据Patch是指对游戏原版数据进行修改与增加。（目前未提供删除功能）

### 4.1 数据类型
当Next安装后，运行游戏时，Next会自动在 `觅长生/BepInEx/plugins/Next` 路径生成Base文件夹

Base文件夹里的文件，大致分为三类：

1. 普通Json数据文件<br/>
    该类文件格式如下
    ```json
    {
        "1":{
            //......
        },
        "2":{
            //......
        }
    }
    ```
    对这类文件制作补丁时，只需要在mod文件夹里创建同名json文件，然后进行修改/新增数据即可。如：
    ```json
    {
        "2":{
            //......
        },
        "10":{
            //......
        }
    }
    ```
    与源数据ID一致的数据会进行替换，其余的数据则会新增到原版数据末尾

2. 字典json数据文件<br/>
    该类文件是放到文件夹里的特殊json文件，其文件名构成了数据的ID，如`ItemJsonData/`文件夹内的文件：<br/>
    `1.json`
    ```json
    {
        "id":1,
        "ItemIcon":0,
        "maxNum":1,
        "name":"锈渍铁剑",
        "TuJianType":0,
        "ShopType":99,
        "ItemFlag":[
            1,
            101
        ],
        "WuWeiType":0,
        "ShuXingType":0,
        "type":0,
        "quality":1,
        "typePinJie":1,
        "StuTime":0,
        "seid":[
            2
        ],
        "vagueType":0,
        "price":350,
        "desc":"主动：下一次造成的伤害提升25%（血量低于10时才能使用）",
        "desc2":"剑身满是锈渍，看起来连凡人用的武器都不如，魏无极的残魂便寄居在其中。",
        "CanSale":1,
        "DanDu":0,
        "CanUse":0,
        "NPCCanUse":0,
        "yaoZhi1":0,
        "yaoZhi2":0,
        "yaoZhi3":0,
        "wuDao":[]
    }
    ```
    要修改此类文件时，将该文件一份至mod文件夹里的`ItemJsonData`文件夹，然后修改其数据即可。<br/>
    如若需要新增数据，则将数据名称命名为新的数字，并修改数据内的ID与其一致即可。

3. SeidJson数据<br/>
    该类文件与类型1一样，但是其是放置在文件夹类的。目前版本以`SeidJsonData`结尾的文件夹皆属于该类。<br/>
    如 `BuffSeidJsonData/1.json` 文件，呈如下结构：
    ```json
    {
        "1":{
            "id":1,
            "value1":1
        },
        "2":{
            "id":2,
            "value1":-1
        },
        //......
    }
    ```
    需要在该类文件里修改或新增数据时，参考类型1的文件，在mod文件夹里新建 `BuffSeidJsonData/1.json`：
    ```json
    {
        "1":{
            "id":1,
            "value1":10
        },
        "666":{
            "id":666,
            "value1":1
        }
    }
    ```
    与源数据ID一致的数据会进行替换，其余的数据则会新增到原版数据末尾<br/>
    注：该类文件一般为游戏内功能函数的配置文件，例如buff数据里配置的seid会在buffSeid文件夹里寻找对应的json文件，然后在该文件内寻找该buffID对应的数据。

### 4.2 增量修改
Next 0.3.0 版本新增了增量修改的功能

当对源数据进行修改时，可以填写需要修改的部分。这样其他内容则不会被覆盖

如 `ItemJsonData/1.json`
```json
{
    "id":1,
    "ItemIcon":0,
    "maxNum":1,
    "name":"锈渍铁剑",
    "TuJianType":0,
    "ShopType":99,
    "ItemFlag":[
        1,
        101
    ],
    "WuWeiType":0,
    "ShuXingType":0,
    "type":0,
    "quality":1,
    "typePinJie":1,
    "StuTime":0,
    "seid":[
        2
    ],
    "vagueType":0,
    "price":350,
    "desc":"主动：下一次造成的伤害提升25%（血量低于10时才能使用）",
    "desc2":"剑身满是锈渍，看起来连凡人用的武器都不如，魏无极的残魂便寄居在其中。",
    "CanSale":1,
    "DanDu":0,
    "CanUse":0,
    "NPCCanUse":0,
    "yaoZhi1":0,
    "yaoZhi2":0,
    "yaoZhi3":0,
    "wuDao":[]
}
```

制造增量补丁时，可以写成如下格式：
```json
{
    "name":"魏老之剑",
    "desc2":"这就是传说中的修仙老爷爷，魏无极的残魂便寄居在其中。",
}
```

这样游戏里的最终数据便会变为如下：
```json
{
    "id":1,
    "ItemIcon":0,
    "maxNum":1,
    "name":"魏老之剑",
    "TuJianType":0,
    "ShopType":99,
    "ItemFlag":[
        1,
        101
    ],
    "WuWeiType":0,
    "ShuXingType":0,
    "type":0,
    "quality":1,
    "typePinJie":1,
    "StuTime":0,
    "seid":[
        2
    ],
    "vagueType":0,
    "price":350,
    "desc":"主动：下一次造成的伤害提升25%（血量低于10时才能使用）",
    "desc2":"这就是传说中的修仙老爷爷，魏无极的残魂便寄居在其中。",
    "CanSale":1,
    "DanDu":0,
    "CanUse":0,
    "NPCCanUse":0,
    "yaoZhi1":0,
    "yaoZhi2":0,
    "yaoZhi3":0,
    "wuDao":[]
}
```

增量修改可以避免游戏本体更新而导致需要重新适配数据文件，并且多个Mod之间也可以相互以增量修改的形式相互覆盖。


### 4.3 数据含义参考

该部分可参考3DM一位大佬的[此篇帖子](https://bbs.3dmgame.com/thread-6185512-1-1.html)

## 5 剧情文件

### 5.1 介绍
剧情文件是Next添加的剧情配置文件，用于向游戏内添加剧情。由于该功能由Next提供运行框架与扩展方法，因此能实现的功能较为有限。有更多的需求欢迎提Issue。

### 5.2 创建剧情文件

在Mod文件夹里新建 `DialogEvent` 文件夹，随后在该文件夹里新建任意名称的json文件，文件结构如下：

example.json
```json
[
    {
        "id":"测试1",
        "character":{
            "旁白" : 0,
            "主角" : 1,
            "倪旭欣" : 609
        },
        "dialog":[
            "主角#武当如何？",
            "倪旭欣#武当天下第一！",
            "旁白#你怎么看？"
        ],
        "option":[
            "天下第一！#测试2",
            "天下第一鸭！"
        ]
    },
    {
        "id":"测试2",
        "character":{
            "主角" : 1,
        },
        "dialog":[
            "主角#确实。"
        ],
        "option":[

        ]
    }
]
```

对象解释：
|字段|类型|说明|
|-|-|-|
|id|字符串|唯一ID|
|character|字典<br/>字符串-数字 键值对|储存预定义的角色ID<br/>预定义ID只能用于重要NPC的对话头像显示<br/>特殊ID：<br/>旁白：0<br/>主角：1<br/>更多的预定义ID可以在文件`AvatarJsonData.json`里查询|
|dialog|字符串列表|对话指令列表，详见 [**剧情对话指令**](剧情对话指令.md)|
|option|字符串列表|选项指令列表，详见 [**选项指令**](选项指令.md)|

## 6 触发器文件

### 6.1 介绍
触发器文件是Next添加的触发器配置文件，用于为剧情提供触发方式。由于触发器由Next提供对游戏原本功能的接口检测，因此能实现的功能较为有限。有更多的需求欢迎提Issue。

### 6.2 触发器文件

在Mod文件夹里新建 `DialogTrigger` 文件夹，随后在该文件夹里新建任意名称的json文件，文件结构如下：

example.json
```json
[
    {
        "id":"测试触发器",
        "type":"交谈",
        "condition":"roleBindID==609",
        "triggerEvent":"测试1"
    }
]
```

对象解释：
|字段|类型|说明|
|-|-|-|
|id|字符串|唯一ID|
|type|字符串|触发器类型，参考[**触发器类型**](触发器类型.md)|
|condition|字符串|检测指令，参考[**运行时脚本**](运行时脚本.md)|
|triggerEvent|字符串|触发事件，使用事件ID<br>参考本篇 **5 剧情文件**|

## 7 添加资源
Next为游戏引入了添加自定义资源的功能

### 7.1 介绍
在Mod文件夹里新建 `Assets` 文件夹，将图标按指定的目录放入其中，即可在游戏内读取出来

### 7.2 详情

该部分可参考 [**添加资源**](添加资源.md)

## 8 自定义捏脸数据

### 8.1 介绍
从Next 0.3.5版本开始，添加了自定义捏脸数据的功能。
通过自定义捏脸数据，可以替换或指定重要NPC的捏脸立绘。
也可以用于`SetCustomFace`指令更改角色的立绘。

注：定义的立绘文件需要新开档生效，或使用`SetCustomFace`指令强制刷新立绘，如`SetCustomFace*614#614`可强制刷新林沐心的立绘。

### 8.2 捏脸数据文件

在Mod文件夹里新建 `CustomFace` 文件夹，随后在该文件夹里新建任意名称的json文件，文件结构如下：

```json
{
    "ID": 618,
    "RandomInfos": {
      "head": 99,
      "eyes": 99,
      "mouth": 99,
      "nose": 99,
      "eyebrow": 99,
      "hair": 99,
      "feature": 99,
      "yanying": 99,
      "a_suit": 95,
      "hairColorR": 99,
      "yanzhuColor": 99,
      "tezhengColor": 99,
      "eyebrowColor": 99
    }
}
```
ID 为捏脸数据的ID
RandomInfos 为捏脸数据具体内容

与Seid文件不同之处在于，该处json文件仅使用文件内定义的ID作为导入的捏脸立绘ID。

通过使用
* 调试面板 - NPC调试 - 选择NPC - 导出捏脸数据  可将捏脸数据复制到剪切板

或者
* 调试面板 - 杂项调试 - 导出主角捏脸  可将捏脸数据导出到输入框内，自行将其复制出来即可。

导出主角立绘 功能可在角色创建界面进行捏脸后导出。