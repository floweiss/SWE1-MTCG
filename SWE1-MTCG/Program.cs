using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;
using Npgsql;

namespace SWE1_MTCG
{
    class Program
    {
        public static void Main(string[] args)
        {
            ApiService apiService = new ApiService();
            Webserver server = new Webserver(apiService);
            server.Start();


            // DB TEST
            /*var cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "DROP TABLE IF EXISTS cars";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE cars(id SERIAL PRIMARY KEY, 
                    name VARCHAR(255), price INT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Audi',52642)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("Done");*/
        }
    }
}