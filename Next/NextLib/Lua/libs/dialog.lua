local eventRunner = require("libs.eventRunner")

local dialog = {}

function dialog.runEvent(scr, funcName, command,
                         env, callback)
    local target = require(scr)
    local func = target[funcName]
    local runner = eventRunner.getRunner(env,command)
    local function runEventCoroutine()
        xpcall(function()
            func(runner,env)
            callback()
        end, function(error)
            CS.SkySwordKill.Next.Main.LogError("Lua运行错误 --> "..error.."\n"..debug.traceback())
            eventRunner.cancelEvent()
        end )

    end
    local eventCoroutine = coroutine.create(runEventCoroutine)
    local state,info = coroutine.resume(eventCoroutine)
    if not state then
        CS.SkySwordKill.Next.Main.LogError(info)
    end
end

return dialog