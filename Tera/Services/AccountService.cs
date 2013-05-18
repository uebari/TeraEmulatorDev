using Communication.Interfaces;
using Data;
using Data.Interfaces;
using Data.DAO;
using Data.Enums.Item;
using Utils;

namespace Tera.Services
{
    class AccountService : IAccountService
    {
        public void Authorized(IConnection connection, string accountName)
        {
            //connection.Account = Cache.GetAccount(accountName);
            connection.Account = DAOManager.accountDAO.LoadAccount(accountName);
            connection.Account.Players = DAOManager.playerDAO.LoadAccountPlayers(accountName);
            connection.Account.AccountWarehouse.Items = DAOManager.inventoryDAO.LoadAccountStorage(connection.Account);
            foreach (var player in connection.Account.Players)
            {
                player.Inventory.Items = DAOManager.inventoryDAO.LoadStorage(player, StorageType.Inventory);
                player.CharacterWarehouse.Items = DAOManager.inventoryDAO.LoadStorage(player, StorageType.CharacterWarehouse);
                player.Quests = DAOManager.questDAO.LoadQuests(player);
            }
        }

        public void AbortExitAction(IConnection connection)
        {
            if (connection.Account.ExitAction != null)
            {
                connection.Account.ExitAction.Abort();
                connection.Account.ExitAction = null;
            }
        }

        public void Action()
        {
            
        }
    }
}
