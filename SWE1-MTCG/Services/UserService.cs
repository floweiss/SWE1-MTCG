﻿using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;
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
                        string userToken = "{" + user.Username + "}-mtcgToken";
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
    }
}
