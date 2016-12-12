using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace demo.Models
{
    public class UserModel
    {
        private SqlConnection conn;
        private SqlCommand com = new SqlCommand();
        private SqlDataReader sdr = null;

        public UserModel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;
            conn = new SqlConnection(connectionString);
            try
            {
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool authenticate(string username, string password)
        {
            string sql = "select password from [user] where username = @username";
            com.CommandText = sql;
            com.CommandType = CommandType.Text;
            com.Connection = conn;
            com.Parameters.Add("@username", SqlDbType.VarChar);
            com.Parameters["@username"].Value = username;
            sdr = com.ExecuteReader();
            if (sdr.Read())
            {
                if (sdr["password"].Equals(password)) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
    }
}