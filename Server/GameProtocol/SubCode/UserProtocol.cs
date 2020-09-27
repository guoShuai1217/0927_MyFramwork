using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class UserProtocol
    {
        /// <summary>
        /// 玩家登陆后获取玩家信息
        /// req:null
        /// </summary>
        public const int GETINFO_CREQ = 2001;
        /// <summary>
        /// 返回玩家信息
        /// 
        /// </summary>
        public const int GETINFO_SRES = 2002;

        /// <summary>
        /// 上传图片
        /// </summary>
        public const int UPDATEIMG_CREQ = 2003;

        /// <summary>
        /// 上传图片结果
        /// </summary>
        public const int UPDATEIMG_SRES = 2004;

        /// <summary>
        /// 加载图片请求
        /// </summary>
        public const int LOADIMG_CREQ = 2005;

        /// <summary>
        /// 加载图片回复
        /// </summary>
        public const int LAODIMG_SRES = 2006;

        /// <summary>
        /// 重命名请求
        /// </summary>
        public const int RENAME_CREQ = 2007;

        /// <summary>
        /// 重命名回复
        /// </summary>
        public const int RENAME_SRES = 2008;

        /// <summary>
        /// 修改个人信息请求
        /// </summary>
        public const int UPDATEINFO_CREQ = 2009;
        /// <summary>
        /// 修改个人信息回复
        /// </summary>
        public const int UPDATEINFO_SRES = 2010;


    }
}
