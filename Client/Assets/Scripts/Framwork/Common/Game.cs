using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using GameProtocol.model.login;
using System.Security.Cryptography;
using guoShuai.Lua;

public class Game : MonoBehaviour
{

    ///////////////////////////////////游戏参数//////////////////////////////////////////////////////////


    #region Public Reference
    private static Game _instance;
    public static Game Instance
    {
        get
        {
            if (_instance == null)
            {
                var pre = Resources.Load<GameObject>("Prefabs/Game");
                if (pre != null)
                {
                    //GameObject.Instantiate(prefab) as GameObject;
                    _instance = pre.GetComponent<Game>();
                    if (_instance == null)
                    {
                        PPP.pppShow();
                    }
                }
                PPP.pppShow();
            }
            return _instance;
        }
    }

    [SerializeField]
    private UIMgr uiMgr = null;
    public static UIMgr UIMgr
    {
        get
        {
            return Instance.uiMgr;
        }
    }


    [SerializeField]
    private SocketMgr socketMgr = null;
    public static SocketMgr SocketMgr
    {
        get
        {
            return Instance.socketMgr;
        }
    }

    [SerializeField]
    private DialogMgr dialogMgr = null;
    public static DialogMgr DialogMgr
    {
        get
        {
            return Instance.dialogMgr;
        }
    }

    [SerializeField]
    private LoaderManager assetbundleMgr = null;
    public static LoaderManager AssetBundleMgr
    {
        get
        {
            return Instance.assetbundleMgr;
        }
    }

    [SerializeField]
    private WebRequest web = null;
    public static WebRequest Web
    {
        get
        {
            return Instance.web;
        }
    }

    [SerializeField]
    private LuaMgr lua = null;
    public static LuaMgr Lua
    {
        get
        {
            return Instance.lua;
        }
    }

    //[SerializeField]
    //private SoundManager soundManager = null;
    //public static SoundManager SoundManager
    //{
    //	get
    //	{
    //		return Instance.soundManager;
    //	}
    //}


    //[SerializeField]
    //private IconManager iconManager = null;
    //public static IconManager IconMgr
    //{
    //	get
    //	{
    //		return Instance.iconManager;
    //	}
    //}

    //public GameObject objLoading=null;

    #endregion

    string downloadPath;

    private CheckUpdate checkRes;

    private void Awake()
    {
        _instance = this;

        // 检测是否有ab包更新
        downloadPath = PathUtil.GetAssetBundlePath();

        // 检测资源更新
        checkRes = new CheckUpdate(StartGame);
        checkRes.DownLoadVersionFile();

    



        //StartCoroutine(DownLoadRes(() =>
        //{
        //    UIMgr.InitializeUISystem();
        //    Game.UIMgr.PushScene(UIPage.LoginPage);

        //    // 资源更新完后 , 去加载Manifest文件
        //    StartCoroutine(LoadManifest.Instance.LoadAssetBundleManifest());
        //}));

    }

    // 更新完成了,开始游戏逻辑
    public void StartGame()
    {     
        // 资源更新完后 , 去加载Manifest文件
        StartCoroutine(LoadManifest.Instance.LoadAssetBundleManifest(BeginLuaMain));
        UIMgr.InitializeUISystem();
        Game.UIMgr.PushScene(UIPage.LoginPage);
    }

    // Manifest文件加载完成,去加载 Main.lua
    private void BeginLuaMain()
    {
        Game.Lua.LoadLuaMain();
      
    }


    // 加载bundle包的方法
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        LoaderManager.Instance.LoadAssetBundle("Scene1", "Prefabs", (bundleName,progress)=>{

    //            Debug.Log(progress);
    //            if (progress >= 1.0)
    //            Instantiate( LoaderManager.Instance.LoadAsset("Scene1", "Prefabs", "Player1"));

    //        });
    //    }
    //}

    // 当前用户信息类
    public static UserModel CurUserModel;

    // 当前用户头像
    public static Sprite curHead;


    #region API


    public static void DelayWaitForEndOfFrame(System.Action callback)
    {
        if (null != Instance)
        {
            StartCoroutineSingle(DelayFrame(callback));
        }
    }

    public static void Delay(float delayTime, System.Action callback)
    {
        if (null != Instance)
        {
            StartCoroutineSingle(DelayTime(delayTime, callback));
        }
    }

    public static Coroutine DelayLoop(int loop, float delayTime, System.Action<int> callback, System.Action onFinish = null)
    {
        if (null == Instance)
        {
            return null;
        }
        else
        {
            return StartCoroutineSingle(DelayLoopTime(loop, delayTime, callback, onFinish));
        }
    }

    public static Coroutine DelayLoop(float delayTime, System.Action callback, System.Action onFinish = null)
    {
        if (null == Instance)
        {
            return null;
        }
        else
        {
            return StartCoroutineSingle(DelayLoopTime(delayTime, callback, onFinish));
        }
    }

    static IEnumerator DelayFrame(System.Action callback)
    {
        yield return new WaitForEndOfFrame();
        callback();
    }

    static IEnumerator DelayTime(float delayTime, System.Action callback)
    {
        yield return new WaitForSeconds(delayTime);
        callback();
    }

    static IEnumerator DelayLoopTime(int loop, float delayTime, System.Action<int> callback, System.Action onFinish)
    {
        for (int i = 0; i < loop; i++)
        {
            yield return new WaitForSeconds(delayTime);
            callback(i);
        }

        if (null != onFinish)
        {
            onFinish();
        }
    }

    static IEnumerator DelayLoopTime(float delayTime, System.Action callback, System.Action onFinish)
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            callback();
        }
    }

    internal static void StopDelay(Coroutine coroutine)
    {
        if (null == Instance || null == coroutine)
        {
            return;
        }
        Instance.StopCoroutine(coroutine);
    }

    public static Coroutine Start(IEnumerator coroutine)
    {
        if (null == Instance || null == coroutine)
        {
            return null;
        }

        return StartCoroutineSingle(coroutine);
    }


    public static Coroutine StartCoroutineSingle(IEnumerator callback)
    {
        return Game.Instance.StartCoroutine(callback);
    }

    #endregion


    void OnApplicationQuit()
    {
#if UNITY_EDITOR && UNITY_ANDROID
		bool isUpdate=false;
		{
			string strPath=Application.dataPath+"/Plugins/Android/common/AndroidManifest.xml";
			if (File.Exists(strPath)) {
				string str= File.ReadAllText(strPath);
				var arrStr=str.Split('\"');
				var strPackageDefine="package=";
				var strWXEntryActivity=".wxapi.WXEntryActivity";
				var strWXPayEntryActivity=".wxapi.WXPayEntryActivity";
				var strFileProvider=".fileProvider";
				var strSchemeActivity=".SchemeActivity";
				var strXLEntryActivity=".sgapi.SGEntryActivity";
				var strXLSchemeID="xianliao";

				if (PPPGameConfig.strAndroidBundleID==null) {
					PPP.pppShow();
					return;
				}
				for (int i = 0; i < arrStr.Length; i++) {
					if (arrStr[i]==null) {
						PPP.pppShow();
						break;
					}
					if (arrStr[i].Contains(strPackageDefine)) {
						if (i+1<arrStr.Length) {
							var strPackage=arrStr[i+1];
							if (strPackage.Equals(PPPGameConfig.strAndroidBundleID)==false) {
								arrStr[i+1]=PPPGameConfig.strAndroidBundleID;
								isUpdate=true;
							}	
						}
					}
					else if (arrStr[i].Contains(strWXEntryActivity)) {
						string strTemp=PPPGameConfig.strAndroidBundleID+strWXEntryActivity;
						if (arrStr[i].Equals(strTemp)==false) {
							arrStr[i]=strTemp;
							isUpdate=true;
						}
					}
					else if (arrStr[i].Contains(strXLEntryActivity)) {
						string strTemp=PPPGameConfig.strAndroidBundleID+strXLEntryActivity;
						if (arrStr[i].Equals(strTemp)==false) {
							arrStr[i]=strTemp;
							isUpdate=true;
						}
					}
					else if (arrStr[i].Contains(strWXPayEntryActivity)) {
						string strTemp=PPPGameConfig.strAndroidBundleID+strWXPayEntryActivity;
						if (arrStr[i].Equals(strTemp)==false) {
							arrStr[i]=strTemp;
							isUpdate=true;
						}
					}
					else if (arrStr[i].Contains(strXLSchemeID)) {
						string strTemp=strXLSchemeID+PPPGameConfig.strXLAppKey;
						if (arrStr[i].Equals(strTemp)==false) {
							arrStr[i]=strTemp;
							isUpdate=true;
						}
					}
					else if (arrStr[i].Contains(strFileProvider)) {
						string strTemp=PPPGameConfig.strAndroidBundleID+strFileProvider;
						if (arrStr[i].Equals(strTemp)==false) {
							arrStr[i]=strTemp;
							isUpdate=true;
						}
					}
					else if (arrStr[i].Contains(strSchemeActivity)) {
						string strTemp=PPPGameConfig.strAndroidBundleID+strSchemeActivity;
						if (arrStr[i].Equals(strTemp)==false) {
							arrStr[i]=strTemp;
							isUpdate=true;
						}
					}
				}
				if (isUpdate) {
					StringBuilder sb=new StringBuilder();
					for (int i = 0; i < arrStr.Length; i++) {
						sb.AppendFormat("{0}\"",arrStr[i]);
					}
					sb.Remove(sb.Length-1,1);
					System.IO.File.WriteAllText(strPath,sb.ToString());
				}
			}
		}
		if (isUpdate) {
			string strPath=Application.dataPath+"/Resources/GameConfig2.cs";
			if (File.Exists(strPath)) {
				string str= File.ReadAllText(strPath);
				var arrStr=str.Split('\"');
				var strVersionDefine="strAndroidVersion=";
				bool isFind=false;
				for (int i = 0; i < arrStr.Length; i++) {
					if (arrStr[i].Contains(strVersionDefine)) {
						if (i+1<arrStr.Length) {
							var strVersion=arrStr[i+1];
							int version=0;
							if (int.TryParse(strVersion,out version)) {
								version++;
								arrStr[i+1]=version.ToString();
								isFind=true;
							}
						}
						break;
					}
				}
				if (isFind) {
					StringBuilder sb=new StringBuilder();
					for (int i = 0; i < arrStr.Length; i++) {
						sb.AppendFormat("{0}\"",arrStr[i]);
					}
					sb.Remove(sb.Length-1,1);
					System.IO.File.WriteAllText(strPath,sb.ToString());
				}
			}

			string strPathIcon=Application.streamingAssetsPath+"/"+PPP.strShareImagePath;
			string strPathDefault=Application.dataPath+"/Resources/icon.png";
			File.Copy(strPathIcon,strPathDefault,true);
		}
#endif
    }



    #region 资源热更

    #region 检测资源,对比MD5文件
    /// <summary>
    /// 检测资源
    /// </summary>
    private IEnumerator DownLoadRes(System.Action callback)
    {
        // 公司的服务器地址
        string url = ServerUtil.GetAssetBundleURL() + PathUtil.GetPlatformName();

        string fileUrl = url + "/" + "file.txt";

        WWW www = new WWW(fileUrl);
        yield return www;
        if (www.error != null)
            Debug.LogError("Error" + www.error);


        // 判断本地有没有这个目录
        // 如果发布到安卓端 : 游戏一运行,就把 streamingAssetsPath 的文件 拷贝到 persistentDataPath 路径下;


        if (!Directory.Exists(downloadPath))
            Directory.CreateDirectory(downloadPath);
        // 把下载的文件写入本地
        File.WriteAllBytes(downloadPath + "/file.txt", www.bytes);

        // 读取里面的内容
        string fileText = www.text;
        string[] lines = fileText.Split(new string[] { "\r\n" }, StringSplitOptions.None); // 2020.03.17 注意这里是 \r\n , 如果只\n的话,md5值不相同

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i])) //有空行,就continue
                continue;

            string[] kv = lines[i].Split('|');
            //kv =  scene1/character.assetbundle | 2503fdd66fea3185fa83930bc972bad2
            string fileName = kv[0];

            // 再拿到本地的这个文件
            string localFile = (downloadPath + "/" + fileName).Trim();

            // 如果没有本地文件 , 就下载
            if (!File.Exists(localFile))
            {
                string dir = Path.GetDirectoryName(localFile);
                Directory.CreateDirectory(dir);

                // 开始网络下载
                string tempUrl = url + "/" + fileName;
                Debug.LogWarning(tempUrl);

                WWW tmpwww = new WWW(tempUrl);
                yield return tmpwww;

                if (tmpwww.error != null)
                    Debug.LogError("加载 " + fileName + "出错 , Error : " + tmpwww.error);
                else
                    File.WriteAllBytes(localFile, tmpwww.bytes);
            }
            else //如果文件有的话 , 就开始比对MD5 ,检测是否更新了
            {
                string md5 = kv[1];
                string localMD5 = GetFileMD5(localFile);

                // 开始比对
                if (md5 == localMD5)
                {
                    // 相等的话,没更新
                    Debug.Log("没更新");
                }
                else
                {
                    // 不相等,说明更新了
                    // 删除本地原来的旧文件
                    File.Delete(localFile);

                    //下载新文件
                    string tmpUrl = url + "/" + fileName;
                    WWW Twww = new WWW(tmpUrl);
                    yield return Twww;
                    //进行一些网络的检测
                    if (Twww.error != null)
                        Debug.LogError("加载 " + fileName + "出错 , Error : " + Twww.error);
                    else
                        File.WriteAllBytes(localFile, Twww.bytes);
                }
            }

        }

        yield return new WaitForEndOfFrame();

        Debug.Log("更新完成,可以开始游戏了");
        callback();
    }
    #endregion


    /// <summary>
    /// 获取文件 MD5 值
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private string GetFileMD5(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open);

        // 引入命名空间   using System.Security.Cryptography;
        MD5 md5 = new MD5CryptoServiceProvider();

        byte[] bt = md5.ComputeHash(fs);
        fs.Close();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bt.Length; i++)
        {
            sb.Append(bt[i].ToString("x2"));
        }

        return sb.ToString();
    }






    #endregion
}
