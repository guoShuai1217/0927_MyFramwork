RegistUIView = {};
local this = RegistUIView;

local transform;
local gameObject;

-- awake
function RegistUIView.awake(obj)
    gameObject = obj ;
    transform = obj.transform;
    transform.parent = UIRootView.UIRoot;
    transform:GetComponent("RectTransform").offsetMin = CS.UnityEngine.Vector2.zero;
    transform:GetComponent("RectTransform").offsetMax = CS.UnityEngine.Vector2.zero;
    this.InitView();
end


-- 获取UI组件
function RegistUIView.InitView()

    this.Content = transform:Find("BackGound/Scroll View/Viewport/Content");
    this.txt_Show = transform:Find("BackGound/txt_Show"):GetComponent("UnityEngine.UI.Text");
    this.txt_Name = transform:Find("BackGound/txt_Name"):GetComponent("UnityEngine.UI.Text");
    
end

-- start
function RegistUIView.start()

end


-- update
function RegistUIView.update()

end


-- ondestroy
function RegistUIView.ondestroy()


end