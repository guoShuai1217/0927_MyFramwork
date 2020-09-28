UIRootCtrl = {};
local this = UIRootCtrl;

local root;
local transform;
local gameObject;

local btn_Login;
local btn_Regist;
local input_Account;
local input_Password;

function UIRootCtrl.New()
    return this;
end

function UIRootCtrl.Awake()
    print("主界面启动");
    
    --调用C#脚本去加载资源 实例化UI
   -- CS.guoShuai.Lua.LuaHelper.Instance:LoadUIScene("UIPrefab/UIRootView",this.OnCreate);
    CS.guoShuai.Lua.LuaHelper.Instance:LoadUIDialog("Scene1","UIPrefab","UIRootView",this.OnCreate);
end

--实例化UI成功的回调
function UIRootCtrl.OnCreate(obj)

    print("实例化 " .. obj.name .." 成功"); --实例化成功后,会执行UI预制体 挂载的 LuaViewBehaviour 脚本
    btn_Login = UIRootView.btn_Login ;
    btn_Login.onClick:AddListener(this.onClickBtnLogin);

    btn_Regist = UIRootView.btn_Regist;
    btn_Regist.onClick:AddListener(this.onClickBtnRegist);

    input_Account = UIRootView.input_Account;
    input_Password = UIRootView.input_Password;

end

-- 登录按钮
function UIRootCtrl.onClickBtnLogin()
    print("点击了"..btn_Login.name);
    print("Account :" .. input_Account.text);
    print("Password :" .. input_Password.text);
end

-- 注册按钮
function UIRootCtrl.onClickBtnRegist()
    print("点击了"..btn_Regist.name);
    GameInit.LoadView(CtrlNames.RegistUICtr); --点击注册按钮，去加载注册界面
end