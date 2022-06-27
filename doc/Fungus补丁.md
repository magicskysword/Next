# Fungus补丁

## 游戏内Fungus数据

### 导出
如果已经导出Base数据的话，可以忽略导出步骤

点击Next面板上的【导出Base】按钮，即可导出Fungus数据。

导出的游戏数据位于 `..\OutPut\Funugs` 根据Next的安装位置，有所不同

### 查看数据
导出的数据格式通常如下：
```json
{
  "Name": "NPCJiaoHuTalk",
  "Blocks": [
    {
      "ItemID": 344,
      "Name": "聊天按钮被点击起始点",
      "Description": "聊天按钮被点击起始点",
      "Position": "(-3033.412,-1448.144)",
      "Commands": [
        {
          "ItemID": 371,
          "CmdType": "Fungus.TryinitFungaus"
        },
        {
          "ItemID": 346,
          "CmdType": "CmdInitNPC"
        },
        {
          "ItemID": 352,
          "CmdType": "Fungus.Call",
          "targetFlowchartName": null,
          "targetBlockID": "351(首次交谈判断)",
          "startLabel": "",
          "startIndex": 0,
          "callMode": 0
        }
      ]
    },
......
```
Flowchart指每一个导出的文件

Block指一个个对话节点

Command指具体运行的剧情指令

## 创建Fungus补丁
在Mod文件夹里新建 `NData/FungusPatch` 文件夹，随后在该文件夹里新建任意名称的json文件，文件结构如下：

`example.json`
```json
[
    {
        "TargetFlowchart" : "NPCJiaoHuTalk",
        "TargetBlock" : 362,
        "TargetCommand" : 1147,
        "Priority" : 0,
        "Type" : "Insert",
        "Command" : {
            "CmdType" : "NextMenu",
            "CmdParams" : "啊哈哈哈鸡汤来咯！#鸡汤来咯"
        }
    },
    {
        "TargetFlowchart" : "NPCJiaoHuTalk",
        "TargetBlock" : 362,
        "TargetCommand" : 397,
        "Priority" : 0,
        "Type" : "Delete",
    }
]
```

对象解释：
|字段|类型|说明|
|-|-|-|
|TargetFlowchart|字符串|目标Flowchart的名称|
|TargetBlock|整数|目标Block的ItemID|
|TargetCommand|整数|要插入的Command的ItemID，若为-1，即为插入到所有Command的最后|
|Priority|整数|优先级，优先级越高的Patch，在插入时index越大|
|Type|PatchType|Patch类型，目前有Insert与Delete两种。<br>Insert类型会插入到目标命令的前面。<br>Delete类型会删除目标命令|
|Command|FPatchCommand|插入的指令数据，仅在Insert模式下生效|

Command对象解释：
|字段|类型|说明|
|-|-|-|
|CmdType|字符串|Patch的指令类型|
|CmdParams|字符串|Patch的指令参数|

通过在导出的Fungus里找到需要修补的剧情，然后将对应的ID填入补丁对象内，即可在游戏运行中对其进行修补。


### 可插入的Command类型
目前可以插入的Command如下：
|CmdType类型|说明|CmdParams参数|
|-|-|-|
|NextEvent|跳转到Next事件|目标事件#跳转条件<br>只有满足条件时才跳转，可不填写，默认为True
|NextMenu|弹出选项框，选择后跳转到Next事件|选项名称#目标事件#显示条件<br>只有满足条件时才显示选项，可不填写，默认为True