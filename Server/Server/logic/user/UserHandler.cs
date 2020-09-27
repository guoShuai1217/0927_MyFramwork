using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame.auto;
using GameProtocol;
using GameProtocol.model.login;
using ServerTools;
using Server.business;
using Server.tool;
using Server.dao;

namespace Server.logic.user
{
    public class UserHandler : IHandler
    {
        public void ClientClose(UserToken token, string error)
        {
            // BizFactory.user.OffLine(token);
            BizFactory.account.OffLine(token);
        }



        public void MessageReceive(UserToken token, SocketModel message)
        {
            if (!cache.CacheFactory.accountCache.IsOnline(token)) return;
            switch (message.command)
            {
                 // 请求用户数据
                case UserProtocol.GETINFO_CREQ:

                    GetInfo(token);
                    break;

                // 重命名请求
                case UserProtocol.RENAME_CREQ:

                    ReName(token,message);

                    break;

                // 存储图片请求
                case UserProtocol.UPDATEIMG_CREQ:
                    ImageModel im = message.GetMessage<ImageModel>();
                    SaveImage(token,im);
                 
                    break;

                // 修改用户信息
                case UserProtocol.UPDATEINFO_CREQ:
                    UserModel tmp = message.GetMessage<UserModel>();
                    updateUserModel(token,tmp);
                    break;

            }
        }

    

        // 获取用户信息请求
        private void GetInfo(UserToken token)
        {
            ExecutorPool.Instance.execute(() => {

                DebugUtil.Instance.LogToTime("请求用户数据");

                UserModel user = BizFactory.user.GetUserInfo(token);

                token.write(TypeProtocol.USER, UserProtocol.GETINFO_SRES, user);

            });
        }


 
        // 重命名
        private void ReName(UserToken token, SocketModel message)
        {
            ExecutorPool.Instance.execute(() => {

                DebugUtil.Instance.LogToTime("用户请求重命名昵称");

                string name = message.GetMessage<string>();

                int result = BizFactory.user.ReName(token, name);

                token.write(TypeProtocol.USER, UserProtocol.RENAME_SRES, result);

            });
          
        }


        // 存储图片
        private void SaveImage(UserToken token, ImageModel im)
        {
            ExecutorPool.Instance.execute(() =>
            {
                int saveResult = BizFactory.user.SaveImage(token, im);

                token.write(TypeProtocol.USER, UserProtocol.UPDATEIMG_SRES, saveResult);
            });
        }


        // 修改个人信息
        private void updateUserModel(UserToken token, UserModel tmp)
        {
            ExecutorPool.Instance.execute(() =>
            {
                int result = BizFactory.user.UpdateInfo(token, tmp);

                token.write(TypeProtocol.USER, UserProtocol.UPDATEINFO_SRES, result);
            });
        }



    }
}
