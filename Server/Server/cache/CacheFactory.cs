using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.cache
{
    /// <summary>
    /// 缓存代理
    /// </summary>
    public class CacheFactory
    {
        /// <summary>
        /// 建立一个用户数据缓存
        /// </summary>
        public readonly static AccountCache accountCache;

        public readonly static UserCache userCache;
        
        static CacheFactory()
        {
            accountCache = new AccountCache();

            userCache = new UserCache();
        }
    }
}
