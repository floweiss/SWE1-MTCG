using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace SWE1_MTCG.Services
{
    public class UserService : IUserService
    {
        public string Register(User user)
        {
            // http://zetcode.com/csharp/postgresql/
            var cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            // ToDo: Replace with check for user
            cmd.CommandText = "DROP TABLE IF EXISTS users";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(username VARCHAR(255), password VARCHAR(255))";
            cmd.ExecuteNonQuery();

            var sql = "INSERT INTO users (username, password) VALUES (@username, @password)";
            using (var cmdPrepared = new NpgsqlCommand(sql, con))
            {
                cmdPrepared.Parameters.AddWithValue("username", user.Username);
                cmdPrepared.Parameters.AddWithValue("password", user.Base64PW);
                cmdPrepared.ExecuteNonQuery();
            }

            return "POST OK by DB";
        }

        public User Login(User user)
        {
            return user;
        }

        public bool isLoggedIn(User user)
        {
            return true;
        }

        public bool IsRegistered(User user)
        {
            return true;
        }
    }
}
