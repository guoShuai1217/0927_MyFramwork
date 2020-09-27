using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.dao
{
    public class USER
    {
        /// <summary>
        /// id
        /// </summary>
        public int acc_id;

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName;

        /// <summary>
        /// 性别 0女,1男,2未知
        /// </summary>
        public int sex;

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string mail;

        /// <summary>
        /// 头像
        /// </summary>
        public string head;

        /// <summary>
        /// 生日
        /// </summary>
        public string birthday;

        public USER()
        {
                
        }

        public USER(int acc_id,string nickName)
        {
            this.acc_id = acc_id;
            this.nickName = nickName;
            this.phone = "";
            this.sex = 2;
            this.mail = "";
            this.head = "";
            this.birthday = "";      
        }


        #region  Method

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public USER(int accountId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select acc_id,nickname,sex,phone,mail,head,birthday ");
            strSql.Append(" FROM USER ");
            strSql.Append(" where acc_id=@acc_id ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@acc_id", MySqlDbType.Int32)};
            parameters[0].Value = accountId;

            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["acc_id"].ToString()))
                {
                    this.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["nickname"] != null)
                {
                    this.nickName = ds.Tables[0].Rows[0]["nickname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sex"] != null && ds.Tables[0].Rows[0]["sex"].ToString() != "")
                {
                    this.sex = int.Parse(ds.Tables[0].Rows[0]["sex"].ToString());
                }
                if (ds.Tables[0].Rows[0]["phone"] != null && ds.Tables[0].Rows[0]["phone"].ToString() != "")
                {
                    this.phone = ds.Tables[0].Rows[0]["phone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["mail"] != null && ds.Tables[0].Rows[0]["mail"].ToString() != "")
                {
                    this.mail = ds.Tables[0].Rows[0]["mail"].ToString();
                }
                if (ds.Tables[0].Rows[0]["head"] != null && ds.Tables[0].Rows[0]["head"].ToString() != "")
                {
                    this.head = ds.Tables[0].Rows[0]["head"].ToString();
                }
                if (ds.Tables[0].Rows[0]["birthday"] != null && ds.Tables[0].Rows[0]["birthday"].ToString() != "")
                {
                    this.birthday = ds.Tables[0].Rows[0]["birthday"].ToString();
                }
               
            }
        }


      


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string nickname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select acc_id from USER");
            strSql.Append(" where nickname=@nickname ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@nickname", MySqlDbType.VarChar,255)};
            parameters[0].Value = nickname;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into USER (");
            strSql.Append("acc_id,nickname,sex,phone,mail,head,birthday)");
            strSql.Append(" values (");
            strSql.Append("@acc_id,@nickname,@sex,@phone,@mail,@head,@birthday)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@acc_id", MySqlDbType.Int32,11),
                    new MySqlParameter("@nickname", MySqlDbType.VarChar,255),
                    new MySqlParameter("@sex", MySqlDbType.Int32,11),
                    new MySqlParameter("@phone", MySqlDbType.VarChar,255),
                    new MySqlParameter("@mail", MySqlDbType.VarChar,255),
                    new MySqlParameter("@head", MySqlDbType.VarChar,255),
                    new MySqlParameter("@birthday", MySqlDbType.VarChar,255)
            };

            parameters[0].Value = acc_id;
            parameters[1].Value = nickName;
            parameters[2].Value = sex;
            parameters[3].Value = phone;
            parameters[4].Value = mail;
            parameters[5].Value = head;
            parameters[6].Value = birthday;

            DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            //getKey();
        }

        //void getKey()
        //{
        //    DataSet ds = DbHelperMySQL.Query("select @@IDENTITY as id");
        //    if (ds.Tables[0].Rows[0]["acc_id"] != null && ds.Tables[0].Rows[0]["acc_id"].ToString() != string.Empty)
        //    {
        //        this.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
        //    }
        //}
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update USER set ");
            strSql.Append("nickname=@nickname,");
            strSql.Append("sex=@sex,");
            strSql.Append("phone=@phone,");
            strSql.Append("mail=@mail,");
            strSql.Append("head=@head,");
            strSql.Append("birthday=@birthday");
            strSql.Append(" where acc_id=@acc_id ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@nickname", MySqlDbType.VarChar,255),
                    new MySqlParameter("@sex", MySqlDbType.Int32,11),
                    new MySqlParameter("@phone", MySqlDbType.VarChar,255),
                    new MySqlParameter("@mail", MySqlDbType.VarChar,255),
                    new MySqlParameter("@head", MySqlDbType.VarChar,255),
                    new MySqlParameter("@birthday", MySqlDbType.VarChar,255),
                    new MySqlParameter("@acc_id", MySqlDbType.Int32,11)
            };
            parameters[0].Value = nickName;
            parameters[1].Value = sex;
            parameters[2].Value = phone;
            parameters[3].Value = mail;
            parameters[4].Value = head;
            parameters[5].Value = birthday;
            parameters[6].Value = acc_id;
 
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from USER ");
            strSql.Append(" where acc_id=@id ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@id", MySqlDbType.Int32)};
            parameters[0].Value = id;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public void GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select acc_id,nickname,sex,phone,mail,head,birthday ");
            strSql.Append(" FROM USER ");
            strSql.Append(" where acc_id=@id ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@id", MySqlDbType.Int32)};
            parameters[0].Value = id;

            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["acc_id"] != null && ds.Tables[0].Rows[0]["acc_id"].ToString() != "")
                {
                    this.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["nickname"] != null)
                {
                    this.nickName = ds.Tables[0].Rows[0]["nickname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sex"] != null && ds.Tables[0].Rows[0]["sex"].ToString() != "")
                {
                    this.sex = int.Parse(ds.Tables[0].Rows[0]["sex"].ToString());
                }
                if (ds.Tables[0].Rows[0]["phone"] != null && ds.Tables[0].Rows[0]["phone"].ToString() != "")
                {
                    this.phone = ds.Tables[0].Rows[0]["phone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["mail"] != null && ds.Tables[0].Rows[0]["mail"].ToString() != "")
                {
                    this.mail = ds.Tables[0].Rows[0]["mail"].ToString();
                }
                if (ds.Tables[0].Rows[0]["head"] != null && ds.Tables[0].Rows[0]["head"].ToString() != "")
                {
                    this.head = ds.Tables[0].Rows[0]["head"].ToString();
                }
                if (ds.Tables[0].Rows[0]["birthday"] != null && ds.Tables[0].Rows[0]["birthday"].ToString() != "")
                {
                    this.birthday = ds.Tables[0].Rows[0]["birthday"].ToString();
                }
               
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM USER ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        #endregion  Method

    }
}
