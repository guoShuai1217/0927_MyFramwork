/*
 *		Description: 所有用Lua加载出来的预制体,都要挂载这个脚本,或者挂载LuaWindowBehaviour
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.09.26
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace guoShuai.Lua
{

    [LuaCallCSharp]
    public class LuaViewBehaviour : MonoBehaviour
    {

        /// <summary>
        /// awake
        /// </summary>
        [CSharpCallLua]
        public delegate void delLuaAwake(GameObject obj);
        LuaViewBehaviour.delLuaAwake luaAwake;

        /// <summary>
        /// start 
        /// </summary>
        [CSharpCallLua]
        public delegate void delLuaStart();
        LuaViewBehaviour.delLuaStart luaStart;

        /// <summary>
        /// update
        /// </summary>
        [CSharpCallLua]
        public delegate void delLuaUpdate();
        LuaViewBehaviour.delLuaUpdate luaUpdate;

        /// <summary>
        /// ondestroy
        /// </summary>
        [CSharpCallLua]
        public delegate void delLuaOnDestroy();
        LuaViewBehaviour.delLuaOnDestroy luaOnDestroy;


        private LuaTable scriptEnv; // 用于访问lua代码 
        private LuaEnv luaEnv; // 全局唯一的lua虚拟机

        void Awake()
        {
            luaEnv = LuaMgr.luaEnv;
            scriptEnv = luaEnv.NewTable();

            LuaTable meta = luaEnv.NewTable(); // 元表
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
 
            string prefabName = name;
            if (prefabName.Contains("(Clone)"))
            {
                prefabName = prefabName.Split('(')[0];
            }
            prefabName = prefabName.Replace("pan_", "");

            luaAwake = scriptEnv.GetInPath<LuaViewBehaviour.delLuaAwake>(prefabName + ".awake");
            luaStart = scriptEnv.GetInPath<LuaViewBehaviour.delLuaStart>(prefabName + ".start");
            luaUpdate = scriptEnv.GetInPath<LuaViewBehaviour.delLuaUpdate>(prefabName + ".update");
            luaOnDestroy = scriptEnv.GetInPath<LuaViewBehaviour.delLuaOnDestroy>(prefabName + ".ondestroy");

            scriptEnv.Set("self", this);

            if (luaAwake != null)
                luaAwake(gameObject);

        }


        void Start()
        {
            if (luaStart != null)
                luaStart();
        }

        void Update()
        {
            if (luaUpdate != null)
                luaUpdate();
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
                luaOnDestroy();

            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            luaAwake = null;
        }


    }
}