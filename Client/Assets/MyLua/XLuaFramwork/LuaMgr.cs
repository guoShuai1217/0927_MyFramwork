/*
 *		Description: Lua虚拟机,处于性能的考虑,一般全局唯一
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.09.26
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace guoShuai.Lua
{
    public class LuaMgr : MonoBehaviour
    {
        public static LuaMgr Instance;

        /// <summary>
        /// 全局唯一的 xlua 环境
        /// </summary>
        public static LuaEnv luaEnv;

        void Awake()
        {
            Instance = this;

            initLuaEnv();
        }

        // 初始化 luaEnv
        private void initLuaEnv()
        {
            luaEnv = new LuaEnv();

            // 设置xlua的脚本路径(Application.dataPath路径下的所有的.lua文件,都是我们需要加载的)
            luaEnv.DoString(string.Format("package.path = '{0}/?.lua'",Application.dataPath));
        }

        /// <summary>
        /// 更新资源完成 加载Main.lua
        /// </summary>
        public void LoadLuaMain()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            LuaMgr.Instance.DoString("require 'MyLua/XLuaLogic/Main'");
#elif UNITY_ANDROID
            LuaMgr.Instance.DoString("require 'MyLua/XLuaLogic/Main'");
#endif
        }


        /// <summary>
        /// 执行lua脚本
        /// </summary>
        /// <param name="str"></param>
        public void DoString(string str)
        {
            luaEnv.DoString(str);
        }

    }
}
