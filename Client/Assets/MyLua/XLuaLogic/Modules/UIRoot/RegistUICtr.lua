require("MyLua.XLuaLogic.Common.LuaDefine")
require("MyLua.XLuaLogic.Entity.RegistTestEntity")
RegistUICtr = {};
local this = RegistUICtr;

 
local transform;
local gameObject;

-- new 一个实例
function RegistUICtr.New()
    return this;
end

-- awake
function RegistUICtr.Awake()
   
        --调用C#脚本去加载资源 实例化UI
        CS.guoShuai.Lua.LuaHelper.Instance:LoadUIDialog("Scene1","UIPrefab","RegistUIView",this.OnCreate);

end



--实例化UI成功的回调
function RegistUICtr.OnCreate(obj)
    print("实例化 " .. obj.name .." 成功"); --实例化成功后,会执行UI预制体 挂载的 LuaViewBehaviour 脚本
   
    -- 先加载 ItemView 资源
    CS.guoShuai.Lua.LuaHelper.Instance:LoadUIDialog("Scene1","ItemPrefab","ItemView",this.OnLoadItemView);

   
 

end

function RegistUICtr.OnLoadItemView(itemViewPrefab)

    print("加载 " .. itemViewPrefab.name .. "成功 !");
    
     local table = {}
    table[#table+1] = RegistTestEntity.New(1001,"牛刀小试",0,"少侠我看你骨骼精奇，这有本如来神掌10块钱卖给你如何");
    table[#table+1] = RegistTestEntity.New(1002,"武器考验",0,"试试这把无尽之刃吧,简直是神器");
    table[#table+1] = RegistTestEntity.New(1003,"技能传授",0,"看我神威,无坚不摧");
    table[#table+1] = RegistTestEntity.New(1004,"修为精进",0,"修为精进,一日千里,真是万中无一的绝世高手");
    table[#table+1] = RegistTestEntity.New(1005,"强化武器",0,"一把无尽不大够,再来一把电刀如何...");
    table[#table+1] = RegistTestEntity.New(1006,"魔法抗性",0,"现在你需要一件幽魂斗篷来保护自己");
    table[#table+1] = RegistTestEntity.New(1007,"物理抗性",0,"没有比荆棘之甲更能保护你了");
    table[#table+1] = RegistTestEntity.New(1008,"春哥馈赠",0,"信春哥得永生 ,春哥宝甲送给你了,可以复活哦");
    table[#table+1] = RegistTestEntity.New(1009,"移速暴增",0,"穿上轻灵之靴,健步如飞,日行千里");

    
    for i = 1, #table do
        
        --加载数据，实例化ItemView，添加点击事件
        local oo = CS.UnityEngine.GameObject.Instantiate(itemViewPrefab);
        oo.transform.parent = RegistUIView.Content;
        local tog =  oo:GetComponent("UnityEngine.UI.Toggle");
        oo.transform:Find("Label"):GetComponent("UnityEngine.UI.Text").text = table[i].Title;
        tog.group = RegistUIView.Content:GetComponent("UnityEngine.UI.ToggleGroup");
        tog.onValueChanged:AddListener(function (isOn)         
           
            if isOn then 
                RegistUIView.txt_Show.text = table[i].Content;
            end
           
        end);

    end

end





 