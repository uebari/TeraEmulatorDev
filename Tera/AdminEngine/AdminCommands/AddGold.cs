using System;
using Communication;
using Data.Enums;
using Data.Interfaces;
using Data.Structures.Player;
using Data.Structures.World;
using Network.Server;
using Tera.Services;
using Utils;

namespace Tera.AdminEngine.AdminCommands
{
    internal class AddGold : ACommand
    {
        public override void Process(IConnection connection, string msg)
        {
            var args = msg.Split(' ');
            int goldAmount = 0;

            try
            {
                // Do have a target ?
                if (int.TryParse(args[0], out goldAmount))
                {
                    // We are Singular!
                    Global.StorageService.AddMoneys(connection.Player, connection.Player.Inventory,
                    int.Parse(args[0]));
                }
                else
                {
                    var target = Communication.Global.PlayerService.GetPlayerByName(args[0]);
                    goldAmount = int.Parse(args[1]);
                    Global.StorageService.AddMoneys(target, target.Inventory,
                    goldAmount);
                }
            }
            catch (Exception e)
            {
                new SpChatMessage("Wrong Syntax!\n Type `addgold {player} {number}", ChatType.Notice).Send(connection);
                Log.Warn(e.ToString());
            }
        }
    }
}
