using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class GameConfig
    {

        // 0 服务器 , 1 本地
        public static int PPP_Debug = 1;


        /// <summary>
        /// 获取头像存储文件夹
        /// </summary>
        /// <returns></returns>
        public static string GetHeadImgDir()
        {
            if (PPP_Debug == 0)
                return "C:/Users/Administrator/Desktop/SERVERPROJECT/20200319_MyFramwork_客户端 + 服务器/LocalLow";
            else
                return "G:/20200319_MyFramwork_客户端 + 服务器/LocalLow";
        }





    }
}
