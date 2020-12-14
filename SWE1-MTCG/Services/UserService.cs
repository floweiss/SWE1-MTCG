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
        private IUserDataService _userDataService = new UserDataService();
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

        public string Register(User user)
        {
            // http://zetcode.com/csharp/postgresql/
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255), fullname VARCHAR(255), bio VARCHAR(255), image VARCHAR(255))";
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

            var sql = "INSERT INTO users (username, password, fullname, bio, image) VALUES (@username, @password, @fullname, @bio, @image)";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sql, con))
            {
                cmdPrepared.Parameters.AddWithValue("username", user.Username);
                cmdPrepared.Parameters.AddWithValue("password", user.HashedPW);
                cmdPrepared.Parameters.AddWithValue("fullname", user.Username.ToUpper());
                cmdPrepared.Parameters.AddWithValue("bio", "Default Bio");
                cmdPrepared.Parameters.AddWithValue("image", "Default Image");
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
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255), fullname VARCHAR(255), bio VARCHAR(255), image VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM users";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            string userToken = "";
            while (reader.Read())
            {
                if (reader.GetString(0) == user.Username)
                {
                    if (reader.GetString(1) == user.HashedPW)
                    {
                        userToken = user.Username + "-mtcgToken";
                        ClientSingleton.GetInstance.ClientMap.AddOrUpdate(userToken, user, (key, oldValue) => user);
                        _userDataService.LoadUserData(user, userToken);
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
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255), fullname VARCHAR(255), bio VARCHAR(255), image VARCHAR(255))";
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
            if (user.Coins == 0)
            {
                return "POST ERR - No coins";
            }

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
            if (cardIds == null || cardIds.Count == 0)
            {
                return "POST ERR - Invalid PackageID";
            }

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
            _userDataService.PersistUserData(user, usertoken);
            return "POST OK";
        }

        public string ShowBio(string username)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255), fullname VARCHAR(255), bio VARCHAR(255), image VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM users";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == username)
                {
                    Dictionary<string, string> userList = new Dictionary<string, string>();
                    userList.Add("Username", reader.GetString(0));
                    userList.Add("Full Name", reader.GetString(2));
                    userList.Add("Bio", reader.GetString(3));
                    userList.Add("Image", reader.GetString(4));

                    return JsonSerializer.Serialize(userList);
                }
            }
            reader.Close();
            return "GET ERR - User does not exist";
        }

        public string EditBio(UserBioDTO userBio, string user)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            string sqlUpdate =
                "UPDATE users SET fullname = @fullname, bio = @bio, image = @image WHERE username = @findUser";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sqlUpdate, con))
            {
                cmdPrepared.Parameters.AddWithValue("fullname", userBio.Name);
                cmdPrepared.Parameters.AddWithValue("bio", userBio.Bio);
                cmdPrepared.Parameters.AddWithValue("image", userBio.Image);
                cmdPrepared.Parameters.AddWithValue("findUser", user);
                cmdPrepared.ExecuteNonQuery();
            }

            return "PUT OK by DB";
        }
    }
}
