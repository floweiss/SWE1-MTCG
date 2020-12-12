using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.DataTransferObject;
using System.Configuration;

namespace SWE1_MTCG.Services
{
    public class PackageService : IPackageService
    {
        private string _cs = "Host=localhost;Username=postgres;Password=postgres123;Database=postgres";
        
        public string CreatePackage(PackageDTO package)
        {
            using NpgsqlConnection con = new NpgsqlConnection(_cs);
            con.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS packages(packageid VARCHAR(255), cardids VARCHAR(255))";
            cmd.ExecuteNonQuery();

            string sqlCheckCards = "SELECT * FROM packages";
            using NpgsqlCommand cmdCheckUser = new NpgsqlCommand(sqlCheckCards, con);
            using NpgsqlDataReader reader = cmdCheckUser.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) == package.PackageId)
                {
                    return "POST ERR - Package already exists";
                }
            }
            reader.Close();

            var sql = "INSERT INTO packages (packageid, cardids) VALUES (@packageid, @cardids)";
            using (NpgsqlCommand cmdPrepared = new NpgsqlCommand(sql, con))
            {
                cmdPrepared.Parameters.AddWithValue("packageid", package.PackageId);
                cmdPrepared.Parameters.AddWithValue("cardids", JsonSerializer.Serialize(package));
                cmdPrepared.ExecuteNonQuery();
            }

            con.Close();
            return "POST OK by DB";
        }

        public string DeletePackage(CardPackage package)
        {
            throw new NotImplementedException();
        }
    }
}
