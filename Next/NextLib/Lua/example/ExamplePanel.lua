---* 界面定义表
---* @field script 界面控制脚本，在进行界面初始化时自动赋值
---* @field contentPane 界面GComponent，在进行界面初始化时自动赋值
local window = {}

-- script的界面函数
-- script:Hide() -- 隐藏界面，会调用DoHideAnimation
-- script:HideImmediately() -- 立即隐藏界面，会调用OnHide
-- script:MakeFullScreenAndCenter() -- 界面全屏并居中

-- contentPane的函数参考教程：https://www.fairygui.com/docs/editor 和文档 https://fairygui.com/api/html/2a055c04-917b-2408-3841-c5c910eba6f9.htm

---* 界面初始化函数
function window.OnInit()

end

---* 界面每帧更新函数
function window.OnUpdate()

end

---* 界面按键按下回调
function window.OnKeyDown(eventContext)
    local inputInfo = eventContext.inputEvent
    local KeyCode = CS.UnityEngine.KeyCode
    -- 判断按键：inputInfo.keyCode == KeyCode.A
end

---* 界面显示函数，界面显示完需要调用OnShown
--如果不需要显示动画，可以删除该函数
function window.DoShowAnimation()
    ---* 界面显示完需要调用OnShown
    window.script:OnShown()
end

---* 界面显示完毕回调
function window.OnShown()

end

---* 界面隐藏函数，界面隐藏完需要调用DoHideAnimation
--如果不需要显示动画，可以删除该函数
function window.DoHideAnimation()
    window.script:HideImmediately()
end

---* 界面隐藏完毕回调
function window.OnHide()

end

-- 返回界面定义表
return window