用lua去加载UI的操作流程 : 
1. 做好预制体CustomUIView，资源可以AB，可以Resource。 目前还没写异步加载AB包，所以只能先放Resources目录下加载；

2. 在 LuaDefine.lua 里注册 CustomUICtrl 和 CustomUIView ；
2.1 CustomUIView.lua文件的存放地址 ：MyLua/XLuaLogic/Modules/UIRoot/ ，不然加载不到文件 (代码见 GameInit.InitView())
2.2 CustomUICtrl.lua文件放在哪里都可以，但是要在 CtrMgr里require,还要在CtrMgr.Init()里New出来存到集合里 (代码见 CtrMgr/require("MyLua.XLuaLogic.Modules.UIRoot.UIRootCtrl")，CtrMgr.Init())
2.3 CustomUICtrl.lua 粘贴 ControlTemplate.lua内容； CustomUIView.lua 粘贴 ViewTemplate.lua内容 ， 记得把里面的Table名和方法名改成自己的名字

------------------------------现在代码都写好了，接下来加载这个UI界面-------------------------------------------------

3. 加载CustonUI界面的API : GameInit.LoadView(CtrlNames.CustomUI);
4. 在CustomUICtrl.lua 的Awake()方法里 ， 去执行 CS.LuaHelper.Instance:LoadUIScene("UIPrefab/CustomUIView",this.OnCreate);
5. 如果一切都顺利的话，在OnCreate回调里，就获取到实例化成功的gameObject对象了


--注 : 
当资源发生改变的时候(修改预制体的时候) , 这时候肯定lua脚本也发生了改变  : 
需要
	1. StreamingAssets/Windows 下的 XLuaLogic 需要拿出来(这里是最新的lua脚本)
	2. 删除AB包 , 重新打AB包
	3. 把XLuaLogic 再放回StreamingAssets/Windows 下 ; 
	4. 生成MD5文件 ;
	5. 拷贝到 服务端 ;

