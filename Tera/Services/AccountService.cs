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
            connection.Account.AccountWarehouse = DAOManager.inventoryDAO.LoadAccountStorage(connection.Account);
            connection.Account.Players = Communication.Global.PlayerService.OnAuthorized(connection.Account);
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
