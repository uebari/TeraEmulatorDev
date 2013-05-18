using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Data.DAO
{
    public abstract class BaseDAO
    {
        private string ConnectionString = string.Empty;

        public static MySqlConnection Connection;

        public BaseDAO(string con)
        {
            this.ConnectionString = con;
            Connection = new MySqlConnection(this.ConnectionString);

            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                Log.ErrorException("DAO ERROR: Can't connect to MySQL", e);
            }
            Connection.Close();
        }

        public byte[] HexToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public string BytesToHex(byte[] bytes)
        {
            return (bytes != null) ? BitConverter.ToString(bytes).Replace("-", "") : "";
        }
    }
}
