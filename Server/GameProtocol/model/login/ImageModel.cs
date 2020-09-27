using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.model.login
{

    [Serializable]
    public class ImageInfo
    {
        public List<ImageModel> list = new List<ImageModel>();
    }


    [Serializable]
    public class ImageModel
    {
        /// <summary>
        /// 图片名
        /// </summary>
        public string imgName;

        /// <summary>
        /// 图片二进制流
        /// </summary>
        public byte[] imgArr;


        public ImageModel(string imgName,byte[] imgArr)
        {
            this.imgName = imgName;
            this.imgArr = imgArr;
        }

    }
}
