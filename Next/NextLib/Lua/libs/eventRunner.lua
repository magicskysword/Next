---@class eventRunner
local eventRunner = {}
local meta = {}
local newEnvMeta = {}
local DialogSystem = CS.SkySwordKill.Next.DialogSystem
local DialogAnalysis = CS.SkySwordKill.Next.DialogSystem.DialogAnalysis
local DialogCommand = CS.SkySwordKill.Next.DialogSystem.DialogCommand
local Main = CS.SkySwordKill.Next.Main

local function async_to_sync(async_func, callback_pos, failureCall)
    return function(...)
        local _co = coroutine.running() or error('this function must be run in coroutine')
        local rets
        local waiting = false
        local function cb_func(...)
            if waiting then
                local state, info = coroutine.resume(_co, ...)
                if not state then
                    Main.LogError("Lua运行错误 --> " .. state)
                    failureCall()
                end
            else
                rets = { ... }
            end
        end

        local params = { ... }
        table.insert(params, callback_pos or (#params + 1), cb_func)
        async_func(table.unpack(params))
        if rets == nil then
            waiting = true
            rets = { coroutine.yield() }
        end

        return table.unpack(rets)
    end
end

local syncRunEvent = async_to_sync(DialogAnalysis.RunDialogEventCommand,
        nil, function() eventRunner.cancelEvent() end)

---@param runner eventRunner
---@param key string
meta["__index"] = function(runner, key)
    return function(...)
        local params = { ... }
        local command = DialogCommand(key, params,
                runner.callCommand.BindEventData, runner.callCommand.IsEnd)
        syncRunEvent(command, runner.env.rawEnv)
    end
end

newEnvMeta["__index"] = function(env, key)
    local value = env.rawEnv[key]
    local extMethod = DialogAnalysis.GetEnvQuery(key)

    if extMethod == nil and value == nil then
        Main.LogError("Lua运行错误 --> " .. "env没有找到扩展方法：" .. key)
        return nil
    end

    if extMethod ~= nil then
        return function(curEnv, ...)
            local params = { ... }
            if type(curEnv) ~= "table" then
                Main.LogError("Lua运行错误 --> " .. "env需要使用:调用函数！")
                return nil
            end

            local context = DialogSystem.DialogEnvQueryContext(curEnv.rawEnv, params)
            return extMethod:Execute(context)
        end
    else
        if type(value) == "function" then
            return function(curEnv ,...)
                local params = { ... }
                if type(curEnv) ~= "table" then
                    Main.LogError("Lua运行错误 --> " .. "env需要使用:调用函数！")
                    return nil
                end
                return value(curEnv.rawEnv, table.unpack(params))
            end
        else
            return env.rawEnv[key]
        end
    end
end

function eventRunner.getRunner(env, command)
    local runner = {}

    local newEnv = {}
    newEnv.rawEnv = env
    setmetatable(newEnv, newEnvMeta)

    runner.env = newEnv
    runner.bindEventData = bindEventData
    runner.callCommand = command
    setmetatable(runner, meta)
    return runner
end

function eventRunner.cancelEvent()
    DialogAnalysis.CancelEvent()
end

return eventRunner
