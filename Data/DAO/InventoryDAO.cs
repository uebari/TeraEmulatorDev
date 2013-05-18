using Data.Enums.Item;
using Data.Structures.Account;
using Data.Structures.Player;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Data.DAO
{
    public class InventoryDAO : BaseDAO
    {
        private MySqlConnection InventoryDAOConnection;

        public InventoryDAO(string conStr)
            : base(conStr)
        {
            InventoryDAOConnection = new MySqlConnection(conStr);
            InventoryDAOConnection.Open();
            Log.Info("DAO: Inventory DAO Started!");
        }

        public bool AddItem(Player player, StorageType type, KeyValuePair<int, StorageItem> KeyVP)
        {
            string SQL = "SELECT * FROM `inventory` WHERE `playerid` = ?pid AND `itemid` = ?itemid AND SLOT = ?slot";
            MySqlCommand cmd = new MySqlCommand(SQL, InventoryDAOConnection);
            cmd.Parameters.AddWithValue("?pid", player.pid);
            cmd.Parameters.AddWithValue("?itemid", KeyVP.Value.ItemId);
            cmd.Parameters.AddWithValue("?slot", KeyVP.Key);
            MySqlDataReader InventoryAddReader = cmd.ExecuteReader();
            bool isExists = InventoryAddReader.HasRows;

            if (!isExists)
            {
                SQL = "INSERT INTO `inventory` "
                    + "(`accountname`, `playerid`, `itemid`, `amount`, `color`, `slot`, `storagetype`) "
                    + "VALUES(?accountname, ?pid, ?itemid, ?count, ?color, ?slot, ?type);";
                cmd = new MySqlCommand(SQL, InventoryDAOConnection);
                cmd.Parameters.AddWithValue("?accountname", player.AccountName);
                cmd.Parameters.AddWithValue("?pid", player.pid);
                cmd.Parameters.AddWithValue("?itemid", KeyVP.Value.ItemId);
                cmd.Parameters.AddWithValue("?count", KeyVP.Value.Count);
                cmd.Parameters.AddWithValue("?color", KeyVP.Value.Color);
                cmd.Parameters.AddWithValue("?slot", KeyVP.Key);
                cmd.Parameters.AddWithValue("?type", type.ToString());
            }
            else
            {
                SQL = "UPDATE `inventory` SET "
                    + "`amount` = ?count, `color` = ?color, `slot` = ?slot, `storagetype` = ?type "
                    + "WHERE `itemid` = ?itemid and `playerid` = ?pid";
                cmd = new MySqlCommand(SQL, InventoryDAOConnection);
                cmd.Parameters.AddWithValue("?count", KeyVP.Value.Count);
                cmd.Parameters.AddWithValue("?color", KeyVP.Value.Color);
                cmd.Parameters.AddWithValue("?slot", KeyVP.Key);
                cmd.Parameters.AddWithValue("?type", type.ToString());
                cmd.Parameters.AddWithValue("?itemid", KeyVP.Value.ItemId);
                cmd.Parameters.AddWithValue("?pid", player.pid);
            }
            InventoryAddReader.Close();
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Log.ErrorException("DAO: ADD ITEM ERROR!", e);
            }
            return false;
        }

        public bool SaveStorage(Player player, Storage storage)
        {
            if (storage.Items.Count > 0)
            {
                foreach (var item in storage.Items)
                    AddItem(player, storage.StorageType, item);

                return true;
            }
            return false;
        }

        public Dictionary<int, StorageItem> LoadStorage(Player player, StorageType type)
        {
            string SQL = "SELECT * FROM `inventory` WHERE "
                + "`playerid` = ?pid AND `storagetype` = ?type";
            MySqlCommand cmd = new MySqlCommand(SQL, InventoryDAOConnection);
            cmd.Parameters.AddWithValue("?pid", player.pid);
            cmd.Parameters.AddWithValue("?type", type.ToString());
            MySqlDataReader LoadStorageReader = cmd.ExecuteReader();

            Dictionary<int, StorageItem> items = new Dictionary<int, StorageItem>();
            if (LoadStorageReader.HasRows)
            {
                while (LoadStorageReader.Read())
                {
                    StorageItem item = new StorageItem()
                    {
                        ItemId = LoadStorageReader.GetInt32(3),
                        Count = LoadStorageReader.GetInt32(4),
                        Color = LoadStorageReader.GetInt32(5),
                    };
                    items.Add(LoadStorageReader.GetInt32(6), item);
                }
            }
            LoadStorageReader.Close();

            return items;
        }

        public Dictionary<int, StorageItem> LoadAccountStorage(Account account)
        {
            string SQL = "SELECT * FROM `inventory` WHERE "
                + "`accountname` = ?accountname AND `storagetype` = ?type";
            MySqlCommand cmd = new MySqlCommand(SQL, InventoryDAOConnection);
            cmd.Parameters.AddWithValue("?accountname", account.Name);
            cmd.Parameters.AddWithValue("?type", StorageType.AccountWarehouse.ToString());
            MySqlDataReader LoadAccountStorageReader = cmd.ExecuteReader();

            Dictionary<int, StorageItem> items = new Dictionary<int, StorageItem>();
            if (LoadAccountStorageReader.HasRows)
            {
                while (LoadAccountStorageReader.Read())
                {
                    StorageItem item = new StorageItem()
                    {
                        ItemId = LoadAccountStorageReader.GetInt32(3),
                        Count = LoadAccountStorageReader.GetInt32(4),
                        Color = LoadAccountStorageReader.GetInt32(5),
                    };
                    items.Add(LoadAccountStorageReader.GetInt32(6), item);
                }
            }
            LoadAccountStorageReader.Close();

            return items;
        }
    }
}
