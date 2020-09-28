UIRootView = {};
local this = UIRootView;

local transform;
local gameObject;



-- awake
function UIRootView.awake(obj)
    gameObject = obj ;
    transform = obj.transform;

    this.InitView();
end


-- 获取UI组件
function UIRootView.InitView()

    print(string.format("获取 %s 组件","UIRootView"));

    this.UIRoot = transform:Find("Canvas/UIRoot").transform;
    this.btn_Login = this.UIRoot:Find("LoginPage/btn_Login"):GetComponent("UnityEngine.UI.Button");
    this.btn_Regist = this.UIRoot:Find("LoginPage/btn_Regist"):GetComponent("UnityEngine.UI.Button");
    this.input_Account = this.UIRoot:Find("LoginPage/input_Account"):GetComponent("UnityEngine.UI.InputField");
    this.input_Password = this.UIRoot:Find("LoginPage/input_Password"):GetComponent("UnityEngine.UI.InputField");
   
end

-- start
function UIRootView.start()

end


-- update
function UIRootView.update()

end


-- ondestroy
function UIRootView.ondestroy()


end

