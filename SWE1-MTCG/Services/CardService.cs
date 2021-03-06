﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Services
{
    public class CardService : ICardService
    {
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

        public string CreateCard(CardDTO card)
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
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS cards(id VARCHAR(255), name VARCHAR(255), cardtype VARCHAR(255), element VARCHAR(255), damage DOUBLE PRECISION)";
            cmd.ExecuteNonQuery();

            string sqlCheckCards = "SELECT * FROM cards";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckCards, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == card.Id)
                {
                    return "POST ERR - Card already exists";
                }
            }
            reader.Close();

            var sql = "INSERT INTO cards (id, name, cardtype, element, damage) VALUES (@id, @name, @cardtype, @element, @damage)";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sql, con))
            {
                cmdPrepared.Parameters.AddWithValue("id", card.Id);
                cmdPrepared.Parameters.AddWithValue("name", card.Name);
                cmdPrepared.Parameters.AddWithValue("cardtype", card.CardType);
                if (!string.IsNullOrWhiteSpace(card.Element))
                {
                    cmdPrepared.Parameters.AddWithValue("element", card.Element);
                }
                else
                {
                    cmdPrepared.Parameters.AddWithValue("element", "");
                }
                cmdPrepared.Parameters.AddWithValue("damage", card.Damage);
                cmdPrepared.ExecuteNonQuery();
            }

            con.Close();
            return "POST OK - Card created ";
        }

        public string DeleteCard(Card card)
        {
            throw new NotImplementedException();
        }

        public string ShowCards(string usertoken)
        {
            User user = null;
            List<string> cardList = new List<string>();
            if (ClientSingleton.GetInstance.ClientMap.ContainsKey(usertoken))
            {
                ClientSingleton.GetInstance.ClientMap.TryGetValue(usertoken, out user);
            }

            foreach (var card in user.Stack.CardCollection)
            {
                cardList.Add(card.ToCardString());
            }

            return JsonSerializer.Serialize(cardList);
        }
    }
}
