/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System.Collections;
using System.Collections.Generic;
using guoShuai.Lua;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace guoShuai.Lua
{

    [LuaCallCSharp]
    public class LuaHelper
    {
        #region 单例
        private static LuaHelper instance;
        public static LuaHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LuaHelper();
                }
                return instance;
            }
        }
        #endregion


        public void LoadUIDialog(string sceneName, string fileName, string assetName, XLuaCustomExport.OnCreate onCreate = null)
        {
            if (!LoaderManager.Instance.IsLoadedAssetBundle(sceneName, fileName)) //没有加载过,就去加载
            {
                LoaderManager.Instance.LoadAssetBundle(sceneName, fileName, (bundleName, progress) =>
                {

                    Debug.Log(progress);

                    if (progress >= 1.0)
                    {
                        GameObject oo = GameObject.Instantiate(LoaderManager.Instance.LoadAsset(sceneName, fileName, assetName)) as GameObject;
                        oo.name = oo.name.Split('(')[0];

                        if (onCreate != null)
                        {
                            if (oo.GetComponent<LuaViewBehaviour>() == null)
                                oo.AddComponent<LuaViewBehaviour>();
                            onCreate(oo);
                        }
                    }

                });
            }
            else
            {
                GameObject oo = GameObject.Instantiate(LoaderManager.Instance.LoadAsset(sceneName, fileName, assetName)) as GameObject;
                oo.name = oo.name.Split('(')[0];

                if (onCreate != null)
                {
                    if (oo.GetComponent<LuaViewBehaviour>() == null)
                        oo.AddComponent<LuaViewBehaviour>();
                    onCreate(oo);
                }
            }
              
        }


        /// <summary>
        /// 实例化UI界面
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="onCreate">实例化UI成功的 回调</param>
        /// <returns></returns>
        public GameObject LoadUIScene(string path, XLuaCustomExport.OnCreate onCreate = null)
        {
            GameObject obj = Resources.Load<GameObject>(path); //这里仅做测试,加载AB包应用异步方式去实现
            if (obj != null)
            {
                GameObject oo = GameObject.Instantiate(obj);
                oo.name = oo.name.Split('(')[0];
                if (onCreate != null)
                {
                    if (oo.GetComponent<LuaViewBehaviour>() == null)
                        oo.AddComponent<LuaViewBehaviour>();
                    onCreate(oo);
                }
                return oo;

            }
            return null;
        }

        public GameObject LoadUIItem(string path)
        {
            GameObject obj = Resources.Load<GameObject>(path); //这里仅做测试,加载AB包应用异步方式去实现
            return obj;
        }

    }
}

