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
    public class UserService : IUserService
    {
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=mtcg-db";
        public string Register(User user)
        {
            // http://zetcode.com/csharp/postgresql/
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM users";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == user.Username)
                {
                    return "POST ERR - User already exists";
                }
            }
            reader.Close();

            var sql = "INSERT INTO users (username, password) VALUES (@username, @password)";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sql, con))
            {
                cmdPrepared.Parameters.AddWithValue("username", user.Username);
                cmdPrepared.Parameters.AddWithValue("password", user.HashedPW);
                cmdPrepared.ExecuteNonQuery();
            }

            con.Close();
            return "POST OK by DB";
        }

        public string Login(User user)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM users";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == user.Username)
                {
                    if (reader.GetString(1) == user.HashedPW)
                    {
                        string userToken = user.Username + "-mtcgToken";
                        ClientSingleton.GetInstance.ClientMap.AddOrUpdate(userToken, user, (key, oldValue) => user);
                        return "POST OK - Authentication-Token: "+userToken;
                    }
                }
            }
            reader.Close();
            return "POST ERR - Login failed";
        }

        public bool isLoggedIn(RequestContext request)
        {
            if (request.CustomHeader.ContainsKey("Authorization"))
            {
                string userToken = request.CustomHeader["Authorization"];
                userToken = userToken.Substring(userToken.IndexOf(':') + 8);
                return ClientSingleton.GetInstance.ClientMap.ContainsKey(userToken);
            }

            return false;
        }

        public bool IsRegistered(User user)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM users";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == user.Username)
                {
                    return true;
                }
            }
            reader.Close();
            return false;
        }

        public string AquirePackage(string usertoken, string packageId)
        {
            User user;
            ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);

            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            List<string> cardIds = null;
            string sqlAllPackages = "SELECT * FROM packages";
            using NpgsqlCommand cmdPackages = new NpgsqlCommand(sqlAllPackages, con);
            using NpgsqlDataReader reader = cmdPackages.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == packageId)
                {
                    try
                    {
                        cardIds = JsonSerializer.Deserialize<PackageDTO>(reader.GetString(1)).CardIds;
                    }
                    catch (Exception e)
                    {
                        cardIds = null;
                    }
                }
            }
            reader.Close();

            CardPackage pack = new CardPackage();
            string sqlAllCards = "SELECT * FROM cards";
            using NpgsqlCommand cmdCards = new NpgsqlCommand(sqlAllCards, con);
            using NpgsqlDataReader readerCards = cmdCards.ExecuteReader();
            while (readerCards.Read())
            {
                foreach (var cardId in cardIds)
                {
                    if (readerCards.GetString(0) == cardId)
                    {
                        CardDTO cardDto = new CardDTO(readerCards.GetString(0), readerCards.GetString(1), readerCards.GetString(2), readerCards.GetString(3), readerCards.GetDouble(4));
                        Card card = cardDto.ToCard();
                        pack.AddCard(card);
                    }
                }
            }
            user.AddCardsToStack(pack);
            readerCards.Close();
            return "POST OK";
        }
    }
}
