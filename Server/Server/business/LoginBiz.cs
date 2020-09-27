using NetFrame;
using GameProtocol;
using GameProtocol.model.login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.cache;
using ServerTools;

namespace Server.business
{
    /// <summary>
    /// 登录业务逻辑处理
    /// </summary>
    public class AccountBiz
    {
 
        /// <summary>
        /// 注册账号
        /// 0 成功,-1失败
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Register(UserToken token, AccountModel model)
        {
            //判断是否含有此账号
            if (CacheFactory.accountCache.IshasAccount(model.Account))
            {
                DebugUtil.Instance.LogToTime("username = " + model.Account + "请求注册失败，账号已经存在", LogType.WARRING);
                return -1;
            }

            // 创建账号
            int result = CacheFactory.accountCache.RegiserAccount(token, model);            
            return result;
        }


        /// <summary>
        /// 登陆
        /// 0 成功
        /// -1 登陆失败
        /// -2 账号密码不合法
        /// -3 账号不存在
        /// -4 账号密码不匹配
        /// -5 账号已上线
        /// </summary>
        public int Login(UserToken token, AccountModel model)
        {
            //判定请求是否正确
            if (model == null || string.IsNullOrEmpty(model.Account) || string.IsNullOrEmpty(model.Password))
            {
                DebugUtil.Instance.LogToTime("token = " + token.conn.RemoteEndPoint + "请求登录失败，请求错误", LogType.WARRING);
                return -1;
            }
            //判定账号密码是否合法
            if (model.Account.Length < 6 || model.Password.Length < 6)
            {
                DebugUtil.Instance.LogToTime("token = " + token.conn.RemoteEndPoint + "请求登录失败，账号密码不合法", LogType.WARRING);
                return -2;
            }
            //判断是否含有此账号
            if (!CacheFactory.accountCache.IshasAccount(model.Account))
            {
                DebugUtil.Instance.LogToTime("username = " + model.Account + "请求登录失败，没有此账号", LogType.WARRING);
                return -3;
            }
            //是否密码正确
            if (!CacheFactory.accountCache.IsPassword(model.Account, model.Password))
            {
                DebugUtil.Instance.LogToTime("username = " + model.Account + "请求登录失败，账号密码不匹配", LogType.WARRING);
                return -4;
            }
            //账号是否正在线
            if (CacheFactory.accountCache.IsOnline(token))
            {
                DebugUtil.Instance.LogToTime("username = " + model.Account + "请求登录失败，账号已在线", LogType.WARRING);
                return -5;
            }
            DebugUtil.Instance.LogToTime("username = " + model.Account + "请求登录验证成功");
            //全部条件满足，进行登录
            CacheFactory.accountCache.Online(token, model.Account);
            return 0;
        }


        ///// <summary>
        ///// 修改昵称请求
        ///// 0 成功, -1失败
        ///// </summary>
        //public int ReNickName(UserToken token, string nickName)
        //{
        //    // 判断缓存里有没有相同的昵称 ? 有的话 , 返回false
        //    // 缓存里没有 , 再去数据库里找 , 有的话 ,返回false 
        //    return CacheFactory.accountCache.ReNickName(token,nickName);           
        //}



        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="token"></param>
        public void OffLine(UserToken token)
        {
          //  CacheFactory.user.Save(token);
            CacheFactory.accountCache.offline(token);
        }




    }
}
