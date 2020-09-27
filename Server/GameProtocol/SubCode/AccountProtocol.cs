using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    /// <summary>
    /// 登录模块行为协议
    /// </summary>
    public class AccountProtocol
    {
        
        /// <summary>
        /// 注册请求
        /// </summary>
        public const int REGISTER_CREQ = 1010;

        /// <summary>
        /// 注册结果回复
        /// </summary>
        public const int REGISTER_SRES = 1011;

        /// <summary>
        /// 登陆请求
        /// </summary>
        public const int LOGIN_CREQ = 1012;

        /// <summary>
        /// 登陆结果回复
        /// </summary>
        public const int LOGIN_SRES = 1013;

        /// <summary>
        /// 退出登录请求
        /// </summary>
        public const int OFFLINE_CREQ = 1014;

        /// <summary>
        /// 退出登录回复
        /// </summary>
        public const int OFFLINE_SRES = 1015;

    
    }
}
