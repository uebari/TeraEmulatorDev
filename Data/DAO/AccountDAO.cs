using Data.Structures.Account;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Data.DAO
{
    public class AccountDAO : BaseDAO
    {
        private MySqlConnection AccountDAOConnection;

        public AccountDAO(string conStr)
            : base(conStr)
        {
            AccountDAOConnection = new MySqlConnection(conStr);
            AccountDAOConnection.Open();
            Log.Info("DAO: Account DAO Started!");
        }

        public int LoadAccountCount()
        {
            string SQL = "SELECT COUNT(*) FROM `accounts`";
            MySqlCommand cmd = new MySqlCommand(SQL, AccountDAOConnection);
            MySqlDataReader LoadAccountCountReader = cmd.ExecuteReader();

            int count = 0;
            while (LoadAccountCountReader.Read())
            {
                count = LoadAccountCountReader.GetInt32(0);
            }

            LoadAccountCountReader.Close();

            return count;
        }

        public Account LoadAccount(string username)
        {
            string SQL = "SELECT * FROM `accounts` WHERE `name` = ?username";
            MySqlCommand cmd = new MySqlCommand(SQL, AccountDAOConnection);
            cmd.Parameters.AddWithValue("?username", username);
            MySqlDataReader AccountReader = cmd.ExecuteReader();

            Account acc = new Account();
            if (AccountReader.HasRows)
            {
                while (AccountReader.Read())
                {
                    acc.AccountId = AccountReader.GetInt32(0);
                    acc.Name = AccountReader.GetString(1);
                    acc.Password = AccountReader.GetString(2);
                    acc.AccessLevel = (byte)AccountReader.GetInt32(3);
                    acc.Membership = (byte)AccountReader.GetInt32(4);
                    acc.LastOnlineUtc = AccountReader.GetInt64(5);
                }
            }

            AccountReader.Close();

            return (acc.Name == "") ? null : acc;
        }

        public bool SaveAccount(Account account)
        {
            string SQL = "INSERT INTO `accounts` (`name`,`password`) VALUES(?name, ?password);";
            MySqlCommand cmd = new MySqlCommand(SQL, AccountDAOConnection);
            cmd.Parameters.AddWithValue("?name", account.Name);
            cmd.Parameters.AddWithValue("?password", account.Password);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Log.ErrorException("DAO: SAVE ACCOUNT ERROR!", e);
            }
            return false;
        }
    }
}
