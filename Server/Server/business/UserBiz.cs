using GameProtocol.model.login;
using NetFrame;
using Server.cache;
using Server.dao;
using ServerTools;
using System;
using System.IO;

namespace Server.business
{
    public class UserBiz
    {

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        public UserModel GetUserInfo(UserToken token)
        {
            // 根据token获取账号id
            int acc_id = CacheFactory.accountCache.GetIdToToken(token);

            // 用户是否存在?
            bool result = CacheFactory.userCache.hasUser(acc_id);

            if (!result) // 没有这个用户数据(新用户),新建一个USER
            {
                CacheFactory.userCache.CreateUser(acc_id);
            }

            // 获取用户信息
            USER user = CacheFactory.userCache.GetUserInfo(acc_id);

            return GetUserModel(token, user);

        }


        // USER -> UserModel 发送给客户端
        UserModel GetUserModel(UserToken token, USER user)
        {
            UserModel model = new UserModel();

            model.account = CacheFactory.accountCache.GetAccountByToken(token);
            model.id = user.acc_id;
            model.nickname = user.nickName;
            model.phone = user.phone;
            model.sex = user.sex;
            model.mail = user.mail;
            model.birthday = user.birthday;

            return model;
        }



        ///// <summary>
        ///// 下线
        ///// </summary>
        ///// <param name="token"></param>
        //public void OffLine(UserToken token)
        //{
        //    CacheFactory.userCache.OffLine(token);
        //}


        /// <summary>
        /// 重命名     
        /// -1 昵称已存在
        /// 0  修改成功
        /// </summary>
        /// <param name="token"></param>
        public int ReName(UserToken token, string name)
        {

            // 1. 缓存里有没有相同的昵称
            // 2. 数据库里有没有相同的昵称
            if (CacheFactory.userCache.IsCacheExistName(name) || CacheFactory.userCache.IsSQLExistName(name)) return -1;


            //3.修改昵称
            int acc_id = CacheFactory.accountCache.GetIdToToken(token);
            return CacheFactory.userCache.ReName(acc_id, name);
        }


        #region 关于图片的存储和加载


        /// <summary>
        /// 存储图片
        /// 0 成功 , -1失败
        /// </summary>
        public int SaveImage(UserToken token, ImageModel imaModel)
        {
            string account = CacheFactory.accountCache.GetAccountByToken(token);

            try
            {
                //  string path = "G:/20200319_MyFramwork_客户端 + 服务器/LocalLow"; //FileUtil.GetRunDirectory();

                string path = GameConfig.GetHeadImgDir();

                path += "/" + account + "/Head/";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                string filePath = path + "/" + imaModel.imgName;
                if (File.Exists(filePath))
                {
                    DebugUtil.Instance.LogToTime(imaModel.imgName + "  图片已存在", LogType.NOTICE);
                    File.Delete(filePath);
                }

                File.WriteAllBytes(filePath, imaModel.imgArr);

                return 0;
            }
            catch (Exception)
            {
                DebugUtil.Instance.LogToTime("图片存储失败", LogType.ERROR);
                return -1;
            }

        }


        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ImageInfo GetImage(UserToken token)
        {
            string account = CacheFactory.accountCache.GetAccountByToken(token);

            try
            {
                string path = FileUtil.GetRunDirectory();

                path += "/" + account + "/SaveImage/";

                if (!Directory.Exists(path)) return null;

                DirectoryInfo info = new DirectoryInfo(path);
                FileSystemInfo[] infoArr = info.GetFileSystemInfos();

                ImageInfo tmp = new ImageInfo();
                foreach (FileSystemInfo item in infoArr)
                {
                    FileInfo fi = item as FileInfo;
                    if (fi != null)
                    {
                        byte[] arr = File.ReadAllBytes(fi.FullName);
                        tmp.list.Add(new ImageModel(fi.Name, arr));
                    }
                }

                return tmp;
            }
            catch (Exception)
            {
                DebugUtil.Instance.LogToTime("Load Image Error", LogType.ERROR);
                return null;
            }
        }

        #endregion


        /// <summary>
        /// 更新用户数据
        /// 0 成功, -1 用户不存在 , -2 数据库错误
        /// </summary>
        /// <param name="token"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateInfo(UserToken token, UserModel model)
        {

            // 根据token获取账号id
            int acc_id = CacheFactory.accountCache.GetIdToToken(token);

            // 用户是否存在?
            bool result = CacheFactory.userCache.hasUser(acc_id);
            if (!result) return -1;

            USER tmp = CacheFactory.userCache.GetUserInfo(acc_id);
            tmp.nickName = model.nickname;
            tmp.phone = model.phone;
            tmp.sex = model.sex;
            tmp.mail = model.mail;
            tmp.birthday = model.birthday;

            bool re = tmp.Update();
            if (re) return 0;

            return -2;

        }


    }
}
