using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using Server.business;
using GameProtocol.model.login;
using ServerTools;
using GameProtocol;
using Server.tool;

namespace Server.logic.login
{
    /// <summary>
    /// 处理客户端二级业务消息分发
    /// </summary>
    public class AccountHandler : IHandler
    {
        /// <summary>
        /// 用户断开连接
        /// </summary>
        /// <param name="token"></param>
        /// <param name="error"></param>
        public void ClientClose(UserToken token, string error)
        {
            //BizFactory.user.OffLine(token);
            BizFactory.account.OffLine(token);
        }
      
        
        /// <summary>
        /// 用户消息到达
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void MessageReceive(UserToken token, SocketModel message)
        {
            //处理客户端的请求
            switch (message.command)
            {
                // 注册请求 2020.03.18
                case AccountProtocol.REGISTER_CREQ:

                    regist(token,message);

                    break;

                // 处理客户端的登录请求
                case AccountProtocol.LOGIN_CREQ:

                    login(token, message);

                    break;

                // 下线
                case AccountProtocol.OFFLINE_CREQ:

                    offLine(token);
                   
                    break;
            }
        }

     
        // 注册
        void regist(UserToken token,SocketModel message)
        {
            ExecutorPool.Instance.execute(() =>
            {
                DebugUtil.Instance.LogToTime("用户注册请求到达");

                AccountModel model = message.GetMessage<AccountModel>();
                int registResult = BizFactory.account.Register(token, model); // 处理注册请求
                token.write(TypeProtocol.ACCOUNT, AccountProtocol.REGISTER_SRES, registResult); // 返回注册结果 给客户端
            });
        }


        // 登陆
        private void login(UserToken token, SocketModel message)
        {
            ExecutorPool.Instance.execute(() =>
            {

                DebugUtil.Instance.LogToTime("用户请求登录消息到达");

                AccountModel acc = message.GetMessage<AccountModel>();

                // 处理登陆请求
                int accResult = BizFactory.account.Login(token, acc);
                // 返回登陆结果 给客户端
                token.write(TypeProtocol.ACCOUNT, AccountProtocol.LOGIN_SRES, accResult);

            });
        }


        // 下线
        private void offLine(UserToken token)
        {
            ExecutorPool.Instance.execute(() =>
            {
                DebugUtil.Instance.LogToTime("用户请求退出登录");

                //BizFactory.user.OffLine(token);
                BizFactory.account.OffLine(token);

            });
        }
    }
}
