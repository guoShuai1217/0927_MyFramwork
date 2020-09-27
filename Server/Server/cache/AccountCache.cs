using GameProtocol.model.login;
using NetFrame;
using Server.dao;
using ServerTools;
using System.Collections.Generic;

namespace Server.cache
{
    /// <summary>
    /// 用户数据缓存
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        /// 用户账号与用户数据的映射
        /// [查询优先查询这个字典,没有再去数据库查找,找到后存到这个字典里,不清空]
        /// </summary>
        Dictionary<string, dao.ACCOUNT> cacheDic = new Dictionary<string, dao.ACCOUNT>();
        /// <summary>
        /// 在线连接和用户账号的映射
        /// </summary>
        Dictionary<UserToken, string> OnlineAccount = new Dictionary<UserToken, string>();
        /// <summary>
        /// 玩家ID与用户连接的映射
        /// </summary>
        Dictionary<int, UserToken> IdToToken = new Dictionary<int, UserToken>();

      

        /// <summary>
        /// 注册账号
        /// 0成功, -1失败
        /// </summary>
        public int RegiserAccount(UserToken token, AccountModel model)
        {
            //创建一个新的角色账号
            dao.ACCOUNT acc = new dao.ACCOUNT();                   
            acc.account = model.Account;
            acc.password = model.Password;

            acc.Add(); // 向数据库添加账号
 
            cacheDic.Add(acc.account, acc);
          //  CacheFactory.user.Online(token, acc.account);
            return 0;
        }

    
        /// <summary>
        /// 是否含有此账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IshasAccount(string account)
        {
            init(account);
            return cacheDic.ContainsKey(account);         
        }

        /// <summary>
        /// 账号密码是否匹配
        /// </summary>
        /// <returns></returns>
        public bool IsPassword(string account,string password)
        {
            init(account);

            if (!IshasAccount(account)) return false;
            if (cacheDic[account].password.Equals(password))
                return true;
            return false;
        }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsOnline(UserToken token)
        {
            if (OnlineAccount.ContainsKey(token))
                return true;
            return false;
        }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsOnline(int id)
        {
            if (IdToToken.ContainsKey(id) && OnlineAccount.ContainsKey(IdToToken[id]))
                return true;
            return false;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void Online(UserToken token, string account)
        {
            if (IsOnline(token))
            {
                DebugUtil.Instance.LogToTime(account + "已在线", LogType.WARRING);
                return;
            }
            if (!IshasAccount(account))
            {
                DebugUtil.Instance.LogToTime(account + "不存在", LogType.WARRING);
                return;
            }
            if (IdToToken.ContainsKey(cacheDic[account].id))
            {
                DebugUtil.Instance.LogToTime(account + "移除账号连接", LogType.WARRING); 
                IdToToken.Remove(cacheDic[account].id);
            }
            DebugUtil.Instance.LogToTime(account + "上线成功", LogType.WARRING);
            IdToToken.Add(cacheDic[account].id, token);
            OnlineAccount.Add(token, account);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void offline(UserToken token)
        {
            //如果在线并且含有此账号
            if (IsOnline(token) && IshasAccount(OnlineAccount[token]))
            {
                int id = cacheDic[OnlineAccount[token]].id;
                if (IdToToken.ContainsKey(id))
                    IdToToken.Remove(id);
                OnlineAccount.Remove(token);
                DebugUtil.Instance.LogToTime(id + "玩家下线了");
            }
        }

      
        public ACCOUNT GetRoleInfoByToken(UserToken token)
        {
            //判断是否在线
            if (!IsOnline(token)) return null;
            //判断是否含有账号
            if (!IshasAccount(OnlineAccount[token])) return null;
            return cacheDic[OnlineAccount[token]];
        }

        public ACCOUNT GetRoleInfoById(int id)
        {
            //判断是否在线
            if (!IsOnline(id)) return null;
            //判断是否含有账号
            if (!IshasAccount(OnlineAccount[IdToToken[id]])) return null;
            return cacheDic[OnlineAccount[IdToToken[id]]];
        }


        public string GetAccountByToken(UserToken token)
        {           
            if (OnlineAccount.ContainsKey(token))
                return OnlineAccount[token];

            return null;
        }

        /// <summary>
        /// 通过连接获取用户ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetIdToToken(UserToken token)
        {
            //如果当前没有在线，或没有此账号 
            if (!IsOnline(token) || !IshasAccount(OnlineAccount[token])) return -1;
            return cacheDic[OnlineAccount[token]].id;
        }

        /// <summary>
        /// 通过用户ID获取连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserToken GetToken(int id)
        {
            //如果当前没有该ID的映射
            if (!IdToToken.ContainsKey(id)) return null;
            return IdToToken[id];
        }

       

        /// <summary>
        /// 检查是否需要向数据库查询数据
        /// </summary>
        /// <param name="account"></param>
        public void init(string account)
        {
            if (cacheDic.ContainsKey(account)) return; // 字典里存在该账号,就不需要向数据库查询了

            ACCOUNT ACC = new ACCOUNT(account); // 向数据库查询该账号
            if (ACC.id >= 0)  // 如果账号存在,就加到字典里
            {
                cacheDic.Add(account, ACC);
            }
        }
    }
}
