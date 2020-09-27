using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.model.login
{
    /// <summary>
    /// 用户信息模型
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string account = "";
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname = "";
        /// <summary>
        /// 用户ID
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 头像
        /// </summary>
        public string head = "";
      
        /// <summary>
        /// 性别
        /// </summary>
        public int sex = 0;
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone = "";

        /// <summary>
        /// 邮箱
        /// </summary>
        public string mail = "";

        /// <summary>
        /// 生日
        /// </summary>
        public string birthday = "";

 
        ///// <summary>
        ///// 金币
        ///// </summary>
        //public int coin = 0;
        ///// <summary>
        ///// 钻石
        ///// </summary>
        //public int cash = 0;

    }
}
