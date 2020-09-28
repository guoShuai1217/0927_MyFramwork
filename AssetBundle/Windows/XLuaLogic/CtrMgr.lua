 --Lua控制器的管理器,作用就是注册所有的控制器
print("启动 CtrMgr.lua");

--require("MyLua/Common/LuaDefine")
require("Windows.XLuaLogic.Common.LuaDefine")

--有多少控制器,就写多少
require("Windows.XLuaLogic.Modules.UIRoot.UIRootCtrl")
require("Windows.XLuaLogic.Modules.UIRoot.RegistUICtr")


CtrMgr = {};
local this = CtrMgr;

--控制器列表
local ctrlList = {};

--初始化 添加所有控制器
function CtrMgr.Init()
    ctrlList[CtrlNames.UIRootCtrl] = UIRootCtrl.New();
    ctrlList[CtrlNames.RegistUICtr] = RegistUICtr.New();

    return this ;
end


--根据控制器名称 获取控制器
function CtrMgr.GetCtrl(ctrlName)
    return ctrlList[ctrlName];
end