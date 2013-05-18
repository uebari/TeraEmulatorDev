using Data.Enums;
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
    public class PlayerDAO : BaseDAO
    {
        private MySqlConnection PlayerDAOConnection;

        public PlayerDAO(string conStr)
            : base(conStr)
        {
            PlayerDAOConnection = new MySqlConnection(conStr);
            PlayerDAOConnection.Open();
            Log.Info("DAO: PlayerDAO Initialized.");
        }

        public List<Player> LoadAccountPlayers(string accName)
        {
            string cmdString = "SELECT * FROM `character` WHERE AccountName=?username AND deleted = ?delete";
            MySqlCommand command = new MySqlCommand(cmdString, PlayerDAOConnection);
            command.Parameters.AddWithValue("?username", accName);
            command.Parameters.AddWithValue("?delete", 0);
            MySqlDataReader reader = command.ExecuteReader();

            List<Player> players = new List<Player>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Player player = new Player()
                    {
                        pid = reader.GetInt32(0),
                        AccountName = reader.GetString(1),
                        Level = reader.GetInt32(2),
                        Exp = reader.GetInt64(3),
                        ExpRecoverable = reader.GetInt64(4),
                        Mount = reader.GetInt32(5),
                        UiSettings = (reader.GetString(6) != null) ? HexToBytes(reader.GetString(6)) : new byte[0],
                        GuildAccepted = (byte)reader.GetInt16(7),
                        PraiseGiven = (byte)reader.GetInt16(8),
                        LastPraise = reader.GetInt32(9),
                        CurrentBankSection = reader.GetInt32(10),
                        CreationDate = reader.GetInt32(11),
                        LastOnline = reader.GetInt32(12)
                    };
                    players.Add(player);
                }
            }
            reader.Close();

            foreach (var player in players)
            {
                cmdString = "SELECT * FROM character_data WHERE PlayerId=?id";
                command = new MySqlCommand(cmdString, PlayerDAOConnection);
                command.Parameters.AddWithValue("?id", player.pid);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        player.PlayerData = new PlayerData()
                        {
                            Name = reader.GetString(1),
                            Gender = (Gender)Enum.Parse(typeof(Gender), reader.GetString(2)),
                            Race = (Race)Enum.Parse(typeof(Race), reader.GetString(3)),
                            Class = (PlayerClass)Enum.Parse(typeof(PlayerClass), reader.GetString(4)),
                            Data = HexToBytes(reader.GetString(5)),
                            Details = HexToBytes(reader.GetString(6)),
                        };

                        player.Position = new Structures.World.WorldPosition()
                        {
                            MapId = reader.GetInt32(7),
                            X = reader.GetFloat(8),
                            Y = reader.GetFloat(9),
                            Z = reader.GetFloat(10),
                            Heading = reader.GetInt16(11)
                        };
                    }
                }
                reader.Close();
            }


            return players;
        }

        public int SaveNewPlayer(Player player)
        {
            string cmdString = "INSERT INTO `character` "
            + "(`AccountName`,`Level`,`Exp`,`ExpRecoverable`,`Mount`,`UiSettings`,`GuildAccepted`,`PraiseGiven`,`LastPraise`,`CurrentBankSection`,`CreationDate`, `deleted`) "
            + "VALUES (?accname, ?lvl, ?exp, ?exprev, ?mount, ?uiset, ?gaccept, ?praisgive, ?lastpraise, ?bank, ?credate, ?delete); SELECT LAST_INSERT_ID();";
            MySqlCommand command = new MySqlCommand(cmdString, PlayerDAOConnection);
            command.Parameters.AddWithValue("?accname", player.AccountName.ToLower());
            command.Parameters.AddWithValue("?lvl", player.Level);
            command.Parameters.AddWithValue("?exp", player.Exp);
            command.Parameters.AddWithValue("?exprev", player.ExpRecoverable);
            command.Parameters.AddWithValue("?mount", player.Mount);
            command.Parameters.AddWithValue("?uiset", BytesToHex(player.UiSettings));
            command.Parameters.AddWithValue("?gaccept", player.GuildAccepted);
            command.Parameters.AddWithValue("?praisgive", player.PraiseGiven);
            command.Parameters.AddWithValue("?lastpraise", player.LastPraise);
            command.Parameters.AddWithValue("?bank", player.CurrentBankSection);
            command.Parameters.AddWithValue("?credate", player.CreationDate);
            command.Parameters.AddWithValue("?delete", 0);

            int id;
            try
            {
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Log.ErrorException("Character Save Error!", ex);
                return 0;
            }

            cmdString = "INSERT INTO character_data "
            + "(`PlayerId`,`Name`,`Gender`,`Race`,`PlayerClass`,`Data`,`Details`,`MapId`,`X`,`Y`,`Z`,`H`) "
            + "VALUES (?id, ?name, ?gender, ?race, ?class, ?data, ?details, ?mapid, ?x, ?y, ?z, ?h);";
            command = new MySqlCommand(cmdString, PlayerDAOConnection);
            command.Parameters.AddWithValue("?id", id);
            command.Parameters.AddWithValue("?name", player.PlayerData.Name);
            command.Parameters.AddWithValue("?gender", player.PlayerData.Gender.ToString());
            command.Parameters.AddWithValue("?race", player.PlayerData.Race.ToString());
            command.Parameters.AddWithValue("?class", player.PlayerData.Class.ToString());
            command.Parameters.AddWithValue("?data", BytesToHex(player.PlayerData.Data));
            command.Parameters.AddWithValue("?details", BytesToHex(player.PlayerData.Details));
            command.Parameters.AddWithValue("?mapid", player.Position.MapId);
            command.Parameters.AddWithValue("?x", player.Position.X);
            command.Parameters.AddWithValue("?y", player.Position.Y);
            command.Parameters.AddWithValue("?z", player.Position.Z);
            command.Parameters.AddWithValue("?h", player.Position.Heading);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Log.ErrorException("Character Data Save Error!", ex);
                return 0;
            }

            return id;
        }

        public void RemovePlayer(int playerId)
        {
            string cmdString = "UPDATE `character` SET `deleted` = 1 WHERE `id` = ?pid";
            MySqlCommand command = new MySqlCommand(cmdString, PlayerDAOConnection);
            command.Parameters.AddWithValue("?pid", playerId);

            try
            {
                Log.Info("Player Reqlinquished On PID : {0}", playerId);
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Log.ErrorException("Character Relinquish Failed!", e);
                return;
            }
        }

        public void UpdatePlayer(Player player)
        {
            string cmdString = "UPDATE `character` SET"
            + "`AccountName`=?accname,`Level`=?lvl,`Exp`=?exp,`ExpRecoverable`=?exprev,`Mount`=?mount,`UiSettings`=?uiset,`GuildAccepted`=?gaccept,`PraiseGiven`=?praisgive,`LastPraise`=?lastpraise,`CurrentBankSection`=?bank,`CreationDate`=?credate "
            + "WHERE Id=?id";
            MySqlCommand command = new MySqlCommand(cmdString, PlayerDAOConnection);
            command.Parameters.AddWithValue("?accname", player.AccountName.ToLower());
            command.Parameters.AddWithValue("?lvl", player.Level);
            command.Parameters.AddWithValue("?exp", player.Exp);
            command.Parameters.AddWithValue("?exprev", player.ExpRecoverable);
            command.Parameters.AddWithValue("?mount", player.Mount);
            command.Parameters.AddWithValue("?uiset", BytesToHex(player.UiSettings));
            command.Parameters.AddWithValue("?gaccept", player.GuildAccepted);
            command.Parameters.AddWithValue("?praisgive", player.PraiseGiven);
            command.Parameters.AddWithValue("?lastpraise", player.LastPraise);
            command.Parameters.AddWithValue("?bank", player.CurrentBankSection);
            command.Parameters.AddWithValue("?credate", player.CreationDate);
            command.Parameters.AddWithValue("?id", player.pid);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Log.ErrorException("Character Update Error!", ex);
                return;
            }

            cmdString = "UPDATE character_data SET "
            + "`Data`=?data,`Details`=?details,`MapId`=?mapid,`X`=?x,`Y`=?y,`Z`=?z,`H`=?h WHERE `PlayerId`=?pid";
            command = new MySqlCommand(cmdString, PlayerDAOConnection);
            command.Parameters.AddWithValue("?data", BytesToHex(player.PlayerData.Data));
            command.Parameters.AddWithValue("?details", BytesToHex(player.PlayerData.Details));
            command.Parameters.AddWithValue("?mapid", player.Position.MapId);
            command.Parameters.AddWithValue("?x", player.Position.X);
            command.Parameters.AddWithValue("?y", player.Position.Y);
            command.Parameters.AddWithValue("?z", player.Position.Z);
            command.Parameters.AddWithValue("?h", player.Position.Heading);
            command.Parameters.AddWithValue("?pid", player.pid);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Log.ErrorException("Chatacter Data Update Error!", ex);
                return;
            }
        }
    }
}
