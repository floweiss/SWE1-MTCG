using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class ScoreService : IScoreService
    {
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

        public string ShowScore()
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            /*foreach (var user in ClientSingleton.GetInstance.ClientMap.Values)
            {
                scores.Add(user.Username, user.ELO);
            }*/

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
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS userdata(token VARCHAR(255), coins INTEGER, elo INTEGER, deck VARCHAR(255), stack VARCHAR(800))";
            cmd.ExecuteNonQuery();

            string sqlCheckUser = "SELECT * FROM userdata";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckUser, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            bool updateUser = false;
            while (reader.Read())
            {
                string userName = reader.GetString(0).Substring(0, reader.GetString(0).IndexOf('-'));
                scores.Add(userName, reader.GetInt32(2));
            }
            reader.Close();

            int place = 1;
            Dictionary<string, string> scoresDictionary = new Dictionary<string, string>();
            foreach (var score in scores.OrderByDescending(key => key.Value))
            {
                scoresDictionary.Add(place + ". Place", score.Key + " with " + score.Value + " ELO");
                place++;
            }

            return JsonSerializer.Serialize(scoresDictionary);
        }
    }
}
