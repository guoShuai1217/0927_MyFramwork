/*
 *		Description: 
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
namespace guoShuai.Lua
{
    public class LuaGameMgr : MonoBehaviour
    {

        void Awake()
        {
            gameObject.AddComponent<LuaMgr>();
        }

        void Start()
        {
            LuaMgr.Instance.DoString("require 'MyLua/XLuaLogic/Main'");
        }
    }

}