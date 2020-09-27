using NetFrame;
using Server.dao;
using ServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.cache
{
    public class UserCache
    {

        // 查询优先查询字典里是否有需要的数据,没有就去数据库找,找到了就存到这个字典里,不清空
        private Dictionary<int, USER> cacheDic = new Dictionary<int, USER>();

        //// 上线的 用户
        //private Dictionary<UserToken, int> onLineDic = new Dictionary<UserToken, int>();


        /// <summary>
        /// 是否存在这个用户
        /// </summary>
        /// <param name="acc_id"></param>
        /// <returns></returns>
        public bool hasUser(int acc_id)
        {
            initData(acc_id);
            return cacheDic.ContainsKey(acc_id);
        }


        /// <summary>
        /// 新建一个用户
        /// </summary>
        /// <param name="acc_id"></param>
        /// <param name="token"></param>
        public void CreateUser(int acc_id)
        {
            USER tmp = new USER(acc_id,"游客"+ acc_id);
            tmp.Add();

            cacheDic.Add(acc_id, tmp); // 加到缓存里
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="acc_id"></param>
        /// <returns></returns>
        public USER GetUserInfo(int acc_id)
        {

            if (!hasUser(acc_id))
            {
                DebugUtil.Instance.LogToTime("没有对应的用户,这是不允许的", LogType.ERROR);
                return null;
            }

            return cacheDic[acc_id];
        }


        #region 重命名

        // 缓存里是否有相同的昵称
        public bool IsCacheExistName(string name)
        {
            foreach (USER item in cacheDic.Values)
            {
                if (item.nickName == name)
                    return true;
            }
            return false;
        }

        // 数据库里是否有相同的昵称
        public bool IsSQLExistName(string name)
        {
            USER tmp = new USER();

            if (tmp.Exists(name))
            {
                DebugUtil.Instance.LogToTime("该昵称已存在 : " + name, LogType.NOTICE);
                return true;
            }
               
            return false;       
        }


        // 重命名
        public int ReName(int acc_id,string name)
        {
            USER tmp = cacheDic[acc_id];
            tmp.nickName = name;
            tmp.Update();

            return 0;
        }


        #endregion



        // 查找数据(缓存里没有,就去数据库查找一次)
        private void initData(int acc_id)
        {
            if (cacheDic.ContainsKey(acc_id)) return;

            USER tmp = new USER(acc_id);
            if (tmp.acc_id > 0)
                cacheDic.Add(tmp.acc_id, tmp);
        }
    }
}
