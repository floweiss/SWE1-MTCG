using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class TradeService : ITradeService
    {
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";
        private UserDataService _userDataService = new UserDataService();

        public string GetTradesFor(string usertoken)
        {
            throw new NotImplementedException();
        }

        public string GetAllTrades()
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            try
            {
                con.Open();
            }
            catch (PostgresException e)
            {
                return "GET ERR - No DB connection";
            }

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS trades(id VARCHAR(255), cardToTrade VARCHAR(255), type VARCHAR(255), minimumDamage DOUBLE PRECISION, usertoken VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlGetTrades = "SELECT * FROM trades";
            using NpgsqlCommand cmdGetTrades = new NpgsqlCommand(sqlGetTrades, con);
            using NpgsqlDataReader reader = cmdGetTrades.ExecuteReader();
            Dictionary<string, string> trades = new Dictionary<string, string>();
            while (reader.Read())
            {
                trades.Add(reader.GetString(0), "Card ID: " + reader.GetString(1) + " of Type " + reader.GetString(2) + " with Minimum Damage of " + reader.GetDouble(3) + " from user " + reader.GetString(4).Substring(0, reader.GetString(4).IndexOf("-")));
            }
            reader.Close();
            return JsonSerializer.Serialize(trades);
        }

        public string AddTrade(TradeDTO trade, string usertoken)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            try
            {
                con.Open();
            }
            catch (PostgresException e)
            {
                return "POST ERR - No DB connection";
            }

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS trades(id VARCHAR(255), cardToTrade VARCHAR(255), type VARCHAR(255), minimumDamage DOUBLE PRECISION, usertoken VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlGetTrades = "SELECT * FROM trades";
            using NpgsqlCommand cmdGetTrades = new NpgsqlCommand(sqlGetTrades, con);
            using NpgsqlDataReader reader = cmdGetTrades.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == trade.Id)
                {
                    return "POST ERR - TradeID already exists";
                }
            }
            reader.Close();

            User user = null;
            ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);
            Card card = user.Stack.GetCard(trade.CardToTrade);
            if (card == null)
            {
                return "POST ERR - Card to trade not in Stack";
            }

            var sql = "INSERT INTO trades (id, cardToTrade, type, minimumDamage, usertoken) VALUES (@id, @cardToTrade, @type, @minimumDamage, @usertoken)";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sql, con))
            {
                cmdPrepared.Parameters.AddWithValue("id", trade.Id);
                cmdPrepared.Parameters.AddWithValue("cardToTrade", trade.CardToTrade);
                cmdPrepared.Parameters.AddWithValue("type", trade.Type);
                cmdPrepared.Parameters.AddWithValue("minimumDamage", trade.MinimumDamage);
                cmdPrepared.Parameters.AddWithValue("usertoken", usertoken);
                cmdPrepared.ExecuteNonQuery();
            }

            con.Close();
            return "POST OK - Trade added";
        }

        public string TradeCards(string tradeID, string cardToTradeID, string usertoken)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            try
            {
                con.Open();
            }
            catch (PostgresException e)
            {
                return "POST ERR - No DB connection";
            }

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS trades(id VARCHAR(255), cardToTrade VARCHAR(255), type VARCHAR(255), minimumDamage DOUBLE PRECISION, usertoken VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlGetTrades = "SELECT * FROM trades";
            using NpgsqlCommand cmdGetTrades = new NpgsqlCommand(sqlGetTrades, con);
            using NpgsqlDataReader reader = cmdGetTrades.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == tradeID)
                {
                    if (reader.GetString(4) == usertoken)
                    {
                        return "POST ERR - Can not trade with yourself";
                    }
                    User user = null;
                    ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);

                    if (!ClientSingleton.GetInstance.ClientMap.ContainsKey(reader.GetString(4)))
                    {
                        return "POST ERR - User to trade with is not logged in";
                    }

                    User userToTradeWith = null;
                    ClientSingleton.GetInstance.ClientMap.TryGetValue(reader.GetString(4), out userToTradeWith);

                    // Cards
                    Card card = user.Stack.GetCard(JsonSerializer.Deserialize<string>(cardToTradeID));
                    if (card == null)
                    {
                        return "POST ERR - Card to trade not in Stack";
                    }
                    if (reader.GetDouble(3) > card.Damage || !card.GetType().ToString().ToLower().Contains(reader.GetString(2)))
                    {
                        return "POST ERR - Wrong type or too low damage for trade";
                    }

                    Card cardToTrade = userToTradeWith.Stack.GetCard(reader.GetString(1));
                    if (cardToTrade == null)
                    {
                        return "POST ERR - Card to trade for not in of other user Stack";
                    }

                    user.Stack.RemoveCard(card);
                    userToTradeWith.Stack.AddCard(card);
                    user.Stack.AddCard(cardToTrade);
                    userToTradeWith.Stack.RemoveCard(cardToTrade);

                    _userDataService.PersistUserData(user, usertoken);
                    _userDataService.PersistUserData(userToTradeWith, reader.GetString(4));
                    break;
                }
            }
            reader.Close();

            string sqlDelete =
                "DELETE FROM trades WHERE id = @id";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sqlDelete, con))
            {
                cmdPrepared.Parameters.AddWithValue("id", tradeID);
                cmdPrepared.ExecuteNonQuery();
            }

            return "POST OK - Cards traded";
        }

        public string RemoveTrade(string tradeID, string usertoken)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            try
            {
                con.Open();
            }
            catch (PostgresException e)
            {
                return "DELETE ERR - No DB connection";
            }

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS trades(id VARCHAR(255), cardToTrade VARCHAR(255), type VARCHAR(255), minimumDamage DOUBLE PRECISION, usertoken VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlGetTrades = "SELECT * FROM trades";
            using NpgsqlCommand cmdGetTrades = new NpgsqlCommand(sqlGetTrades, con);
            using NpgsqlDataReader reader = cmdGetTrades.ExecuteReader();
            bool tradeExistsAndAllowed = false;
            while (reader.Read())
            {
                if (reader.GetString(0) == tradeID)
                {
                    if (reader.GetString(4) == usertoken)
                    {
                        tradeExistsAndAllowed = true;
                        break;
                    }
                }
            }
            reader.Close();

            if (!tradeExistsAndAllowed)
            {
                return "DELETE ERR - Trade does not exists or is not your trade";
            }

            string sqlDelete =
                "DELETE FROM trades WHERE id = @id";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sqlDelete, con))
            {
                cmdPrepared.Parameters.AddWithValue("id", tradeID);
                cmdPrepared.ExecuteNonQuery();
            }

            return "DELETE OK - Trade deleted";
        }
    }
}
