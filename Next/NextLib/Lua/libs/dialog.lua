local eventRunner = require("libs.eventRunner")

local dialog = {}
local Main = CS.SkySwordKill.Next.Main

function dialog.runEvent(scr, funcName, command,
                         env, callback)
    local target = require(scr)
    local func = target[funcName]
    local runner = eventRunner.getRunner(env, command)
    local function runEventCoroutine()
        xpcall(function()
            func(runner, env)
            callback()
        end, function(error)
            Main.LogError("Lua运行错误 --> " .. error .. "\n" .. debug.traceback(nil, 2))
            eventRunner.cancelEvent()
        end)

    end

    local eventCoroutine = coroutine.create(runEventCoroutine)
    local state, info = coroutine.resume(eventCoroutine)
    if not state then
        Main.LogError(info)
    end
end

return dialog
