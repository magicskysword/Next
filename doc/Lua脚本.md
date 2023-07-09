# Lua脚本
Next通过嵌入xlua以实现与lua脚本交互的能力

Lua脚本不同于运行时脚本，运行时脚本本质上仅是为了方便进行逻辑运算而引入的C#逻辑判断脚本，但Lua脚本则可以独立调用剧情指令，进行更加复杂的流程调控。

## 文件结构
在NData文件夹里新建 `Lua` 文件夹，随后在该文件夹里新建任意名称的lua文件，例如`test.lua`，文件内容如下：

```lua
local test = {}

function test.func(runner,env)
    -- 运行内容
end

function test.func2(runner,env)
    -- 运行内容
end


return test
```

在剧情对话指令中，通过`RunLua*脚本名#函数名`的方式即可调用Lua，如`RunLua*test#func`

## 调用剧情对话指令
剧情对话指令参考：[剧情对话指令](剧情对话指令.md)

所有的剧情对话指令在lua脚本中都可以直接调用，通过`runner.command()`的方式调用剧情指令，如：
```lua
local test = {}

function test.func(runner,env)
    runner.Say("主角","你好。")
end

return test
```
为了表达方便，在后面的范例中将直接以
```lua
runner.Say("主角","你好。")
```
的方式呈现函数中的内容

为了方便脚本编写，剧情对话指令在lua中转换成了协程调用的方式，每条指令在调用后，会等待到调用结束再调用下一条指令，如：
```lua
runner.Say("主角","你好。")
runner.Say("主角","今天是周一。")
runner.Say("主角","据说局部地区会下纯阳法器。")
```

## 调用运行时脚本环境
运行时脚本参考：[运行时脚本](运行时脚本.md)

使用lua时，同样可以如同运行时脚本那样调用环境指令。只需要对env进行字段获取或函数调用即可

获取字段 范例：
```lua
local a = env.itemID
```

调用方法 范例：
```lua
local a = env:GetHP()
```
**提示：在xlua中，C#对象调用方法时，需要使用`obj.method(obj,arg1,arg2,...)`的方式进行，而使用语法糖:号则可以将自身作为第一个参数传入函数之中。**

**结论：env中，带括号调用的需要用冒号代替点，如`env:method()`**

## 获取Lua函数返回值
在运行时脚本中，通过`GetLuaInt(luaFile,luaFunc)`或`GetLuaStr(luaFile,luaFunc)`的方式即可运行Lua函数并获取返回值。

获取返回值的函数与被对话指令调用的函数格式略有不同，函数范例如下：
```lua
local test = {}

function test.I1(env)
    return env:GetMoney() * 2
end

function test.S1(env)
    return "灵石：" .. tostring(env:GetMoney())
end

return test
```
env为运行时脚本环境，通过return即可将计算完的数值返回。

## Lua脚本逻辑范例

```lua
local test = {}

function test.T1(runner,env)
    runner.SetChar("倪旭欣","609")
    runner.Say("倪旭欣","{daoyou}你好。")
    runner.ChangeMoney(1000)
    if env:GetMoney() < 2000 then
        runner.Say("倪旭欣","拿少了，再来！")
        runner.ChangeMoney(1000)
    else
        runner.Say("倪旭欣","钱给了，告辞！")
    end
end

function test.I1(env)
    return env:GetMoney() * 2
end

function test.S1(env)
    return "灵石：" .. tostring(env:GetMoney())
end

return test
```

## Require路径说明

### Lua内置库
Lua内置了一些库，可以用于require，
内置库的位置位于 `NextLib/Lua` 下

Require时，直接以库位置作为根目录进行Require即可，如require`NextLib/Lua/libs/test.json`时：

```lua
local json = require 'libs/json'
local json = require 'libs.json'
local json = require('libs/json')
```

### 自定义Lua文件
在`Mod文件夹/Lua`下的文件，如同内置库一般可以直接进行require，如require`Mod文件夹/Lua/test.json`时：

```lua
local test = require 'test'
local test = require 'test'
local test = require('test')
```

## 内置函数

`print(arg1,arg2,...)`

    打印数据，该函数已经重定向到Next的输出上。


`getmodpath(luafile)`

    参数为lua脚本地址，可参考require
    该函数返回lua脚本的mod目录来源。