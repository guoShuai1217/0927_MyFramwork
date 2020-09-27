using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.model.login
{
    /// <summary>
    /// 账号模型
    /// </summary>
    [System.Serializable]
    public class AccountModel
    {

        public string Account;

        public string Password;

        public AccountModel(string Account, string psd)
        {
            this.Account = Account;
            this.Password = psd;
        }

    }
}
