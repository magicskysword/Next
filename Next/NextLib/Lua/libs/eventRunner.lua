---@class eventRunner
local eventRunner = {}
local meta = {}

local function async_to_sync(async_func, callback_pos, failureCall)
    return function(...)
        local _co = coroutine.running() or error('this function must be run in coroutine')
        local rets
        local waiting = false
        local function cb_func(...)
            if waiting then
                local state, info = coroutine.resume(_co, ...)
                if not state then
                    CS.SkySwordKill.Next.Main.LogError("Lua运行错误 --> " .. state)
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

local syncRunEvent = async_to_sync(CS.SkySwordKill.Next.DialogAnalysis.RunDialogEventCommand,
    nil, function() eventRunner.cancelEvent() end)

---@param runner eventRunner
---@param key string
meta["__index"] = function(runner, key)
    return function(...)
        local params = { ... }
        local command = CS.SkySwordKill.Next.DialogCommand(key, params,
            runner.callCommand.BindEventData, runner.callCommand.IsEnd)
        syncRunEvent(command, runner.env)
    end
end

function eventRunner.getRunner(env, command)
    local runner = {}
    runner.env = env
    runner.bindEventData = bindEventData
    runner.callCommand = command
    setmetatable(runner, meta)
    return runner
end

function eventRunner.cancelEvent()
    CS.SkySwordKill.Next.DialogAnalysis.CancelEvent()
end

return eventRunner
