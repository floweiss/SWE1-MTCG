using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class UserDataService : IUserDataService
    {
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

        public void PersistUserData(User user, string usertoken)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            try
            {
                con.Open();
            }
            catch (PostgresException e)
            {
                return;
            }

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS userdata(token VARCHAR(255), coins INTEGER, elo INTEGER, deck VARCHAR(255), stack VARCHAR(800))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM userdata";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            bool updateUser = false;
            while (reader.Read())
            {
                if (reader.GetString(0) == usertoken)
                {
                    updateUser = true;
                }
            }
            reader.Close();
            if (updateUser)
            {
                string sqlUpdate =
                    "UPDATE userdata SET token = @token, coins = @coins, elo = @elo, deck = @deck, stack = @stack WHERE token = @findToken";
                using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sqlUpdate, con))
                {
                    cmdPrepared.Parameters.AddWithValue("token", usertoken);
                    cmdPrepared.Parameters.AddWithValue("findToken", usertoken);
                    cmdPrepared.Parameters.AddWithValue("coins", user.Coins);
                    cmdPrepared.Parameters.AddWithValue("elo", user.ELO);
                    cmdPrepared.Parameters.AddWithValue("deck", "{ \"CardIds\":" + JsonSerializer.Serialize(user.Deck.ToCardIds()) + "}");
                    cmdPrepared.Parameters.AddWithValue("stack", "{ \"CardIds\":" + JsonSerializer.Serialize(user.Stack.ToCardIds()) + "}");
                    cmdPrepared.ExecuteNonQuery();
                }
            }
            else
            {
                var sqlInsert = "INSERT INTO userdata (token, coins, elo, deck, stack) VALUES (@token, @coins, @elo, @deck, @stack)";
                using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sqlInsert, con))
                {
                    cmdPrepared.Parameters.AddWithValue("token", usertoken);
                    cmdPrepared.Parameters.AddWithValue("coins", user.Coins);
                    cmdPrepared.Parameters.AddWithValue("elo", user.ELO);
                    cmdPrepared.Parameters.AddWithValue("deck", "{ \"CardIds\":" + JsonSerializer.Serialize(user.Deck.ToCardIds()) + "}");
                    cmdPrepared.Parameters.AddWithValue("stack", "{ \"CardIds\":" + JsonSerializer.Serialize(user.Stack.ToCardIds()) + "}");
                    cmdPrepared.ExecuteNonQuery();
                }
            }
        }

        public void LoadUserData(User user, string usertoken)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            try
            {
                con.Open();
            }
            catch (PostgresException e)
            {
                return;
            }

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS userdata(token VARCHAR(255), coins INTEGER, elo INTEGER, deck VARCHAR(255), stack VARCHAR(800))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM userdata";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            List<string> deckIds = new List<string>();
            List<string> stackIds = new List<string>();
            while (reader.Read())
            {
                if (reader.GetString(0) == usertoken)
                {
                    user.Coins = reader.GetInt32(1);
                    user.ELO = reader.GetInt32(2);
                    deckIds = JsonSerializer.Deserialize<CardIdsDTO>(reader.GetString(3)).CardIds;
                    stackIds = JsonSerializer.Deserialize<CardIdsDTO>(reader.GetString(4)).CardIds;
                }
            }
            reader.Close();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS cards(id VARCHAR(255), name VARCHAR(255), cardtype VARCHAR(255), element VARCHAR(255), damage DOUBLE PRECISION)";
            cmd.ExecuteNonQuery();
            string sqlAllCards = "SELECT * FROM cards";
            using NpgsqlCommand cmdCards = new NpgsqlCommand(sqlAllCards, con);
            using NpgsqlDataReader readerCards = cmdCards.ExecuteReader();
            while (readerCards.Read())
            {
                foreach (var cardId in deckIds)
                {
                    if (readerCards.GetString(0) == cardId)
                    {
                        CardDTO cardDto = new CardDTO(readerCards.GetString(0), readerCards.GetString(1), readerCards.GetString(2), readerCards.GetString(3), readerCards.GetDouble(4));
                        Card card = cardDto.ToCard();
                        user.Deck.AddCard(card);
                    }
                }

                foreach (var cardId in stackIds)
                {
                    if (readerCards.GetString(0) == cardId)
                    {
                        CardDTO cardDto = new CardDTO(readerCards.GetString(0), readerCards.GetString(1), readerCards.GetString(2), readerCards.GetString(3), readerCards.GetDouble(4));
                        Card card = cardDto.ToCard();
                        user.Stack.AddCard(card);
                    }
                }
            }

            ClientSingleton.GetInstance.ClientMap.AddOrUpdate(usertoken, user, (key, oldValue) => user);
        }
    }
}
