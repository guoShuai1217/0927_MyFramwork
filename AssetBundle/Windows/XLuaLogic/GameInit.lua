print("启动 GameInit.lua")

require("Windows.XLuaLogic.CtrMgr")

GameInit = {}

local this = GameInit;


-- 初始化
function GameInit.Init()

    this.InitView();
    CtrMgr.Init();

    -- 初始默认加载UIRoot界面
    GameInit.LoadView(CtrlNames.UIRootCtrl);
    
end

-- 初始化UI
function GameInit.InitView()

    for i = 1, #ViewNames  do
        require("Windows/XLuaLogic/Modules/UIRoot/"..tostring(ViewNames[i]))
    end
end


-- 加载UI界面
function GameInit.LoadView(type)
    local ctrl = CtrMgr.GetCtrl(type);
    if ctrl ~= nil then
        ctrl.Awake(); --第一次执行 就是执行UIRootCtrl.Awake();
    end
end