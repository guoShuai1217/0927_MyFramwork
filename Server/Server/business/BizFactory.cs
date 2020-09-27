using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.business
{
    /// <summary>
    /// 业务逻辑代理
    /// </summary>
    public class BizFactory
    {
        //readonly  和 const  都是表示只读的  
        //readonly可以修饰静态类  
        //const只能修饰变量  
        //建立一个公开的只读的静态类
        /// <summary>
        /// 登录业务逻辑处理类
        /// </summary>
        public readonly static AccountBiz account;
        /// <summary>
        /// 用户业务逻辑处理类
        /// </summary>
        public readonly static UserBiz user;
      
        static BizFactory()
        {
            account = new AccountBiz();
            user = new UserBiz();
         
        }
    }
}
